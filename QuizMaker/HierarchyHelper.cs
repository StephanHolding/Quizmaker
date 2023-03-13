using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace QuizMaker
{
	internal static class HierarchyHelper
	{
		public static Type[] GetTypesThatInheritFrom<T>()
		{
			Type baseType = typeof(T);
			Assembly assembly = typeof(T).Assembly;
			return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToArray();
		}	

		public static T FindParentOfType<T>(DependencyObject instance) where T : DependencyObject
		{
			while (true)
			{
				DependencyObject parent = VisualTreeHelper.GetParent(instance);

				if (parent != null)
				{
					if (parent is T correctTypeParent)
					{
						return correctTypeParent;
					}
					else
					{
						instance = parent;
					}
				}
				else
				{
					throw new NullReferenceException("Parent could not be found");
				}
			}
		}
	}
}
