using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuizMaker
{
	public partial class TagTab : Page
	{

		public TagTab()
		{
			InitializeComponent();
			DataContext = this;
			UpdateUI();
		}

		private void UpdateUI()
		{
			List<Tag> tags = FileManager.CurrentFile.tags;
			List<ListViewItem> items = new List<ListViewItem>();

			for (int i = 0; i < tags.Count; i++)
			{ 
				ListViewItem item = DrawTagItem(tags[i], i);
				items.Add(item);
			}

			TagListView.ItemsSource = items;
		}

		private ListViewItem DrawTagItem(Tag tag, int index)
		{
			Grid itemContent = new Grid()
			{
				ColumnDefinitions = { UIBuilder.CreateStarColumnDefinition(1), UIBuilder.CreateStarColumnDefinition(3) }
			};

			TextBlock indexLabel = new TextBlock()
			{
				Text = index.ToString() + ": ",
				VerticalAlignment = VerticalAlignment.Center,
			};

			TextBox textBox = new TextBox
			{
				Padding = new Thickness(3),
				Text = tag.tag
			};

			textBox.GotFocus += SelectParent;
			textBox.LostFocus += UpdateFile;

			indexLabel.SetValue(Grid.ColumnProperty, 0);
			textBox.SetValue(Grid.ColumnProperty, 1);

			itemContent.Children.Add(indexLabel);
			itemContent.Children.Add(textBox);

			ListViewItem toReturn = new ListViewItem
			{
				Margin = new Thickness(2),

				Content = itemContent
			};

			return toReturn;
		}

		private void SelectParent(object sender, RoutedEventArgs routedEventArgs)
		{
			DependencyObject parent = HierarchyHelper.FindParentOfType<ListViewItem>(sender as TextBox);

			if (parent is ListViewItem listViewItem)
			{
				TagListView.SelectedItems.Clear();
				listViewItem.IsSelected = true;
			}
			else
			{
				MessageBox.Show("its not a listview man");
			}
		}

		private void UpdateFile(object sender, RoutedEventArgs routedEventArgs)
		{
			TextBox changed = (TextBox)sender;
			FileManager.CurrentFile.ChangeTagValue(TagListView.SelectedIndex, changed.Text);
		}

		private void OnAddNewTag(object sender, RoutedEventArgs e)
		{
			FileManager.CurrentFile.AddTag(string.Empty);
			UpdateUI();
		}

		private void OnRemoveSelectedTag(object sender, RoutedEventArgs e)
		{
			FileManager.CurrentFile.RemoveTag(TagListView.SelectedIndex);
			UpdateUI();
		}
	}
}
