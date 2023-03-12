
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

	    private int currentBlockSelection = -1;

        public QuizOverview()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
	        List<QuizBlock> blocks = FileManager.CurrentFile.allBlocks;
	        QuizBlockList.Items.Clear();

	        foreach (QuizBlock block in blocks)
	        {
                ListViewItem item = new ListViewItem
                {
	                Content = block.GetBlockRepresentation()
                };
                item.Selected += QuizBlockSelected;

		        QuizBlockList.Items.Add(item);
	        }
        }

		private void QuizBlockSelected(object sender, RoutedEventArgs e)
		{
			currentBlockSelection = QuizBlockList.Items.IndexOf(sender);
		}

		private void AddNewQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            QuizBlock newQ = FileManager.CurrentFile.AddBlock();
            MainWindow.ShowPage(new QuestionEditor(newQ), MainWindow.MainContentFrame);
        }

        private void EditQuestionButtonClick(object sender, RoutedEventArgs e)
        {
	        QuizBlock toEdit = FileManager.CurrentFile.GetBlock(currentBlockSelection);
	        MainWindow.ShowPage(new QuestionEditor(toEdit), MainWindow.MainContentFrame);
        }
    }
}