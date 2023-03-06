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
	/// Interaction logic for QuizOverview.xaml
	/// </summary>
	public partial class QuizOverview : Page, IUIUpdateable
	{
		public QuizOverview()
		{
			InitializeComponent();
		}

		public void UpdateUI()
		{
			throw new NotImplementedException();
		}

		private void AddNewQuestionButtonClick(object sender, RoutedEventArgs e)
		{
			//add to listbox

			MainWindow.ShowPage(new QuestionEditor());
		}

		private void EditQuestionButtonClick(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuestionEditor());
		}
	}
}
