#region Copyright notice

//<copyright file="WidgetReader.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T01:23">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ninject;
using TCResourceVerifier.Entities;
using TCResourceVerifier.Interfaces;
using TCResourceVerifier.Services;

namespace TCResourceVerifier
{
    public interface IWidgetReader
    {
        List<IWidget> LoadWidgets(string rootFolderName);
    }

    public class WidgetReader : IWidgetReader
    {
		private const string ImageFileExtensions = ".jpg.png.gif";
		private readonly IFileSystemService _fileSystemService;

		public WidgetReader()
			: this(new FileSystemServiceImpl())
		{
		}

        [Inject]
		public WidgetReader(IFileSystemService fileSystemService)
		{
			if (fileSystemService == null)
			{
				throw new ArgumentNullException("fileSystemService");
			}

			_fileSystemService = fileSystemService;
		}

		public List<IWidget> LoadWidgets(string rootFolderName)
		{
            if (!_fileSystemService.DirectoryExists(rootFolderName))
            {
                throw new FileNotFoundException(string.Format(@"Cannot find folder ""{0}""", rootFolderName));
            }
			var resultList = new List<IWidget>();
            var files = new List<string>(_fileSystemService.EnumerateFileSystemEntries(rootFolderName,"*.xml",SearchOption.TopDirectoryOnly));

			foreach (string file in files)
			{
				IEnumerable<IWidget> widgets = LoadWidgetsFromFile(file);
				resultList.AddRange(widgets);
			}
			return resultList;
		}

		private IEnumerable<IWidget> LoadWidgetsFromFile(string path)
		{
			var widgetsInFile = new List<IWidget>();
			var fileInfo = new FileInfo(path);
			string xmlFile = File.ReadAllText(path);
			XDocument xDoc = XDocument.Parse(xmlFile);
			IEnumerable<XElement> xElements = xDoc.Elements("scriptedContentFragments");
			foreach (XElement xElement in xElements)
			{
				Widget widget = null;

				try
				{
					XElement widgetElement = xElement.Element("scriptedContentFragment");
					if (widgetElement != null)
					{
						widget = new Widget {WidgetFileType = WidgetFileType.WidgetDefinition, FileName = fileInfo.Name};

						//load widget contentScript section's content
						XElement elementContentScript = widgetElement.Element("contentScript");
						if (elementContentScript != null)
						{
							widget.ContentScript = elementContentScript.Value;
						}
					    XElement headerScript = widgetElement.Element("headerScript");
                        if (headerScript != null)
                        {
                            widget.HeaderScript = headerScript.Value;
                        }
					    XElement configSection = widgetElement.Element("configuration");
					    if (configSection != null)
					    {
					        widget.ConfigSection = configSection.Value;
					    }
						LoadInfoFromHeader(widgetElement, widget);
						LoadLanguages(widget, widgetElement);
						LoadDependenices(widget, fileInfo);
					    ResolveWidgetHeader(widget);
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e);
					widget = null;
				}

				//if loading fails, we need to set widget var to NULL
				if (widget != null)
				{
					widgetsInFile.Add(widget);
				}
			}
			return widgetsInFile;
		}

        private const string HeaderResourcePrefix = "${resource:";
	    private void ResolveWidgetHeader(Widget widget)
	    {
	        widget.NameDescXml = string.Format("\"{0}\" \"{1}\"", widget.Name, widget.Description);
            if (widget.Name.StartsWith(HeaderResourcePrefix))
            {
                widget.Name = ResolveResource(widget, widget.Name);
            }
            if (widget.Description.StartsWith(HeaderResourcePrefix))
            {
                widget.Description = ResolveResource(widget, widget.Description);  
            }
	    }

	    private string ResolveResource(Widget widget, string stringToResolve)
	    {
            if (widget.Languages.Count != 0)
            {
                var firstLang = widget.Languages.Keys.FirstOrDefault();
                if (firstLang != null)
                {
                    string token = stringToResolve.TrimEnd(new[] { ' ', '}' }).Replace(HeaderResourcePrefix,"");
                    return widget.Languages[firstLang][token];
                }
            }
            return stringToResolve;
	    }

	    private void LoadDependenices(Widget widget, FileInfo fileInfo)
		{
			//read dependencies and parse VM files
			string dependencyPath = string.Format("{0}\\{1}\\",
			                                      fileInfo.DirectoryName,
			                                      widget.InstanceIdentifier.ToString("N"));

			if (!_fileSystemService.DirectoryExists(dependencyPath))
			{
				return;
			}
			IEnumerable<string> dependencies = _fileSystemService.EnumerateFileSystemEntries(dependencyPath,
			                                                                                 "*",
			                                                                                 SearchOption.TopDirectoryOnly);
			foreach (string dependencyFile in dependencies)
			{
				IWidgetDependencyFile dependency = ProcessDependency(widget, dependencyFile);
				widget.DependencyFiles.Add(dependency);
			}
		}

		private static void LoadInfoFromHeader(XElement widgetElement, Widget widget)
		{
			XAttribute nameAttr = widgetElement.Attribute("name");
			if (nameAttr != null)
			{
				widget.Name = nameAttr.Value;
			}
			XAttribute desctAttr = widgetElement.Attribute("description");
			if (desctAttr != null)
			{
				widget.Description = desctAttr.Value;
			}

			XAttribute guidAttr = widgetElement.Attribute("instanceIdentifier");
			if (guidAttr != null)
			{
				widget.InstanceIdentifier = new Guid(guidAttr.Value);
			}
		}

		private IWidgetDependencyFile ProcessDependency(IWidgetFile parent, string path)
		{
			var fileInfo = new FileInfo(path);
			var dependencyFile = new WidgetDependencyFile
				{
					FullPath = path,
					FileName = fileInfo.Name,
					Parent = parent
				};

			if (fileInfo.Extension == ".xml")
			{
				throw new InvalidOperationException("error: dependency fileInfo.Extension == 'xml'");
			}
			if (fileInfo.Extension == ".vm")
			{
				dependencyFile.WidgetFileType = WidgetFileType.VelocityFile;
			}
			else if (ImageFileExtensions.Contains(fileInfo.Extension))
			{
				dependencyFile.WidgetFileType = WidgetFileType.Image;
			}
			else if (fileInfo.Extension == ".css")
			{
				dependencyFile.WidgetFileType = WidgetFileType.StyleSheet;
			}
			else if (fileInfo.Extension == ".js")
			{
				dependencyFile.WidgetFileType = WidgetFileType.JavaScript;
			}
			else
			{
				dependencyFile.WidgetFileType = WidgetFileType.Other;
			}
			return dependencyFile;
		}

		private static void LoadLanguages(Widget widget, XElement xElement)
		{
			XElement resourcesRoot = xElement.Element("languageResources");
			if (resourcesRoot != null)
			{
				IEnumerable<XElement> languages = resourcesRoot.Descendants("language");
				foreach (XElement language in languages)
				{
					XAttribute langNameAttribute = language.Attribute("key");
					if (langNameAttribute != null)
					{
						string languageName = langNameAttribute.Value;
						IEnumerable<XElement> languageResources = language.Descendants("resource");
						foreach (XElement languageString in languageResources)
						{
							XAttribute resourceNameAttr = languageString.Attribute("name");
							if (resourceNameAttr != null)
							{
								string resourceName = resourceNameAttr.Value;
								if (!widget.Languages.ContainsKey(languageName))
								{
									widget.Languages.Add(languageName, new Dictionary<string, string>());
								}
								//TODO: raise error "Key already defined for language" if key exists
								widget.Languages[languageName][resourceName] = languageString.Value;
							}
						}
					}
				}
			}
			else
			{
				throw new Exception("error: no resources");
			}
		}
	}
}