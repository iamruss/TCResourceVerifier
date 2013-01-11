#region Copyright notice

//<copyright file="IWidget.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T01:23">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System;
using System.Collections.Generic;

namespace TCResourceVerifier.Interfaces
{
    public interface IWidget : IWidgetFile
	{
		string Name { get; set; }
		string Description { get; set; }
		string ContentScript { get; set; }
		Guid InstanceIdentifier { get; set; }

		List<IWidgetDependencyFile> DependencyFiles { get; set; }
		Dictionary<string, Dictionary<string, string>> Languages { get; set; }
	}
}