#region Copyright notice

//<copyright file="FindMissingLanguageResourceKeys.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T08:35">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using TCResourceVerifier.Entities;
using TCResourceVerifier.Extensions;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Strategies
{
    public class FindMissingLanguageResourceKeys : IVerificationStrategy<Dictionary<string, MissingResourceInfo>>
	{
        public Dictionary<IWidgetFile, Dictionary<string, MissingResourceInfo>> Use(string rootFolderName)
		{
			var problems = new Dictionary<IWidgetFile, Dictionary<string, MissingResourceInfo>>();
            var widgetReader = new WidgetReader(rootFolderName);
			List<IWidget> widgets = widgetReader.LoadWidgets();

			foreach (IWidget widget in widgets)
			{
                //find tokens used in this widget 
			    IEnumerable<string> tokensInUse = widget.ContentScript.ParseLanguageToken();
                //
				VerifyLanguageTokens(widget, tokensInUse, widget, problems);
				foreach (IWidgetDependencyFile dependencyFile in widget.DependencyFiles.Where((df) => df.WidgetFileType == WidgetFileType.VelocityFile))
				{
					string velocityFileContent = File.ReadAllText(dependencyFile.FullPath);
					IEnumerable<string> tokensInUseByDependencyFile = velocityFileContent.ParseLanguageToken();
					VerifyLanguageTokens(dependencyFile, tokensInUseByDependencyFile, widget, problems);
				}
			}
			return problems;
		}

		private static void VerifyLanguageTokens(IWidgetFile widget, IEnumerable<string> tokensInUse, IWidget rootWidgetFile, Dictionary<IWidgetFile, Dictionary<string, MissingResourceInfo>> problems)
		{
			foreach (string token in tokensInUse)
			{
				foreach (string languageName in rootWidgetFile.Languages.Keys)
				{
					if (rootWidgetFile.Languages[languageName].Keys.Contains(token) == false)
					{
						if (!problems.ContainsKey(rootWidgetFile))
						{
							problems.Add(rootWidgetFile, new Dictionary<string, MissingResourceInfo>());
						}
						problems[rootWidgetFile][languageName] = new MissingResourceInfo
							{
								FileName = widget.FileName,
								LanguageName = languageName,
								ResourceName = token
							};
					}
				}
			}
		}

	}
}