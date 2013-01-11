#region Copyright notice

//<copyright file="Class1.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T01:32">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System;
using System.Collections.Generic;
using TCResourceVerifier.Interfaces;

namespace TCResourceVerifier.Entities
{
    public class Widget : IWidget
	{
		public Widget()
		{
			Languages = new Dictionary<string, Dictionary<string, string>>();
			DependencyFiles = new List<IWidgetDependencyFile>();
		}

		#region Implementation of IWidgetFile

		public string FileName { get; set; }
		public Guid InstanceIdentifier { get; set; }

		#endregion

		#region Implementation of IWidget

		public string Name { get; set; }
		public string Description { get; set; }
		public string ContentScript { get; set; }

		public List<IWidgetDependencyFile> DependencyFiles { get; set; }
		public Dictionary<string, Dictionary<string, string>> Languages { get; set; }

		public WidgetFileType WidgetFileType { get; set; }

		#endregion

        private sealed class WidgetEqualityComparer : IEqualityComparer<Widget>
        {
            public bool Equals(Widget x, Widget y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }
                if (ReferenceEquals(x, null))
                {
                    return false;
                }
                if (ReferenceEquals(y, null))
                {
                    return false;
                }
                if (x.GetType() != y.GetType())
                {
                    return false;
                }
                return x.InstanceIdentifier.Equals(y.InstanceIdentifier) && x.WidgetFileType.Equals(y.WidgetFileType);
            }

            public int GetHashCode(Widget obj)
            {
                unchecked
                {
                    return (obj.InstanceIdentifier.GetHashCode() * 397) ^ obj.WidgetFileType.GetHashCode();
                }
            }
        }

        private static readonly IEqualityComparer<Widget> WidgetComparerInstance = new WidgetEqualityComparer();

        public static IEqualityComparer<Widget> WidgetComparer
        {
            get { return WidgetComparerInstance; }
        }
	}
}