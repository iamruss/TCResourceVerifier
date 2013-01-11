using System.Collections.Generic;
using TCResourceVerifier.Entities;

namespace TCResourceVerifier.Interfaces
{
    public interface IVerificationStrategy<T>
    {
        Dictionary<IWidgetFile, T> Use(string rootFolderName);
    }
}