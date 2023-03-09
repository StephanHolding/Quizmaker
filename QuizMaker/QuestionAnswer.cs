using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuizMaker
{
    public static class TypeHolder
    {
        public static readonly Type[] types =
        {
            typeof(QuizBlock),
            typeof(QuizElement)
        };
    }

    #region BaseTypes

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

    public class QuizBlock
    {
        public readonly Dictionary<string, QuizElement> quizElements = new Dictionary<string, QuizElement>();

        public delegate void DataEvent();

        public event DataEvent OnDataChanged;
        
        public QuizBlock()
        {
            quizElements.Add("question", new QuizElement(this, "question"));
            quizElements.Add("answer", new QuizElement(this, "answer"));
        }

        public void DrawElement(string elementKey, Panel parent)
        {
            quizElements[elementKey].DrawAllComponents(parent);
        }

        public void DrawElement(string elementKey, ItemsControl parent)
        {
            quizElements[elementKey].DrawAllComponents(parent);
        }

        public void ExecuteOnDataChanged()
        {
            OnDataChanged?.Invoke();
        }
    }

    public class QuizElement
    {
        private readonly List<QuizComponent> components = new List<QuizComponent>();
        private readonly QuizBlock owner;
        private readonly string name;
        
        public QuizElement(QuizBlock owner, string name)
        {
            this.owner = owner;
            this.name = name;
        }

        public void AddComponent<T>() where T : QuizComponent
        {
            AddComponent(typeof(T));
        }

        public void AddComponent(Type type)
        {
            components.Add((QuizComponent)Activator.CreateInstance(type, this));
            MessageBox.Show("adding component for " + name);
            RaiseDataChangedEvent();
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
            if (components.Contains(instance))
            {
                components.Remove(instance);
                
                RaiseDataChangedEvent();
            }
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

        private void RaiseDataChangedEvent()
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
    }

    #endregion

    #region Components

    public abstract class QuizComponent : DynamicUI
    {
        private readonly QuizElement owner;
        private object componentData;
        
        protected QuizComponent(QuizElement owner)
        {
            this.owner = owner;
        }

        protected virtual Control DrawComponentHeader()
        {
            Button removeButton = new Button
            {
                Content = "Remove Component",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(5)
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
        
    }

    public class TextComponent : QuizComponent
    {
        public TextComponent(QuizElement owner) : base(owner)
        {
        }

        public override Control[] Draw()
        {
            List<Control> toReturn = new List<Control>
            {
                DrawComponentHeader(),
            };

            TextBox textBox = new TextBox();
            textBox.TextChanged += delegate { SetData(textBox.Text); };

            if (DataIsPresent())
                textBox.Text = GetData<string>();
            
            toReturn.Add(textBox);
            
            return toReturn.ToArray();
        }
    }

    public class ImageComponent : QuizComponent
    {
        public ImageComponent(QuizElement owner) : base(owner)
        {
        }

        public override Control[] Draw()
        {
            throw new NotImplementedException();
        }
    }

    public class AudioComponent : QuizComponent
    {
        public AudioComponent(QuizElement owner) : base(owner)
        {
        }

        public override Control[] Draw()
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}