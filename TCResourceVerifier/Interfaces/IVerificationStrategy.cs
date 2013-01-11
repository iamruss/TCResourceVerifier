using System.Collections.Generic;
using TCResourceVerifier.Entities;

namespace TCResourceVerifier.Interfaces
{
    public interface IVerificationStrategy
    {
        ResourceIssue Use(IWidget widgets);
    }
}