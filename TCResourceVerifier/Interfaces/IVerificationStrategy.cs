using System.Collections.Generic;
using TCResourceVerifier.Entities;

namespace TCResourceVerifier.Interfaces
{
    public interface IVerificationStrategy
    {
        Dictionary<IWidgetFile, ResourceIssue> Use(IEnumerable<IWidget> widgets);
    }
}