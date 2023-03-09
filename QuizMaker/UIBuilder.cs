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
		public static MenuItem BuildComponentMenu(QuizElement componentOwner)
		{
			MenuItem toReturn = new MenuItem
			{
				Header = "Add Component"
			};

			Type[] allComponents = GetTypesThatInheritFrom<QuizComponent>();
			List<MenuItem> menuItems = new List<MenuItem>();
			
			foreach (Type componentType in allComponents)
			{
				MenuItem toAdd = new MenuItem()
				{
					Header = componentType.Name,
				};
				
				toAdd.Click += delegate { componentOwner.AddComponent(componentType); };
				menuItems.Add(toAdd);
			}

			toReturn.ItemsSource = menuItems;
			return toReturn;
		}

		private static Type[] GetTypesThatInheritFrom<T>()
		{
			Type baseType = typeof(T);
			Assembly assembly = typeof(T).Assembly;
			return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToArray();
		}

	}
}
