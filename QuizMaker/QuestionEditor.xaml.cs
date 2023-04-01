using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuizMaker.Commands;

namespace QuizMaker
{
	public partial class QuestionEditor : Page
	{

		private readonly QuizBlock currentlyEditing;
		private QuizElement selectedQuizElement;

		private const int MAX_CHARACTER_TAG_SELECTOR = 45;

		public QuestionEditor(QuizBlock currentlyEditing)
		{
			this.currentlyEditing = currentlyEditing;

			currentlyEditing.OnComponentChanged += DrawComponentUI;
			currentlyEditing.OnQuizElementListChanged += DrawElementListUI;
			FileManager.CurrentFile.OnTagsChanged += AvailableTagsChanged;

			InitializeComponent();
			DataContext = this;
			DrawElementListUI();
			BuildTagSelectorMenu();
			DisplaySelectedTags();
			UpdateNameUI();
		}

		~QuestionEditor()
		{
			currentlyEditing.OnComponentChanged -= DrawComponentUI;
			currentlyEditing.OnQuizElementListChanged -= DrawElementListUI;
			FileManager.CurrentFile.OnTagsChanged -= AvailableTagsChanged;
			CommandHandler.ClearCommandStack();
		}

		private void DrawComponentUI()
		{
			ComponentStackPanel.Children.Clear();
			selectedQuizElement.DrawAllComponents(ComponentStackPanel);
		}

		private void DrawElementListUI()
		{
			QuizElementList.Items.Clear();
			string[] names = currentlyEditing.GetAllQuizElementNames();

			for (int i = 0; i < names.Length; i++)
			{
				ListViewItem toAdd = new ListViewItem
				{
					Content = names[i]
				};

				int index = i;
				toAdd.Selected += delegate { QuizElementSelected(index); };
				QuizElementList.Items.Add(toAdd);
			}
		}

		private void DisplaySelectedTags(MenuItem[] selectedTags = null)
		{
			string selectedTagsString = string.Empty;

			if (selectedTags == null)
			{
				selectedTags = GetSelectedMenuItems();
			}

			foreach (MenuItem tag in selectedTags)
			{
				if (selectedTagsString.Length == 0)
					selectedTagsString += tag.Header;
				else
					selectedTagsString += ", " + tag.Header;
			}

			if (selectedTagsString.Length > MAX_CHARACTER_TAG_SELECTOR)
			{
				selectedTagsString = selectedTagsString.Substring(0, MAX_CHARACTER_TAG_SELECTOR) + "...";
			}

			if (string.IsNullOrWhiteSpace(selectedTagsString))
				selectedTagsString = "No tags selected";

			TagSelector.Header = selectedTagsString;
		}

		private void ApplyAndExit(object sender, RoutedEventArgs e)
		{
			currentlyEditing.questionName = !string.IsNullOrWhiteSpace(QuestionNameTextBox.Text) ? QuestionNameTextBox.Text : "Untitled Question";
			MainWindow.ShowPage(new QuizOverview(), MainWindow.MainContentFrame);
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			MainWindow.ShowPage(new QuizOverview(), MainWindow.MainContentFrame);
		}

		private void QuizElementSelected(int index)
		{
			MinusButton.IsEnabled = index > 1;

			selectedQuizElement = currentlyEditing.quizElements[index];
			DrawComponentUI();
			BuildComponentMenu(selectedQuizElement);
		}

		private void EvaluateSelectedTags()
		{
			List<MenuItem> selectedTags = new List<MenuItem>();
			ItemCollection allTags = TagSelector.Items;

			for (int i = 0; i < allTags.Count; i++)
			{
				if (allTags[i] is MenuItem menuItem)
				{
					if (menuItem.IsChecked)
					{
						currentlyEditing.ToggleTag(i, true);
						selectedTags.Add(menuItem);
					}
					else
					{
						currentlyEditing.ToggleTag(i, false);
					}
				}
			}

			DisplaySelectedTags(selectedTags.ToArray());
		}

		private MenuItem[] GetSelectedMenuItems()
		{
			List<MenuItem> toReturn = new List<MenuItem>();
			ItemCollection items = TagSelector.Items;
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] is MenuItem menuItem && menuItem.IsChecked)
				{
					toReturn.Add(menuItem);
				}
			}

			return toReturn.ToArray();
		}

		private void BuildTagSelectorMenu()
		{
			Tag[] possibleTags = FileManager.CurrentFile.allAvailableTags;
			List<MenuItem> toReturn = new List<MenuItem>();

			for (int i = 0; i < possibleTags.Length; i++)
			{
				if (possibleTags[i].IsSet())
				{
					MenuItem toAdd = new MenuItem()
					{
						Header = possibleTags[i].tag,
						IsCheckable = true,
						IsChecked = currentlyEditing.IsTagSelected(i)
					};

					toAdd.Click += delegate { EvaluateSelectedTags(); };

					toReturn.Add(toAdd);
				}
			}

			TagSelector.ItemsSource = toReturn;
		}

		public void BuildComponentMenu(QuizElement componentOwner)
		{
			ComponentMenu.Items.Clear();

			MenuItem parent = new MenuItem
			{
				Header = "Add Component",
				Padding = new Thickness(25, 5, 25, 5)
			};

			Type[] allComponents = HierarchyHelper.GetTypesThatInheritFrom<QuizComponent>();
			List<MenuItem> menuItems = new List<MenuItem>();

			foreach (Type componentType in allComponents)
			{
				MenuItem toAdd = new MenuItem()
				{
					Header = componentType.Name,
					IsEnabled = componentType == typeof(TextComponent) || componentType == typeof(ImageComponent)
				};

				toAdd.Click += delegate { componentOwner.AddComponent(componentType); };
				menuItems.Add(toAdd);
			}

			parent.ItemsSource = menuItems;
			ComponentMenu.Items.Add(parent);
		}

		private void AvailableTagsChanged()
		{
			BuildTagSelectorMenu();
			DisplaySelectedTags();
		}

		private void PlusButtonClicked(object sender, RoutedEventArgs e)
		{
			currentlyEditing.AddWrongAnswer();
		}

		private void MinusButtonClicked(object sender, RoutedEventArgs e)
		{
			currentlyEditing.RemoveWrongAnswer(QuizElementList.SelectedIndex);
		}

		private void UpdateNameUI()
		{
			QuestionNameTextBox.Text = currentlyEditing.questionName;
		}
	}
}
