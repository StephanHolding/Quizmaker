using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuizMaker
{
	internal static class UIBuilder
	{

		public static ColumnDefinition CreateStarColumnDefinition(int value)
		{
			return new ColumnDefinition()
			{
				Width = new GridLength(value, GridUnitType.Star)
			};
		}

		public static ColumnDefinition CreateAutoColumnDefinition(int value)
		{
			return new ColumnDefinition()
			{
				Width = new GridLength(value, GridUnitType.Auto)
			};
		}

		public static void AddToGrid(Grid grid, UIElement element, int column = 0, int row = 0)
		{
			grid.Children.Add(element);

			if (column != 0)
				Grid.SetColumn(element, column);

			if (row != 0)
				Grid.SetRow(element, row);
		}

	}
}
