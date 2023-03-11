using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuizMaker.Commands;

namespace QuizMaker
{
	public partial class MainWindow : Window
	{
		private static Frame mainFrame;

		private const string NYI = "functionality not yet implemented.";

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
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = FileManager.EXTENSION_FILTER
			};

			if (openFileDialog.ShowDialog() == true)
			{
				if (openFileDialog.CheckFileExists)
				{
					if (FileManager.Load(openFileDialog.FileName))
					{
						ShowPage(new QuizOverview());
					}
				}
			}
		}

		private void SaveFileMenuItemClicked(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = FileManager.EXTENSION_FILTER
			};

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

		private void Undo_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			CommandHandler.Undo();
		}

		private void Undo_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void Redo_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			CommandHandler.Redo();
		}

		private void Redo_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
	}
}
