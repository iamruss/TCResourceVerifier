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
            MissingResources = new List<ProblemResourceInfo>();
            UnusedResources = new List<ProblemResourceInfo>();
        }

        /// <summary>
        /// Widget having problem with resource (the very root xml file)
        /// </summary>
        public IWidget Widget { get; protected set; }

        /// <summary>
        /// Missing resources
        /// </summary>
        public List<ProblemResourceInfo> MissingResources { get; set; }

        /// <summary>
        /// Resources, not used in widget
        /// </summary>
        public List<ProblemResourceInfo> UnusedResources { get; set; }

        /// <summary>
        /// Flag if issue is in fact an issue
        /// </summary>
        /// <returns></returns>
        public bool HasIssues()
        {
            return MissingResources.Count > 0 || UnusedResources.Count > 0;
        }
    }
}