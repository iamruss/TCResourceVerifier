using System.Collections.Generic;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Entities
{
    /// <summary>
    /// contains widget issues
    /// </summary>
    public class ResourceIssue : IResourceIssue
    {
        public ResourceIssue(IWidget widget)
        {
            Widget = widget;
            ProblemResources = new List<ProblemResourceInfo>();
        }

        /// <summary>
        /// Widget having problem with resource (the very root xml file)
        /// </summary>
        public IWidget Widget { get; protected set; }

        /// <summary>
        /// Problematic resource
        /// </summary>
        public List<ProblemResourceInfo> ProblemResources { get; set; }


        /// <summary>
        /// Flag if issue is in fact an issue
        /// </summary>
        /// <returns></returns>
        public bool HasIssues()
        {
            return ProblemResources.Count > 0;
        }
    }
}