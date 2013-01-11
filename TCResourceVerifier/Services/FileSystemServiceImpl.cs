#region Copyright notice

//<copyright file="FileSystemServiceImpl.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T10:14">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.IO;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Services
{
	public class FileSystemServiceImpl : IFileSystemService
	{
		#region Implementation of IFileSystemService

		public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption option)
		{
			return Directory.EnumerateFileSystemEntries(path, searchPattern, option);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		#endregion
	}
}