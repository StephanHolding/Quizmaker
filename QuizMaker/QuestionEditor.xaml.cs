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
	/// Interaction logic for QuestionEditor.xaml
	/// </summary>
	public partial class QuestionEditor : Page
	{

		private List<string> incorrectQuestions = new List<string>();

		public QuestionEditor()
		{
			InitializeComponent();
		}

		private void AddIncorrectQuestion(object sender, RoutedEventArgs e)
		{
			if (addAnswerInputfield.Text != string.Empty)
				incorrectQuestionList.Items.Add(addAnswerInputfield.Text);
		}

		private void ApplyAndExit(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview());
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview());
		}
	}
}
