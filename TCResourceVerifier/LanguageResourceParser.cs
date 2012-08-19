﻿#region Copyright notice

//<copyright file="LanguageResourceParser.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T08:49">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TCResourceVerifier
{
	public class LanguageTokenParser
	{
		private static readonly Regex TokenRegex
			= new Regex(@"_language\.GetResource\(('|""){1}(?<langToken>([^'\$""]*)+)('|""){1}\)",
							RegexOptions.Compiled
							| RegexOptions.IgnoreCase
							| RegexOptions.Multiline
							| RegexOptions.IgnorePatternWhitespace);

		public static IEnumerable<string> Parse(string content)
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