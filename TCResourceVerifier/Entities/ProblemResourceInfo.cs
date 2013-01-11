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
        protected bool Equals(ProblemResourceInfo other)
        {
            return ProblemType == other.ProblemType && string.Equals(LanguageName, other.LanguageName) && string.Equals(ResourceName, other.ResourceName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProblemResourceInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int) ProblemType;
                hashCode = (hashCode*397) ^ (LanguageName != null ? LanguageName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ResourceName != null ? ResourceName.GetHashCode() : 0);
                return hashCode;
            }
        }

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

        /// <summary>
        /// type of problem
        /// </summary>
        public ResourceProblemType ProblemType { get; set; }
	}
}