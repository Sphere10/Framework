//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Sphere 10 Software">
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
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Sphere10.Framework
{
	public static class StringExtensions {
		private static readonly char[] InvalidFilePathChars = ("*?/\\:" + Path.GetInvalidPathChars().ToDelimittedString(string.Empty)).ToCharArray();
		private static readonly Regex AlphabetCheckRegex = new Regex("^[a-zA-Z0-9]*$");

		#region General 

        public static IEnumerable<int> IndexOfAll(this string sourceString, string subString) {
            return Regex.Matches(sourceString, Regex.Escape(subString)).Cast<Match>().Select(m => m.Index);
        }

        public static IEnumerable<string> SplitAndKeep(this string @string, char[] delims, StringSplitOptions options = StringSplitOptions.None) {
            var start = 0;
            var index = 0;

            while ((index = @string.IndexOfAny(delims, start)) != -1) {
                index = Interlocked.Exchange(ref start, index + 1);

                if (start - index - 1 > 0 || !options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
                    yield return @string.Substring(index, start - index - 1);

                yield return @string.Substring(start - 1, 1);
            }

            if (options.HasFlag(StringSplitOptions.RemoveEmptyEntries)) {
                if (start < @string.Length) {
                    yield return @string.Substring(start);
                }
            } else {
                yield return @string.Substring(start);
            }
        }

        public static string Clip(this string text, int maxLength, string cap = "...") {
            if (string.IsNullOrEmpty(text))
                return text;

            if (text.Length <= maxLength) {
                return text;
            }
            if (text.Length < cap.Length || maxLength < cap.Length)
                return text.Substring(0, maxLength);
            return text.Substring(0, maxLength - cap.Length) + cap;
        }

	    public static bool ContainsAnySubstrings(this string text, params string[] substrings ) {
			return substrings.Any(text.Contains);
		}

		public static bool IsAlphabetic(this string text) {
			return AlphabetCheckRegex.IsMatch(text);
		}

		public static string Tabbify(this string text, int tabs = 1) {
			var tabbedText = new StringBuilder();
			var tabBuilder = new StringBuilder();
			for (var i = 0; i < tabs; i++)
				tabBuilder.Append("\t");

			var tabSpace = tabBuilder.ToString();

			foreach (var line in GetLines(text))
				tabbedText.Append(String.Format("{0}{1}{2}", tabSpace, line, Environment.NewLine));

			return tabbedText.ToString();
		}

		public static string RemoveNonAlphaNumeric(this string @string) {
			var sb = new StringBuilder();
			for (int i = 0; i < @string.Length; i++) {
				char c = @string[i];
				if (Char.IsLetterOrDigit(c))
					sb.Append(c);
			}
			return sb.ToString();
		}

		public static string RemoveWhitespace(this string @string) {
			var sb = new StringBuilder();
			for (int i = 0; i < @string.Length; i++) {
				char c = @string[i];
				if (!char.IsWhiteSpace(c))
					sb.Append(c);
			}
			return sb.ToString();
		}

#if !__WP8__
		public static byte[] ToAsciiByteArray(this string asciiString) {
			var encoding = new ASCIIEncoding();
			return encoding.GetBytes(asciiString);
		}
#endif

		public static IEnumerable<string> GetLines(this string str, bool removeEmptyLines = false) {
			return str.Split(
				new[] { "\r\n", "\r", "\n" },
				removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None
			);
		}

		public static string TrimDelimitters(this String inputString, params string[] delimitters) {
			StringBuilder sbuilder = new StringBuilder(inputString);
			delimitters.ForEach(d => sbuilder.Replace(d, String.Empty));
			return sbuilder.ToString();
		}

		static public string EncapsulateWith(this string value, string prePostFix) {
			return value.EncapsulateWith(prePostFix, prePostFix);
		}

		static public string EncapsulateWith(this string value, string preFix, string postFix) {
			if (!value.StartsWith(preFix))
				value = preFix + value;

			if (!value.EndsWith(postFix))
				value = value + postFix;

			return value;
		}

		public static byte[] FromHexStringToByteArray(this String hex) {
			if (String.IsNullOrEmpty(hex))
				return new byte[0];

			var offset = 0;
			if (hex.StartsWith("0x"))
				offset = 2;

			var numberChars = (hex.Length - offset) / 2;

			var bytes = new byte[numberChars];
			using (var stringReader = new StringReader(hex)) {
				for (var i = 0; i < numberChars; i++) {
					if (i >= offset)
						bytes[i - offset] = Convert.ToByte(new string(new char[2] { (char)stringReader.Read(), (char)stringReader.Read() }), 16);
				}
			}
			return bytes;
		}

		public static string FormatWith(this string _string, params object[] _params) {
			if (_params == null || !_params.Any())
				return _string;

			return String.Format(_string, _params);
		}

		/// <summary>
		/// Parse a string into an enumeration
		/// </summary>
		/// <typeparam name="TEnum">The Enumeration type to cast to</typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static TEnum ParseEnum<TEnum>(this string source) {
			Type t = typeof(TEnum);

			if (!t.IsEnum)
				throw new ArgumentException("TEnum must be a valid Enumeration", "TEnum");

			return (TEnum)Enum.Parse(t, source);
		}

		public static string EscapeBraces(string text) {
			Debug.Assert(text != null);
			return text.Replace("{", "{{").Replace("}", "}}");
		}

		public static string TrimToLength(string @string, int len, string append = "...") {
			if (@string == null || @string.Length <= len)
				return @string;

			return @string.Substring(0, len) + append;
		}

		public static string TrimWordsToLength(this string @string, int len, string append = "...") {
			if (null == @string || @string.Length <= len)
				return @string;

			var match = new Regex(@"[^\s]\s", RegexOptions.RightToLeft).Match(@string, len - append.Length + 1);
			return match.Success
					   ? @string.Substring(0, match.Index + 1) + append
					   : @string.Substring(0, len - append.Length) + append;
		}


        public static string MakeStartWith(this string @string, string startsWith, bool caseSensitive = true) {
            var cmpString = caseSensitive ? @string : @string.ToUpperInvariant();
            var cmpStartsWith = caseSensitive ? @startsWith : @startsWith.ToUpperInvariant();

            if (cmpString.StartsWith(cmpStartsWith))
                return @string;
            return startsWith + @string;
        }

		public static string MakeEndWith(this string @string, string endsWith, bool caseSensitive = true) {
            var cmpString = caseSensitive ? @string : @string.ToUpperInvariant();
            var cmpEndsWith = caseSensitive ? endsWith : endsWith.ToUpperInvariant();

            if (cmpString.EndsWith(cmpEndsWith))
                return @string;
			return @string + endsWith;
		}


        public static string TrimStart(this string @string, string substring, bool caseSensitive = true) {
            var cmpString = caseSensitive ? @string : @string.ToUpperInvariant();
            var cmpSubstring = caseSensitive ? substring : substring.ToUpperInvariant();

            if (cmpString.StartsWith(cmpSubstring))
                return @string.Substring(substring.Length);

            return @string;
        }

        public static string TrimEnd(this string @string, string substring, bool caseSensitive = true) {
            var cmpString = caseSensitive ? @string : @string.ToUpperInvariant();
            var cmpSubstring = caseSensitive ? substring : substring.ToUpperInvariant();

            if (cmpString.EndsWith(cmpSubstring))
                return @string.Substring(0, @string.Length - substring.Length);
            return @string;
        }

#if !__WP8__
		public static string ToBase64(this string str) {
			byte[] encbuff = Encoding.UTF8.GetBytes(str);
			return Convert.ToBase64String(encbuff);
		}

		public static string FromBase64(this string str) {
			byte[] decbuff = Convert.FromBase64String(str);
			return Encoding.UTF8.GetString(decbuff);
		}
#endif

		public static string GetRegexMatch(this string input, string pattern, string group) {
			var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
			return match.Groups[@group].Success ? match.Groups[@group].Value : null;
		}

		public static string NullIfEmpty(this string text) {
			if (string.IsNullOrEmpty(text))
				return null;
			return text;
		}


		#endregion

		#region Paths

		public static string ToCleanPathString(this string path) {
			path = path.Trim();
			if (path.Length == 3 && path[2] == '\\') {
#warning its drive so dont trim back slash from it Make this code better.
				return path;
			} else {
				return path.Trim().TrimEnd(Path.DirectorySeparatorChar);
			}
		}

		public static string ToPathSafe(this string path) {
			Array.ForEach(
				InvalidFilePathChars,
				  c => path = path.Replace(c.ToString(), String.Empty)
			);
			return path == ".." ? string.Empty : path;
		}

		static public string EscapeCSV(this string value) {
			if (String.IsNullOrEmpty(value))
				return value;
			else
				return value.Replace("\"", "\"\"");
		}

		public static bool ToBool(this string value) {
			bool retval = false;
			switch (value.ToUpper()) {
				case "CHECKED":
				case "1":
				case "Y":
				case "YES":
				case "OK":
				case "TRUE":
				case "GRANTED":
				case "PERMISSION GRANTED":
				case "APPROVED":
					retval = true;
					break;
			}
			return retval;
		}

		#endregion

		#region Escaping / Unescaping 

		static public string UrlEncoded(this string str) {
			return Tools.Url.EncodeUrlData(str);	
		}

		static public string UrlDecoded(this string str) {
			return Tools.Url.DecodeUrlData(str);
		}

		public static string EscapeJavascriptString(this string value) {
			return value.Replace("'", "\\'").Replace("\"", "\\\"");
		}

		public static string EscapeJavaScript(this string s) {
			StringBuilder sb = new StringBuilder(s);
			sb.Replace("\\", "\\\\");
			sb.Replace("\"", "\\\"");
			sb.Replace("\'", "\\'");
			sb.Replace("\t", "\\t");
			sb.Replace("\r", "\\r");
			sb.Replace("\n", "\\n");
			return sb.ToString();
		}

		/// <summary>
		/// Escapes a string safely for javascript interpretation, avoiding HTML embedding and character encoding issues by
		/// unicode-escaping all characters not guaranteed to be safe.
		/// </summary>
		public static string EscapeHtmlJavacript(this string @string) {
			/*
			 * Allowed are printable US-ASCII characters, minus Javascript-unsafe, minus XML/HTML-unsafe.
			 * 
			 * Skipped as javascript-unsafe:  " ' \
			 * Skipped as XML/HTML-unsafe:    Space & " ' < >
			 */
			if (null == @string) {
				return null;
			} else {
				return Regex.Replace(@string, @"[^\u0021\u0023-\u0025\u0028-\u003B\u003D\u003F-\u005B\u005D-\u007E]",
									 new MatchEvaluator(delegate(Match match) {
					byte[] bb = Encoding.Unicode.GetBytes(match.Value);
					return String.Format(@"\u{0}{1}", bb[1].ToString("x2"), bb[0].ToString("x2"));
				}));
			}
		}

		public static string ReplaceNewLinesWithBR(string @string) {
			return @string.Replace("\n", "<br/>");
		}

        public static Dictionary<string, string> ParseQueryString(this string encdata) {
			return Tools.Url.ParseQueryString(encdata);
		}

		static public string EscapeSQL(this string unsafeString) {
			return unsafeString.Replace("'", "''");
		}

		#endregion

		#region Casing

		public static string ToSentenceCase(this string value) {
			return ParagraphBuilder.StringToSentenceCase(value);
		}

		/// <summary>
		/// Every word starts with capital.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToTitleCase(this string value) {
			string[] parts = value.Split(new string[] { " " }, StringSplitOptions.None);
			StringBuilder builder = new StringBuilder();
			foreach (string part in parts) {
				builder.Append(part.ToSentenceCase());
				builder.Append(" ");
			}
			return builder.ToString();
		}

		public static string RemoveCamelCase(this string value) {
			StringBuilder retval = new StringBuilder();
			char lastChar = value[0];
			retval.Append(Char.ToUpper(lastChar));
			for (int i = 1; i < value.Length; i++) {
				char currChar = value[i];
				if (Char.IsLower(lastChar) && Char.IsUpper(currChar)) {
					retval.Append(" ");
				}
				retval.Append(currChar);
				lastChar = currChar;
			}
			return retval.ToString();
		}

		/// <summary>
		/// Parses a camel cased or pascal cased string and returns a new 
		/// string with spaces between the words in the string.
		/// </summary>
		/// <example>
		/// The string "PascalCasing" will return an array with two 
		/// elements, "Pascal" and "Casing".
		/// </example>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string SplitUpperCaseToString(this string source) {
			return String.Join(" ", SplitUpperCase(source));
		}

		/// <summary>
		/// Parses a camel cased or pascal cased string and returns an array 
		/// of the words within the string.
		/// </summary>
		/// <example>
		/// The string "PascalCasing" will return an array with two 
		/// elements, "Pascal" and "Casing".
		/// </example>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string[] SplitUpperCase(this string source) {
			if (source == null)
				return new string[] { }; //Return empty array.

			if (source.Length == 0)
				return new string[] { "" };

			var words = new List<string>();
			int wordStartIndex = 0;

			char[] letters = source.ToCharArray();

			// Skip the first letter. we don't care what case it is.
			for (int i = 1; i < letters.Length; i++) {
				if (Char.IsUpper(letters[i])) {
					if (i + 1 < letters.Length && !Char.IsUpper(letters[i + 1])) {
						//Grab everything before the current index.
						words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
						wordStartIndex = i;
					}
				}
			}

			//We need to have the last word.
			words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

			//Copy to a string array.
			string[] wordArray = new string[words.Count];
			words.CopyTo(wordArray, 0);
			return wordArray;
		}

		public static string GetLeafDirectory(this string path) {
			return path.Split(Path.DirectorySeparatorChar).Last();
		}

		public static bool IsUNCPath(this string path) {
			Uri uri = new Uri(path);
			return uri.IsUnc;
		}

		public static string GetUNCHost(this string path) {
			Debug.Assert(IsUNCPath(path));
			var uri = new Uri(path);
			return Uri.UnescapeDataString(uri.Host);
		}


		public static string ToCamelCase(this string s) {
			return s.ToCamelCase(true, false);
		}

		public static string ToCamelCase(this string s, bool startWithLetter, bool hyphenate) {
			return s.ToCamelCase(true, false, '-');
		}

		public static string ToCamelCase(this string @string, bool startWithLetter, bool hyphenate, char hyphen) {
			StringBuilder sb = new StringBuilder();

			bool wantStart = startWithLetter;
			bool makeUpper = true;
			bool addHyphen = false;
			bool wantFirst = true;

			for (int i = 0; i < @string.Length; i++) {
				char c = @string[i];

				// If we want a starting character at this point, ignore non-letters
				if (wantStart && !Char.IsLetter(c))
					continue;
				wantStart = false;

				// Pretend apostrophies are not in the name
				if (c == '\'')
					continue;

				if (!Char.IsLetterOrDigit(c)) {
					makeUpper = true;
					if (hyphenate)
						addHyphen = true;
					continue;
				}

				// Until something is added, do not add a hyphen
				// so that we avoid a hyphen being the first character
				if (!wantFirst && addHyphen)
					sb.Append(hyphen);
				addHyphen = false;
				wantFirst = false;

				if (makeUpper)
					sb.Append(Char.ToUpper(c));
				else
					sb.Append(Char.ToLower(c));
				makeUpper = false;
			}
			return sb.ToString();
		}

		public static string CamelCaseExpand(this string @string) {
			return
				Regex.Replace(
					Regex.Replace(
						Regex.Replace(
							@string,
							@"([\p{Ll}])([\p{Lu}])",
							"$1 $2"),
						@"([\p{L}])(-)([\p{Lu}])",
						"$1 $2 $3"),
					@"([\p{L}])_([\p{Lu}])",
					"$1 $2"
					);
		}

		#endregion

		#region Parse Many

		public static T1 ParseMany<T1>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return ParseToken<T1>(tokens, 0);
		}

		public static Tuple<T1, T2> ParseMany<T1, T2>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return Tuple.Create(ParseToken<T1>(tokens, 0), ParseToken<T2>(tokens, 1));
		}

		public static Tuple<T1, T2, T3> ParseMany<T1, T2, T3>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return Tuple.Create(ParseToken<T1>(tokens, 0), ParseToken<T2>(tokens, 1), ParseToken<T3>(tokens, 2));
		}

		public static Tuple<T1, T2, T3, T4> ParseMany<T1, T2, T3, T4>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return Tuple.Create(ParseToken<T1>(tokens, 0), ParseToken<T2>(tokens, 1), ParseToken<T3>(tokens, 2), ParseToken<T4>(tokens, 3));
		}

		public static Tuple<T1, T2, T3, T4, T5> ParseMany<T1, T2, T3, T4, T5>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return Tuple.Create(ParseToken<T1>(tokens, 0), ParseToken<T2>(tokens, 1), ParseToken<T3>(tokens, 2), ParseToken<T4>(tokens, 3), ParseToken<T5>(tokens, 4));
		}

		public static Tuple<T1, T2, T3, T4, T5, T6> ParseMany<T1, T2, T3, T4, T5, T6>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return Tuple.Create(ParseToken<T1>(tokens, 0), ParseToken<T2>(tokens, 1), ParseToken<T3>(tokens, 2), ParseToken<T4>(tokens, 3), ParseToken<T5>(tokens, 4), ParseToken<T6>(tokens, 5));
		}

		public static Tuple<T1, T2, T3, T4, T5, T6, T7> ParseMany<T1, T2, T3, T4, T5, T6, T7>(this string stringValue, params string[] delimitters) {
			var tokens = Tokenize(stringValue, delimitters);
			return Tuple.Create(ParseToken<T1>(tokens, 0), ParseToken<T2>(tokens, 1), ParseToken<T3>(tokens, 2), ParseToken<T4>(tokens, 3), ParseToken<T5>(tokens, 4), ParseToken<T6>(tokens, 5), ParseToken<T7>(tokens, 6));
		}

		private static T1 ParseToken<T1>(string[] tokens, int index) {
			if (index >= tokens.Length)
				throw new SoftwareException($"No {typeof(T1).Name} found in token {index}");
			return Parse<T1>(tokens[index]);
		}

		private static string[] Tokenize(string stringValue, params string[] delimitters) {
			return stringValue.Split(delimitters, StringSplitOptions.None);
		}

		private static T Parse<T>(string token) {
			T val;
			if (typeof(T) == typeof(string)) {
				val = (T)(object)token;
			} else {
				val = Tools.Parser.Parse<T>(token);
			}
			return val;
		}

		#endregion

	}
}