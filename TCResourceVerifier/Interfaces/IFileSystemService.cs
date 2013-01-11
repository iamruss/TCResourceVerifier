#region Copyright notice

//<copyright file="IFileSystemService.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T09:00">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.IO;

namespace TCResourceVerifier.Interfaces
{
    public interface IFileSystemService
	{
		IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption option);
		bool DirectoryExists(string path);
	}
}