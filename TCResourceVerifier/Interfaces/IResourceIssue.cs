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
        /// Problematic resources
        /// </summary>
        List<ProblemResourceInfo> ProblemResources { get; set; }
    }
}