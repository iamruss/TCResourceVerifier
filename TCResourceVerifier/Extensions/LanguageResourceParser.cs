#region Copyright notice

//<copyright file="LanguageResourceParser.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T08:49">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TCResourceVerifier.Extensions
{
	public static class LanguageTokenParserExtension
	{
		private static readonly Regex TokenRegex
			= new Regex(@"\$core_v2_language\.GetResource\(('|""){1}(?<langToken>([^'\$""]*)+)('|""){1}\)",
							RegexOptions.Compiled
							| RegexOptions.IgnoreCase
							| RegexOptions.Multiline
							| RegexOptions.IgnorePatternWhitespace);

		public static IEnumerable<string> ParseLanguageToken(this string content)
		{
			MatchCollection matches = TokenRegex.Matches(content);
			List<string> tokens = matches.Cast<Match>()
				.Where(m => m.Success)
				.Select(m => m.Groups["langToken"].ToString())
				.ToList();
			return tokens.Distinct();
		}
	}
}