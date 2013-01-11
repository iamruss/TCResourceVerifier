#region Copyright notice

//<copyright file="FindMissingLanguageResourceKeys.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T08:35">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TCResourceVerifier.Entities;
using TCResourceVerifier.Extensions;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Strategies
{
    public class FindMissingLanguageResourceKeys : IVerificationStrategy
	{
        public ResourceIssue Use(IWidget widget)
		{
            var widgetIssues = new ResourceIssue(widget);

            //find tokens used in this widget 
			IEnumerable<string> tokensInUse = widget.ContentScript.ParseLanguageToken();
                
            //verify root widget file
			VerifyLanguageTokensHaveResourceEntries(widget, tokensInUse, widget, widgetIssues.ProblemResources);

            //verify dependencies
			foreach (IWidgetDependencyFile dependencyFile in widget.DependencyFiles.Where(df => df.WidgetFileType == WidgetFileType.VelocityFile))
			{
				string velocityFileContent = File.ReadAllText(dependencyFile.FullPath);
				IEnumerable<string> tokensInUseByDependencyFile = velocityFileContent.ParseLanguageToken();
                VerifyLanguageTokensHaveResourceEntries(dependencyFile, tokensInUseByDependencyFile, widget, widgetIssues.ProblemResources);
			}

			return widgetIssues;
		}



        private static void VerifyLanguageTokensHaveResourceEntries(IWidgetFile widget, IEnumerable<string> tokensInUse, IWidget rootWidgetFile, List<ProblemResourceInfo> issueList)
		{
			foreach (string token in tokensInUse)
			{
				foreach (string languageName in rootWidgetFile.Languages.Keys)
				{
					if (rootWidgetFile.Languages[languageName].Keys.Contains(token) == false)
					{
                        Debug.WriteLine(token);
                        if (!issueList.Any(pri => pri.LanguageName == languageName && pri.ResourceName == token && pri.ProblemType != ResourceProblemType.MissingResource))
                        {
                            issueList.Add(new ProblemResourceInfo
                                {
                                    LanguageName = languageName,
                                    ResourceName = token,
                                    WidetFile = widget,
                                    ProblemType = ResourceProblemType.MissingResource
                                });
                        }
					}
				}
			}
		}

	}
}