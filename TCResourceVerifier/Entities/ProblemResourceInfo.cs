#region Copyright notice

//<copyright file="ProblemResourceInfo.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T09:53">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Entities
{
	public class ProblemResourceInfo
	{
		/// <summary>
		/// lang key
		/// </summary>
		public string LanguageName { get; set; }

        ///// <summary>
        ///// file where problem is found
        ///// </summary>
        //[Obsolete]
        //public string FileName { get; set; }

		/// <summary>
		/// Missing resource name
		/// </summary>
		public string ResourceName { get; set; }

        /// <summary>
        /// Specific file which references problematic resource
        /// </summary>
        public IWidgetFile WidetFile { get; set; }
	}
}