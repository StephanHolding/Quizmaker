
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
			QuestionContainer.Items.Clear();

			for (int i = 0; i < blocks.Count; i++)
			{
				int index = i;

				Grid grid = new Grid()
				{
					ColumnDefinitions =
					{
						UIBuilder.CreateStarColumnDefinition(1),
						UIBuilder.CreateAutoColumnDefinition(1),
						UIBuilder.CreateAutoColumnDefinition(1),
						UIBuilder.CreateAutoColumnDefinition(1)
					}
				};

				Label questionName = new Label()
				{
					Content = blocks[i].questionName
				};
				Button editButton = new Button()
				{
					Content = "Edit",
					Padding = new Thickness(40, 1, 40, 1),
					Margin = new Thickness(5, 0, 5, 0)
				};
				editButton.Click += delegate { QuestionButtonClicked(index); };

				Button moveUpButton = new Button()
				{
					Content = "\u25b2",
					Padding = new Thickness(5, 2, 5, 2)
				};
				moveUpButton.Click += delegate { MoveQuestionUp(index); };

				Button moveDownButton = new Button()
				{
					Content = "\u25bc",
					Padding = new Thickness(5, 2, 5, 2)
				};
				moveDownButton.Click += delegate { MoveQuestionDown(index); };

				UIBuilder.AddToGrid(grid, questionName, column: 0);
				UIBuilder.AddToGrid(grid, editButton, column: 1);
				UIBuilder.AddToGrid(grid, moveUpButton, column: 2);
				UIBuilder.AddToGrid(grid, moveDownButton, column: 3);

				QuestionContainer.Items.Add(grid);
			}
		}

		private void QuestionButtonClicked(int index)
		{
			MainWindow.ShowPage(new QuestionEditor(FileManager.CurrentFile.GetBlock(index)), MainWindow.MainContentFrame);
		}

		private void AddNewQuestionButtonClick(object sender, RoutedEventArgs e)
		{
			QuizBlock newQ = FileManager.CurrentFile.AddBlock();
			UpdateUI();
		}

		private void MoveQuestionUp(int index)
		{
			int newIndex = index - 1;
			FileManager.CurrentFile.ReinsertQuizblock(index, newIndex);
			UpdateUI();
		}

		private void MoveQuestionDown(int index)
		{
			int newIndex = index + 1;
			FileManager.CurrentFile.ReinsertQuizblock(index, newIndex);
			UpdateUI();
		}

		private void RemoveSelected(object sender, RoutedEventArgs e)
		{
			if (QuestionContainer.SelectedIndex != -1)
			{
				FileManager.CurrentFile.RemoveBlock(QuestionContainer.SelectedIndex);
				UpdateUI();
			}
		}
	}
}