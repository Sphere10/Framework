//-----------------------------------------------------------------------
// <copyright file="Result.cs" company="Sphere 10 Software">
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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Text;

namespace Sphere10.Framework {
	/// <summary>
	///     Result of a business component method.
	/// </summary>
	[XmlRoot]
    [DataContract]
	public class Result {
		public Result() {
			ErrorMessages = new List<string>();
			InformationMessages = new List<string>();
		}

		[XmlIgnore]
        [IgnoreDataMember]
		public bool Success {
			get {
				return !Failure;
			}
		}

		[XmlIgnore]
        [IgnoreDataMember]
		public bool Failure {
			get {
				return ErrorMessages.Count > 0;
			}
		}

		[XmlIgnore]
        [IgnoreDataMember]
		public bool HasInformation {
			get { return InformationMessages.Count > 0; }
		}

		[XmlElement]
        [DataMember]
		public List<string> ErrorMessages { get; set; }

		[XmlElement]
        [DataMember]
		public List<string> InformationMessages { get; set; }


		public virtual void AddError(string message) {
			ErrorMessages.Add(message);
		}

		public virtual void AddError(string message, params object[] formatArgs) {
			ErrorMessages.Add(string.Format(message, formatArgs));
		}

		public virtual void AddException(Exception exception) {
			ErrorMessages.Add(exception.ToDiagnosticString());
		}

		public virtual Result Merge(Result result) {
			ErrorMessages.AddRange(result.ErrorMessages);
			InformationMessages.AddRange(result.InformationMessages);
		    return this;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			if (HasInformation)
				stringBuilder.Append(InformationMessages.ToDelimittedString("."));

			if (Failure) {
				if (HasInformation)
					stringBuilder.AppendLine();
				stringBuilder.Append(ErrorMessages.ToDelimittedString("."));
			}

			return stringBuilder.ToString();
		}

        [XmlIgnore]
        [IgnoreDataMember]
		public static Result Default {
			get { return new Result(); }
		}

        public static Result Error(params string[] errorMessages) {
            var result = new Result();
            foreach(var err in errorMessages)
                result.AddError(err);
            return result;
        }

    }

    /// <summary>
    ///     Result of a business component method which carries a payload object (usually a model/data object/etc).
    /// </summary>
    /// <typeparam name="T">The payload objects type.</typeparam>
    [XmlRoot]
	[DataContract]
	public class Result<T> : Result {
		private T _value;

		public Result() {
			Value = default(T);
		}

		public Result(string errMsg)
			: this() {
		}

		public Result(T @value)
			: this() {
			Value = @value;
		}



		public new virtual Result<T> Merge(Result result) {
			return base.Merge(result) as Result<T>;
		}

	    [XmlElement]
	    [DataMember]
	    public T Value {
		    get {
				// Removed 2017-02-22: causing json serialisation failure
				//if (Failure)
				//	throw new SoftwareException(this);
			    return _value;
		    }
		    set {
			    _value = value;
		    }
	    }

	    [XmlIgnore]
        [IgnoreDataMember]
		public new static Result<T> Default => new Result<T>();
    }
}
