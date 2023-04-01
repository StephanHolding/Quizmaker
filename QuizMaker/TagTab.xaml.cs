using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			TagListView.Items.Clear();
			Tag[] tags = FileManager.CurrentFile.allAvailableTags;
			ObservableCollection<ListViewItem> items = new ObservableCollection<ListViewItem>();

			for (int i = 0; i < tags.Length; i++)
			{
				ListViewItem item = DrawTagItem(tags[i], i);
				items.Add(item);
			}

			TagListView.ItemsSource = items;
		}

		private ListViewItem DrawTagItem(Tag tag, int index)
		{
			ListViewItem toReturn = new ListViewItem
			{
				Margin = new Thickness(2),
			};

			Grid itemContent = new Grid()
			{
				Margin = new Thickness(2),
				ColumnDefinitions = { UIBuilder.CreateStarColumnDefinition(1), UIBuilder.CreateStarColumnDefinition(3) }
			};

			TextBlock indexLabel = new TextBlock()
			{
				Text = (index + 1).ToString() + ": ",
				VerticalAlignment = VerticalAlignment.Center,
			};

			TextBox textBox = new TextBox
			{
				Padding = new Thickness(3),
				Text = tag.tag
			};

			textBox.GotFocus += delegate { SelectListViewItem(toReturn); };
			textBox.LostFocus += UpdateFile;

			UIBuilder.AddToGrid(itemContent, indexLabel);
			UIBuilder.AddToGrid(itemContent, textBox, 1);

			toReturn.Content = itemContent;

			return toReturn;
		}

		private void SelectListViewItem(ListViewItem item)
		{
			TagListView.SelectedItems.Clear();
			item.IsSelected = true;
		}

		private void UpdateFile(object sender, RoutedEventArgs routedEventArgs)
		{
			TextBox changed = (TextBox)sender;
			FileManager.CurrentFile.ChangeAvailableTagValue(TagListView.SelectedIndex, changed.Text);
		}

		/*private void OnAddNewTag(object sender, RoutedEventArgs e)
		{
			FileManager.CurrentFile.AddAvailableTag(string.Empty);
			UpdateUI();
		}

		private void OnRemoveSelectedTag(object sender, RoutedEventArgs e)
		{
			FileManager.CurrentFile.RemoveAvailableTag(TagListView.SelectedIndex);
			UpdateUI();
		}*/
	}
}
