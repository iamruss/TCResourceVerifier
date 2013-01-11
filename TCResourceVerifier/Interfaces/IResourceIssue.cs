using System.Collections.Generic;
using TCResourceVerifier.Entities;

namespace TCResourceVerifier.Interfaces
{
    public interface IResourceIssue
    {
        /// <summary>
        /// Widget having problem with resource (the very root xml file)
        /// </summary>
        IWidget Widget { get; }

        /// <summary>
        /// Missing resources
        /// </summary>
        List<ProblemResourceInfo> MissingResources { get; set; }

        /// <summary>
        /// Resources, not used in widget
        /// </summary>
        List<ProblemResourceInfo> UnusedResources { get; set; }
    }
}