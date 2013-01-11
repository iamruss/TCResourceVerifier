using System.Collections.Generic;
using System.Linq;
using Ninject;
using TCResourceVerifier.Entities;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier
{
    public class Verifier
    {
        private readonly IKernel _kernel;

        public Verifier()
        {
            _kernel = new StandardKernel(new SimpleModule());
        }

        public Dictionary<IWidgetFile, ResourceIssue> Process(string rootFolder, IVerificationStrategy[] strategies)
        {
            List<IWidget> widgets = _kernel.Get<IWidgetReader>().LoadWidgets(rootFolder);

            Dictionary<IWidgetFile, ResourceIssue> result = new Dictionary<IWidgetFile, ResourceIssue>();

            foreach (var widget in widgets)
            {
                foreach (var verificationStrategy in strategies)
                {
                    ResourceIssue resourceIssues = verificationStrategy.Use(widget);
                    if (resourceIssues.HasIssues())
                    {
                        if (!result.ContainsKey(widget))
                        {
                            result.Add(widget, resourceIssues);
                        }
                        else
                        {
                            var uniq = resourceIssues.ProblemResources.Except(result[widget].ProblemResources);
                            result[widget].ProblemResources.AddRange(uniq);
                        }
                    }
                }
            }
            return result;
        }
    }
}