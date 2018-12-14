//-----------------------------------------------------------------------
// <copyright file="PathTool.cs" company="Sphere 10 Software">
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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sphere10.Framework;

#if __WP8__
using Windows.Security;
#endif

namespace Tools {


        public static class Paths {
            public readonly static string DirectorySeparatorString;

            static Paths() {
                DirectorySeparatorString = new string(new [] {Path.DirectorySeparatorChar});
            }

            public static bool IsWellFormedDirectoryPath(string path) {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                var driveCheck = new Regex(@"^[a-zA-Z]:\\$");
                if (!driveCheck.IsMatch(path.Substring(0, 3)))
                    return false;
                var strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
                strTheseAreInvalidFileNameChars += @":/?*" + "\"";
                var containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");    
                if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                    return false;

                return true;
            }

            public static string GetParentDirectory(string path) {
                var splits = path.Split(Path.DirectorySeparatorChar);
                if (splits.Length < 2)
                    return null;
                return splits[splits.Length - 2];
            }

            public static string GetRelativePath(string basePath, string absolutePath) {
                char separator = Path.DirectorySeparatorChar;
                if (string.IsNullOrEmpty(basePath))
                    basePath = Directory.GetCurrentDirectory();
                var returnPath = "";
                var commonPart = "";
                var basePathFolders = basePath.Split(separator);
                var absolutePathFolders = absolutePath.Split(separator);
                var i = 0;
                while (i < basePathFolders.Length & i < absolutePathFolders.Length) {
                    if (basePathFolders[i].ToLower() == absolutePathFolders[i].ToLower()) {
                        commonPart += basePathFolders[i] + separator;
                    } else {
                        break;
                    }
                    i += 1;
                }
                if (commonPart.Length > 0) {
                    var parents = basePath.Substring(commonPart.Length - 1).Split(separator);
                    foreach (var parentDir in parents) {
                        if (!string.IsNullOrEmpty(parentDir))
                            returnPath += ".." + separator;
                    }
                }
                returnPath += absolutePath.Substring(commonPart.Length);
                return returnPath;
            }

            /// <summary>
            /// Resolves tokens within a path string 
            /// </summary>
            /// <param name="pathTemplate"></param>
            /// <returns></returns>
            public static string ResolvePathTemplate(string pathTemplate) {
                return ResolvePathTemplate(pathTemplate, DefaultPathTokenResolver);
            }

            public static string ResolvePathTemplate(string pathTemplate, Func<string, string> tokenResolver) {
                return Tools.Text.FormatEx(pathTemplate, (token) => tokenResolver(token) ?? DefaultPathTokenResolver(token));
            }

            public static string DefaultPathTokenResolver(string token) {
                switch (token) {
                    case "\\":
                    case "/":
                        return DirectorySeparatorString;
                    case "Programs":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                    case "Favorites":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
                    case "ApplicationData":
                        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    case "Personal":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    case "StartMenu":
                        return Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                    case "Startup":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Startup);
#if !__WP8__
                    case "CommonApplicationData":
                        return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    case "CommonProgramFiles":
                        return Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                    case "Cookies":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
                    case "Desktop":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    case "DesktopDirectory":
                        return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    case "History":
                        return Environment.GetFolderPath(Environment.SpecialFolder.History);
                    case "InternetCache":
                        return Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                    case "LocalApplicationData":
                        return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    case "MyComputer":
                        return Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                    case "MyDocuments":
                        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    case "MyMusic":
                        return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                    case "MyPictures":
                        return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    case "ProgramFiles":
                        return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    case "Recent":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Recent);
                    case "SendTo":
                        return Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
                    case "System":
                        return Environment.GetFolderPath(Environment.SpecialFolder.System);
                    case "Templates":
                        return Environment.GetFolderPath(Environment.SpecialFolder.Templates);
#endif
                }
                return token;
            }

            public static string GenerateTempFilename(string ext = null) {
                return Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToStrictAlphaString(),
                    ext != null ? (ext.StartsWith(".") ? ext : "." + ext) : string.Empty);
            }
        }
    }

