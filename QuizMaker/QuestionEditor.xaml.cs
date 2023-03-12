using System.Windows;
using System.Windows.Controls;
using QuizMaker.Commands;

namespace QuizMaker
{
	/// <summary>
	/// Interaction logic for QuestionEditor.xaml
	/// </summary>
	public partial class QuestionEditor : Page
	{

		private static QuizBlock currentlyEditing;
		private static QuizElement selectedQuizElement;
		
		public QuestionEditor(QuizBlock currentlyEditing)
		{
			QuestionEditor.currentlyEditing = currentlyEditing;
			QuestionEditor.currentlyEditing.OnDataChanged += DrawComponentUI;
			
			InitializeComponent();
			DataContext = this;
			DrawElementListUI();
		}

		~QuestionEditor()
		{
			currentlyEditing.OnDataChanged -= DrawComponentUI;
			CommandHandler.ClearCommandStack();
		}

		private void DrawComponentUI()
		{
			ComponentStackPanel.Children.Clear();
			selectedQuizElement.DrawAllComponents(ComponentStackPanel);
		}

		private void DrawElementListUI()
		{
			string[] keys = currentlyEditing.GetAllQuizElementKeys();
			foreach (string key in keys)
			{
				ListViewItem toAdd = new ListViewItem
				{
					Content = key
				};
				toAdd.Selected += QuizElementSelected;
				QuizElementList.Items.Add(toAdd);
			}
		}

		private void DrawAddComponentMenu()
		{
			ComponentMenu.Items.Clear();
			ComponentMenu.Items.Add(UIBuilder.BuildComponentMenu(selectedQuizElement));
		}

		private void ApplyAndExit(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview(), MainWindow.MainContentFrame);
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview(), MainWindow.MainContentFrame);
		}

		private void QuizElementSelected(object sender, RoutedEventArgs e)
		{
			string selectedKey = ((ListViewItem)sender).Content.ToString();
			selectedQuizElement = currentlyEditing.quizElements[selectedKey];
			DrawComponentUI();
			DrawAddComponentMenu();
		}
	}
}
