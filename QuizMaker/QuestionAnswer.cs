using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using QuizMaker.Commands;
using System.Runtime.Serialization;
using System.Windows.Navigation;

namespace QuizMaker
{
	public static class TypeHolder
	{
		public static readonly Type[] types =
		{
			typeof(QuizBlock),
			typeof(QuizElement),
			typeof(Tag),
			typeof(DynamicUI),
			typeof(QuizComponent),
			typeof(TextComponent),
			typeof(ImageComponent),
			typeof(AudioComponent)
		};
	}

	#region BaseTypes

	[DataContract]
	public abstract class DynamicUI
	{
		public abstract Control[] Draw();

		public void AddToUI(Panel addTo)
		{
			Control[] controls = Draw();
			foreach (Control c in controls)
			{
				addTo.Children.Add(c);
			}
		}

		public void AddToUI(ItemsControl addTo)
		{
			Control[] controls = Draw();
			foreach (Control c in controls)
			{
				addTo.Items.Add(c);
			}
		}
	}

	#endregion

	#region Block and Elements

	[DataContract]
	public class QuizBlock
	{
		[DataMember]
		public List<QuizElement> quizElements = new List<QuizElement>();
		[DataMember]
		private int selectedTagsBitmask = 0;


		public delegate void DataEvent();
		public event DataEvent OnComponentChanged;
		public event DataEvent OnQuizElementListChanged;


		public QuizBlock()
		{
			quizElements.Add(new QuizElement("Question"));
			quizElements.Add(new QuizElement("Correct Answer"));

			InjectReferences();
		}

		public void AddWrongAnswer()
		{
			QuizElement toAdd = new QuizElement("Incorrect Answer " + (quizElements.Count - 1).ToString());
			toAdd.InjectReference(this);
			quizElements.Add(toAdd);
			OnQuizElementListChanged?.Invoke();
		}

		public void RemoveWrongAnswer(int index)
		{
			Debug.Assert(index > 1);
			quizElements.RemoveAt(index);
			OnQuizElementListChanged?.Invoke();
		}

		public void ToggleTag(int index, bool toggle)
		{
			if (toggle)
				selectedTagsBitmask |= (1 << index);
			else
				selectedTagsBitmask &= ~(1 << index);
		}

		public bool IsTagSelected(int index)
		{
			return (selectedTagsBitmask & (1 << index)) != 0;
		}

		public void DrawElement(int elementIndex, Panel parent)
		{
			quizElements[elementIndex].DrawAllComponents(parent);
		}

		public void DrawElement(int elementIndex, ItemsControl parent)
		{
			quizElements[elementIndex].DrawAllComponents(parent);
		}

		public void ExecuteOnDataChanged()
		{
			OnComponentChanged?.Invoke();
		}

		public string[] GetAllQuizElementNames()
		{
			List<string> names = new List<string>();
			for (int i = 0; i < quizElements.Count; i++)
			{
				names.Add(quizElements[i].name);
			}

			return names.ToArray();
		}

		public void InjectReferences()
		{
			foreach (QuizElement quizElement in quizElements)
			{
				quizElement.InjectReference(this);
			}
		}

	}

	[DataContract]
	public class QuizElement : IReferenceInjection
	{
		[DataMember]
		public readonly List<QuizComponent> components = new List<QuizComponent>();
		[DataMember]
		public readonly string name;

		private QuizBlock owner;

		public QuizElement(string name)
		{
			this.name = name;
		}

		public void AddComponent<T>() where T : QuizComponent
		{
			AddComponent(typeof(T));
		}

		public void AddComponent(Type type)
		{
			CommandHandler.ExecuteCommand(new AddComponentCommand(this, type));
		}

		public QuizComponent GetComponent<T>() where T : QuizComponent
		{
			return FindComponents<T>(true)[0];
		}

		public QuizComponent[] GetComponents<T>() where T : QuizComponent
		{
			return FindComponents<T>(false);
		}

		public void RemoveComponent<T>() where T : QuizComponent
		{
			QuizComponent toRemove = FindComponents<T>(true)[0];
			if (toRemove != null)
			{
				RemoveComponent(toRemove);
			}
		}

		public void RemoveComponent(QuizComponent instance)
		{
			CommandHandler.ExecuteCommand(new RemoveComponentCommand(instance, this));
		}

		public void DrawAllComponents(Panel addTo)
		{
			foreach (QuizComponent quizComponent in components)
			{
				quizComponent.AddToUI(addTo);
			}
		}

		public void DrawAllComponents(ItemsControl addTo)
		{
			foreach (QuizComponent quizComponent in components)
			{
				quizComponent.AddToUI(addTo);
			}
		}

		public void RaiseDataChangedEvent()
		{
			owner.ExecuteOnDataChanged();
		}

		private QuizComponent[] FindComponents<T>(bool one) where T : QuizComponent
		{
			List<QuizComponent> toReturn = new List<QuizComponent>();
			foreach (QuizComponent t in components)
			{
				if (t.GetType() == typeof(T))
				{
					toReturn.Add(t);
					if (one)
					{
						return toReturn.ToArray();
					}
				}
			}

			return toReturn.ToArray();
		}

		public void InjectReference(params object[] args)
		{
			owner = args[0] as QuizBlock;

			foreach (var component in components)
			{
				component.InjectReference(this);
			}
		}
	}

	#endregion

	#region Components

	[DataContract]
	public abstract class QuizComponent : DynamicUI, IReferenceInjection
	{
		private QuizElement owner;

		[DataMember]
		private object componentData;

		protected virtual Control DrawComponentHeader()
		{
			Button removeButton = new Button
			{
				Content = "X",
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(5),
				Padding = new Thickness(5, 2, 5, 2)
			};

			removeButton.Click += delegate { owner.RemoveComponent(this); };

			return removeButton;
		}

		protected T GetData<T>()
		{
			return (T)componentData;
		}

		protected void SetData(object newData)
		{
			componentData = newData;
		}

		protected bool DataIsPresent()
		{
			return componentData != null;
		}

		public void InjectReference(params object[] args)
		{
			owner = args[0] as QuizElement;
		}
	}

	[DataContract]
	public class TextComponent : QuizComponent
	{

		public override Control[] Draw()
		{
			List<Control> toReturn = new List<Control>
			{
				DrawComponentHeader(),
			};

			TextBox textBox = new TextBox()
			{
				Padding = new Thickness(3),
				Margin = new Thickness(5, 0, 5, 0)
			};
			textBox.TextChanged += delegate { SetData(textBox.Text); };

			if (DataIsPresent())
				textBox.Text = GetData<string>();

			toReturn.Add(textBox);

			return toReturn.ToArray();
		}
	}

	[DataContract]
	public class ImageComponent : QuizComponent
	{

		public override Control[] Draw()
		{
			throw new NotImplementedException();
		}
	}

	[DataContract]
	public class AudioComponent : QuizComponent
	{
		public override Control[] Draw()
		{
			throw new NotImplementedException();
		}
	}

	#endregion
}