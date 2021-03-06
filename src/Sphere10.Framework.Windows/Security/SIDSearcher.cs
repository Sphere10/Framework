//-----------------------------------------------------------------------
// <copyright file="SIDSearcher.cs" company="Sphere 10 Software">
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
using System.Text;

using System.Diagnostics;

using System.Security.Principal;
using System.IO;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace Sphere10.Framework.Windows.Security {


    public class SIDSearcher {
        List<SecurityIdentifier> _searchList;
        bool _findDanglingSIDs;
        IActionObserver _actionObserver;
        string _hostname;

        public SIDSearcher(string hostname, List<SecurityIdentifier> list, bool findDanglingSIDs)
            : this(hostname, list, findDanglingSIDs, SecurityTool.DefaultActionObserver) {
        }

        public SIDSearcher(
            string hostname,
            List<SecurityIdentifier> list,
            bool findDanglingSids,
            IActionObserver actionObserver) {
            Debug.Assert(hostname != null);
            Debug.Assert(list != null);
            Debug.Assert(actionObserver != null);
            Hostname = hostname;
            FindDanglingSIDs = findDanglingSids;
            SearchList = list;
            ActionObserver = actionObserver;
        }

        public string Hostname {
            get { return _hostname; }
            set { _hostname = value; }
        }

        public bool FindDanglingSIDs {
            get { return _findDanglingSIDs; }
            set { _findDanglingSIDs = value; }
        }

        public List<SecurityIdentifier> SearchList {
            get { return _searchList; }
            set { _searchList = value; }
        }

        public IActionObserver ActionObserver {
            get { return _actionObserver; }
            set { _actionObserver = value; }
        }

        public void SearchEntireRegistry() {
            if (Tools.WinTool.KeyExists(Hostname, "HKEY_CLASSES_ROOT")) {
                SearchRegistry("HKEY_CLASSES_ROOT", true);
            }
            if (Tools.WinTool.KeyExists(Hostname, "HKEY_CURRENT_USER")) {
                SearchRegistry("HKEY_CURRENT_USER", true);
            }

            if (Tools.WinTool.KeyExists(Hostname, "HKEY_LOCAL_MACHINE")) {
                SearchRegistry("HKEY_LOCAL_MACHINE", true);
            }

            if (Tools.WinTool.KeyExists(Hostname, "HKEY_USERS")) {
                SearchRegistry("HKEY_USERS", true);
            }

            if (Tools.WinTool.KeyExists(Hostname, "HKEY_CURRENT_CONFIG")) {
                SearchRegistry("HKEY_CURRENT_CONFIG", true);
            }

            if (Tools.WinTool.KeyExists(Hostname, "HKEY_DYN_DATA")) {
                SearchRegistry("HKEY_DYN_DATA", true);
            }
        }

        public void SearchRegistry(string rootKey, bool recursive) {
            try {
                ActionObserver.NotifyAction("Searching for SID usage", "Registry", rootKey, Hostname);
                RegistryKey key = Tools.WinTool.OpenKey(Hostname, rootKey);
                try {
                    foreach (SecurityIdentifier match in SearchObjectSecurity(key.GetAccessControl(AccessControlSections.All))) {
                        ActionObserver.NotifyInformation(
                            "RegistryKey '{0}' referenced '{1}'", rootKey, match.ToString()
                        );
                    }
                } catch (Exception error) {
                    ActionObserver.NotifyError("Unable to search SIDs on registry key '{0}' due to error '{1}'", rootKey, error.Message);
                }

                if (recursive) {
                    foreach (string subKey in Tools.WinTool.GetSubKeys(Hostname, rootKey)) {
                        if (Tools.WinTool.KeyExists(Hostname, subKey)) {
                            SearchRegistry(subKey, recursive);
                        }  else {
                            ActionObserver.NotifyWarning("Unable to access key '{0}'", subKey);
                        }
                    }
                }
            } catch (Exception error) {
                ActionObserver.NotifyError("Unable to search SIDs on registry key '{0}' due to error='{1}'", rootKey, error.Message);
            }
        }

        public void SearchDirectory(string directory, bool recursive) {
            Debug.Assert(Directory.Exists(directory));
            try {
                ActionObserver.NotifyAction("Searching for SID usage", "Directory", directory, string.Empty);
                try {

                    foreach (SecurityIdentifier match in SearchObjectSecurity(Directory.GetAccessControl(directory, AccessControlSections.All))) {
                        ActionObserver.NotifyInformation(
                            "Directory '{0}' referenced '{1}'", directory, match.ToString()
                        );
                    }
                } catch (Exception error) {
                    ActionObserver.NotifyError("Unable to search SIDs on directory '{0}' due to error '{1}'", directory, error.Message);
                }

                try {
                    // process files
                    foreach (string file in Directory.GetFiles(directory)) {
                        SearchFile(file);
                    }
                } catch (Exception error) {
                    ActionObserver.NotifyError("Unable to search SIDs on directory '{0}' due to error '{1}'", directory, error.Message);
                }


                if (recursive) {

                    foreach (string subDirectory in Directory.GetDirectories(directory)) {
                        SearchDirectory(subDirectory, recursive);
                    }
                }
            } catch (Exception error) {
                ActionObserver.NotifyError("Unable to search SIDs on directory '{0}' due to error '{1}'", directory, error.Message);
            }
        }

        public void SearchFile(string file) {
            Debug.Assert(File.Exists(file));
            ActionObserver.NotifyAction("Searching for SID usage", "File", file, string.Empty);
            try {
                foreach (SecurityIdentifier match in SearchObjectSecurity(File.GetAccessControl(file, AccessControlSections.All))) {
                    ActionObserver.NotifyInformation(
                        "File '{0}' referenced '{1}'", file, match.ToString()
                    );
                }
            } catch (Exception error) {
                ActionObserver.NotifyError("Unable to search SIDs on file '{0}' due to error '{1}'", file, error.Message);
            }

        }


        public List<SecurityIdentifier> SearchObjectSecurity(CommonObjectSecurity security) {
            List<SecurityIdentifier> retval = new List<SecurityIdentifier>();
            string sddl = security.GetSecurityDescriptorSddlForm(AccessControlSections.All);
            foreach (SecurityIdentifier sid in SearchList) {
                if (sddl.Contains(sid.ToString())) {
                    retval.Add(sid);
                }
            }
            return retval;
        }


    }

}
