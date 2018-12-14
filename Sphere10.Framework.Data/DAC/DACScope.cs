//-----------------------------------------------------------------------
// <copyright file="DACScope.cs" company="Sphere 10 Software">
//
// Copyright (c) Sphere 10 Software. All rights reserved. (http://www.sphere10.com)
//
// Distributed under the MIT software license, see the accompanying file
// LICENSE or visit http://www.opensource.org/licenses/mit-license.php.
//
// <author>Herman Schoenfeld</author>
// <date>2018</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Sphere10.Framework.Data {
	
	public sealed class DACScope : ScopeContext<DACScope> {
        private const string DefaultContextPrefix = "EA9CC911-C209-42B9-B113-84562706145D";
        private const string ContextNameTemplate = "DACScope:{0}:{1}";
        private readonly bool _scopeOwnsConnection;
        private bool _scopeOwnsTransaction;
	    private RestrictedConnection _connection;
        private RestrictedTransaction _transaction;
	    private DACScope _transactionOwner;
	    private bool _withinSystemTransactionScope;
	    private bool _scopeHasOpenTransaction;
	    private bool _voteRollback;
	    private Auto? _autoAction;

        internal DACScope(IDAC dac, ScopeContextPolicy policy, bool openConnection, string contextPrefix = DefaultContextPrefix, Auto? autoAction = null)
            : base(string.Format(ContextNameTemplate, contextPrefix, dac.ConnectionString), policy) {
		    if (dac == null)
                throw new ArgumentNullException("dac");
		    DAC = dac;
		    if (IsRootScope) {
                _connection = new RestrictedConnection( DAC.CreateConnection());
                if (openConnection)
                    _connection.Open();
                _scopeOwnsConnection = true;
		        _transaction = null;
		        _scopeOwnsTransaction = false;
		        _transactionOwner = null;
		    } else {   
                if (RootScope._connection == null)
                    throw new SoftwareException("Internal Error: RootScope DAC had null connection");

                _connection = RootScope._connection;
		        _transaction = RootScope._transaction;
		        _transactionOwner = RootScope._transactionOwner;
                _scopeOwnsTransaction = false;
                _scopeOwnsConnection = false;
                if (openConnection && _connection.State.IsIn(ConnectionState.Closed, ConnectionState.Broken))
                    _connection.Open();
            }
            _withinSystemTransactionScope = System.Transactions.Transaction.Current != null;
            if (_scopeOwnsConnection && _withinSystemTransactionScope)
                DAC.EnlistInSystemTransaction(_connection.DangerousInternalConnection, System.Transactions.Transaction.Current);
            _voteRollback = false;
		    _scopeHasOpenTransaction = false;
            _autoAction = autoAction;
        }

        public IDAC DAC { get; internal set; }

		public RestrictedConnection Connection { get { return _connection; } }

        public RestrictedTransaction Transaction { get { return _transaction; } }

        /// <summary>
        /// Whether or not DAC commands are subject to a DACSCope transaction or system transaction
        /// </summary>
        public bool ParticipatesWithinTransaction { get { return _withinSystemTransactionScope || _transaction != null; } }

	    public void EnlistInSystemTransaction() {
	        if (_transaction != null) {
	            throw new SoftwareException("Unable to enlist in system transaction as DACScope has declared it's own transaction");
	        }

	        var systemTransaction = System.Transactions.Transaction.Current;
	        if (systemTransaction == null) {
	            throw new SoftwareException("No system transaction has been declared (i.e. TransactionScope)");
	        }
            DAC.EnlistInSystemTransaction(_connection.DangerousInternalConnection, systemTransaction);
            _withinSystemTransactionScope = true;	        
	    }
       
	    public void BeginTransaction(IsolationLevel? isolationLevel = null) {
	        if (isolationLevel == null)
	            isolationLevel = DAC.DefaultIsolationLevel;

	        if (_withinSystemTransactionScope) {
	            _withinSystemTransactionScope = System.Transactions.Transaction.Current != null; // may have been removed
	            if (_withinSystemTransactionScope) {
	                throw new SoftwareException("DACScope transactions cannot be used a System.Transactions.TransactionScope.");
	            }
	        }

	        if (_transaction == null) {
	            _transaction = _connection.BeginTransactionInternal(isolationLevel.Value);
	            _scopeOwnsTransaction = true;
	            _transactionOwner = this;
                // make sure child scopes can see this transaction and owner by placing it in rootscope (cleaned up on exit)
                RootScope._transaction = _transaction; 
	            RootScope._transactionOwner = this;
	        } else {
                // if this is the parent scope, error
                if (_scopeOwnsTransaction)
                    throw new SoftwareException("Scope has already created a transaction");

	            // parent scope already defined a transaction, so use it
                if (isolationLevel.Value > _transaction.IsolationLevel)
                    throw new SoftwareException("A transaction already exists with lower isolation level. Requested = {0}, Current = {1}", isolationLevel, _transaction.IsolationLevel);
	            _scopeOwnsTransaction = false;
	        }
            _scopeHasOpenTransaction = true;
	    }

	    public void Rollback() {
            if (_withinSystemTransactionScope) {
                throw new SoftwareException("DACScope transactions cannot be used within a System.Transactions.TransactionScope.");
            }
            if (_transaction == null)
                throw new SoftwareException("No transaction has been declared");

	        if (_scopeOwnsTransaction) {
	            _transaction.RollbackInternal();                
	        } else {
	            _transactionOwner._voteRollback = true;
	        }
            CloseTransaction();
	    }

	    public void Commit() {
	        if (_withinSystemTransactionScope) {
	            throw new SoftwareException("DACScope transactions cannot be used a System.Transactions.TransactionScope.");
	        }

	        if (_transaction == null)
	            throw new SoftwareException("No transaction has been declared");

	        if (_scopeOwnsTransaction) {
	            if (!_voteRollback) {
	                _transaction.CommitInternal();
	            } else {
	                _transaction.RollbackInternal();
	            }                
	        }
            CloseTransaction();
	    }

	    protected override void OnScopeEnd(DACScope rootScope, bool inException) {
	        if (!inException && _autoAction.HasValue) {
	            switch (_autoAction.Value) {
	                    case Auto.Commit:
                        Commit();
                        break;
                        case Auto.Rollback:
                        Rollback();
	                    break;
                    default:
	                    throw new NotSupportedException(_autoAction.Value.ToString());
	            }
	        }            
	        var scopeWasInOpenTransaction = _scopeHasOpenTransaction;
	        var errors = new List<Exception>();
	        if (_transaction != null && _scopeOwnsTransaction) {	            
	                Tools.Exceptions.ExecuteIgnoringException(CloseTransaction, errors);
	        }

	        if (_scopeOwnsConnection) {
	            Tools.Exceptions.ExecuteIgnoringException(_connection.CloseInternal, errors);
	            Tools.Exceptions.ExecuteIgnoringException(_connection.DisposeInternal, errors);
	            _connection = null;
	        }

            if (scopeWasInOpenTransaction && !inException) {
                errors.Add( new SoftwareException("DACScope transaction was left open. Please call Commit or Rollback explicitly to close the transaction."));
	        }

	        if (!inException) {
                switch (errors.Count) {
                    case 0:
                        break;
                    case 1:
                        throw errors[0];
                    default:
                        throw new AggregateException(errors);
                }
	        }
	    }

	    public new static DACScope GetCurrent(string connectionString) {
	        return ScopeContext<DACScope>.GetCurrent(string.Format(ContextNameTemplate, DefaultContextPrefix, connectionString));
	    }

        public static DACScope GetCurrent(string connectionString, string contextPrefix) {
            return ScopeContext<DACScope>.GetCurrent(string.Format(ContextNameTemplate, contextPrefix, connectionString));
        }

        /// <summary>
        /// Closes the transaction. Behaviour is different for root scope, owning scope & child scope.
        /// </summary>
	    private void CloseTransaction() {
            // indicate local scope has closed txn
	        _scopeHasOpenTransaction = false;
            
            // owning scope needs to do extra
            if (_scopeOwnsTransaction) {
                
                // dispose transaction and remove from root scope
                if (_transaction != null) {
                    _transaction.DangerousInternalTransaction.Dispose();
                    _transaction = null;
                    RootScope._transaction = null;
                }

                // clear rollback flag
                _voteRollback = false;
                RootScope._voteRollback = false;

                // reset flags to allow consequtive txn's
                _scopeOwnsTransaction = false;
                _transactionOwner = null;
                RootScope._transactionOwner = null;
            }	        
	    }


	    public enum Auto {
	        Commit,
            Rollback
	    }

	}

}
