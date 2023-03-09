using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QuizMaker.CustomControls
{
    public partial class ComponentMenu : UserControl
    {
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public static readonly DependencyProperty quizElementKeyDependency = DependencyProperty.Register(nameof(QuizElementKey), typeof(string), typeof(ComponentMenu), new PropertyMetadata(string.Empty, OnPropertyChanged));
        
        private bool once;
        
        public string QuizElementKey
        {
            get => (string)GetValue(quizElementKeyDependency);
            set => SetValue(quizElementKeyDependency, value);
        }

        public ComponentMenu()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void BuildMenu()
        {
            if (!once)
            {
                MessageBox.Show("Building menu for: " + QuizElementKey);
                MenuItems.Add(UIBuilder.BuildComponentMenu(QuestionEditor.CurrentlyEditing.quizElements[QuizElementKey]));
                once = true;
            }
        }
        
		private static void OnPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
            ComponentMenu componentMenuObject = source as ComponentMenu;
            componentMenuObject?.BuildMenu();
		}
	}
}