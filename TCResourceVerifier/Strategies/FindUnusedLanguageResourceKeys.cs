using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TCResourceVerifier.Entities;
using TCResourceVerifier.Extensions;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Strategies
{
    public class FindUnusedLanguageResourceKeys : IVerificationStrategy
    {
        public ResourceIssue Use(IWidget widget)
        {
            var widgetIssues = new ResourceIssue(widget);

            //find tokens used in this widget 

            //verify root widget file
            List<string> tokensNotInUse = GetRootWidgetTokens(widget).ToList();
            VerifyLanguageTokensHaveResourceEntries(widget, tokensNotInUse, widget, widgetIssues.ProblemResources);

            //verify dependencies
            var depFiles = widget.DependencyFiles.Where(df => df.WidgetFileType == WidgetFileType.VelocityFile).ToList();
            if (depFiles.Count == 0)
            {
                return widgetIssues;
            }
            foreach (IWidgetDependencyFile dependencyFile in depFiles)
            {
                string velocityFileContent = File.ReadAllText(dependencyFile.FullPath);
                IEnumerable<string> tokensInUseByDependencyFile = velocityFileContent.ParseLanguageToken().ToList();
                if (tokensNotInUse != null)
                {
                    tokensNotInUse.AddRange(tokensInUseByDependencyFile.Except(tokensNotInUse));
                    tokensNotInUse = VerifyLanguageTokensHaveResourceEntries(dependencyFile, tokensNotInUse, widget, widgetIssues.ProblemResources) as List<string>;
                    if (tokensNotInUse != null && tokensNotInUse.Count == 0)
                    {
                        break;
                    }
                }
            }

            if (tokensNotInUse != null)
                foreach (var token in tokensNotInUse)
                {
                    //Debug.WriteLine(token);
                    if (!widgetIssues.ProblemResources.Any(pri =>  pri.ResourceName == token && pri.ProblemType != ResourceProblemType.MissingResource))
                    {
                        widgetIssues.ProblemResources.Add(new ProblemResourceInfo
                                          {
                                              LanguageName = "none",
                                              ResourceName = token,
                                              WidetFile = widget,
                                              ProblemType = ResourceProblemType.UnusedResources
                                          });
                    }
                }
            return widgetIssues;
        }

        private IEnumerable<string> GetRootWidgetTokens(IWidget widget)
        {
            IEnumerable<string> contentTokens = widget.ContentScript.ParseLanguageToken().ToList();
            IEnumerable<string> configTokens = widget.ConfigSection.ParseLanguageToken().ToList();
            IEnumerable<string> headerTokens = widget.HeaderScript.ParseLanguageToken().ToList();
            IEnumerable<string> widgetNameDesc = widget.NameDescXml.ParseLanguageToken().ToList();
            var combined = new List<string>(contentTokens);
            combined.AddRange(configTokens.Except(combined));
            combined.AddRange(headerTokens.Except(combined));
            combined.AddRange(widgetNameDesc.Except(combined));
            return combined.Distinct();
        }


        private static IList<string> VerifyLanguageTokensHaveResourceEntries(IWidgetFile widget, IEnumerable<string> tokensInUse, IWidget rootWidgetFile, List<ProblemResourceInfo> issueList)
        {
            if (rootWidgetFile.Languages.Count == 0)
            {
                return tokensInUse as IList<string>;
            }
            IList<string> inUse = tokensInUse as IList<string> ?? tokensInUse.ToList();
            IEnumerable<string> unusedTokens = rootWidgetFile.Languages["en-us"].Keys.Except(inUse);
            return unusedTokens.ToList();
        }
    }
}