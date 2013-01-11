#region Copyright notice

//<copyright file="MainWindow.xaml.cs" company="ISV Rouslan Grabar" datetime="2012-08-17T22:07">
//  Copyright (c) ISV Rouslan Grabar (c) 2012. All rights reserved.
//</copyright>

#endregion

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Grabrus.TC.ResourceVerifier.Properties;
using TCResourceVerifier;
using TCResourceVerifier.Entities;
using TCResourceVerifier.Interfaces;
using TCResourceVerifier.Strategies;
using WPFFolderBrowser;

namespace Grabrus.TC.ResourceVerifier
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string _path = string.Empty;
		private Dictionary<IWidgetFile, ResourceIssue> _problems;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void OnClickBtnVerify(object sender, RoutedEventArgs e)
		{

            if (string.IsNullOrEmpty(_path))
            {
#if DEBUG
                _path = "C:\\RG\\inetpub\\tc60\\Web\\filestorage\\defaultwidgets\\7bb87a0cc5864a9392ae5b9e5f9747b7";
#else
				OnCLickBtnSelectFolder(null, null);
#endif
            }
			_problems = null;
			problemsGrid.ItemsSource = null;
			detailsGrid.ItemsSource = null;

			SetButtonDisabledState(true);
			
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += WorkerTranactionDoWork;
			worker.RunWorkerCompleted += WorkerTranactionOnRunWorkerCompleted;
			worker.RunWorkerAsync();
		}

		private void SetButtonDisabledState(bool disabled)
		{
			btnVerify.IsEnabled = !disabled;
			btnSelectFolder.IsEnabled = !disabled;
		}

		private void WorkerTranactionOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if(_problems != null)
			{
				problemsGrid.ItemsSource = _problems.Keys;
			}
			SetButtonDisabledState(false);
		}

		private void WorkerTranactionDoWork(object sender, DoWorkEventArgs e)
		{
			var strategy = new FindMissingLanguageResourceKeys();
            var widgetReader = new WidgetReader(_path);
            _problems = strategy.Use(widgetReader.LoadWidgets());
		}

		private void OnSelectedProblemSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (problemsGrid.SelectedItem != null && _problems.ContainsKey((IWidgetFile) problemsGrid.SelectedItem))
			{
				ResourceIssue selected = _problems[((IWidgetFile) problemsGrid.SelectedItem)];
				detailsGrid.ItemsSource = selected.MissingResources;
			}
		}

		private void OnCLickBtnSelectFolder(object sender, RoutedEventArgs e)
		{
			var ofd = new WPFFolderBrowserDialog("Select ROOT folder below defaultwidgets folder (i.e. your Default Widget Factory Plugin Guid)")
				{
					FileName = Settings.Default.Path
				};
			bool? res = ofd.ShowDialog(this);
			if (res.HasValue && res.Value)
			{
				_path = ofd.FileName;
			}
		}
	}
}