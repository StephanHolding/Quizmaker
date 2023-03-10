
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace QuizMaker
{
    /// <summary>
    /// Interaction logic for QuizOverview.xaml
    /// </summary>
    public partial class QuizOverview : Page
    {

	    public QuizOverview()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
	        List<QuizBlock> blocks = FileManager.CurrentFile.allBlocks;
	        QuestionContainer.Children.Clear();

	        for (int i = 0; i < blocks.Count; i++)
	        {
		        Button newButton = new Button()
		        {
			        Content = "Question " + i.ToString(),
                    Padding = new Thickness(10),
                    Margin = new Thickness(10)
		        };

		        int index = i;
		        newButton.Click += delegate { QuestionButtonClicked(index); };

		        QuestionContainer.Children.Add(newButton);
	        }
        }

        private void QuestionButtonClicked(int index)
		{
			MainWindow.ShowPage(new QuestionEditor(FileManager.CurrentFile.GetBlock(index)), MainWindow.MainContentFrame);
		}

		private void AddNewQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            QuizBlock newQ = FileManager.CurrentFile.AddBlock();
            MainWindow.ShowPage(new QuestionEditor(newQ), MainWindow.MainContentFrame);
        }
    }
}