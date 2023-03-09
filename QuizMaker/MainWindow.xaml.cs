﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private static Frame mainFrame;

		private const string NYI = " functionality not yet implemented.";

		public string TestString { get; set; } = "This is a test";

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;

			mainFrame = Frame;
		}

		public static void ShowPage(Page toShow)
		{
			mainFrame.Content = toShow;
		}

		private void OnNewFileMenuItemClicked(object sender, RoutedEventArgs e)
		{
			FileManager.NewFile();
			ShowPage(new QuizOverview());
		}

		private void OpenFileMenuItemClicked(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = FileManager.EXTENSION_FILTER;

			if (openFileDialog.ShowDialog() == true)
			{
				if (openFileDialog.CheckFileExists)
				{
					if (FileManager.Load(openFileDialog.FileName))
					{
						//Create UI For loaded file.
					}
				}
			}
		}

		private void SaveFileMenuItemClicked(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = FileManager.EXTENSION_FILTER;

			if (saveFileDialog.ShowDialog() == true)
			{
				FileManager.SaveCurrent(saveFileDialog.FileName);
			}
		}

		private void ToggleHintsClicked(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Hints toggle" + NYI);
		}

		private void ExitButtonClicked(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
