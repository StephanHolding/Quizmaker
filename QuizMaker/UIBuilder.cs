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

		public static ColumnDefinition autoColumnDefinition = new ColumnDefinition()
		{
			Width = new GridLength(1, GridUnitType.Auto)
		};

		public static ColumnDefinition starColumnDefinition = new ColumnDefinition()
		{
			Width = new GridLength(1, GridUnitType.Star)
		};

		public static RowDefinition autoRowDefinition = new RowDefinition()
		{
			Height = new GridLength(1, GridUnitType.Auto)
		};

		public static RowDefinition starRowDefinition = new RowDefinition()
		{
			Height = new GridLength(1, GridUnitType.Star)
		};

		public static ColumnDefinition CreateStarColumnDefinition(int value)
		{
			return new ColumnDefinition()
			{
				Width = new GridLength(value, GridUnitType.Star)
			};
		}

	}
}
