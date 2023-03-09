using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for QuestionEditor.xaml
	/// </summary>
	public partial class QuestionEditor : Page
	{

		public static QuizBlock CurrentlyEditing { get; private set; }
		
		public QuestionEditor(QuizBlock currentlyEditing)
		{
			CurrentlyEditing = currentlyEditing;

			CurrentlyEditing.OnDataChanged += DrawUI;
			
			InitializeComponent();
			DataContext = this;
			DrawUI();
		}

		~QuestionEditor()
		{
			CurrentlyEditing.OnDataChanged -= DrawUI;
		}

		private void DrawUI()
		{
			Q_Panel.Children.Clear();
			CA_Panel.Children.Clear();
			IA_View.Items.Clear();

			CurrentlyEditing.DrawElement("question", Q_Panel);
			CurrentlyEditing.DrawElement("answer", CA_Panel);
		}

		private void AddIncorrectQuestion(object sender, RoutedEventArgs e)
		{

		}

		private void ApplyAndExit(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview());
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview());
		}

		private void incorrectQuestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
