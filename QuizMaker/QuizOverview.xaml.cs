using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaker
{
    /// <summary>
    /// Interaction logic for QuizOverview.xaml
    /// </summary>
    public partial class QuizOverview : Page
    {
        public QuizOverview()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
/*			SaveFile current = SerializationManager.CurrentFile;

			foreach (Question question in current.questions)
			{
				listbox.Items.Add(question.GetQuestionRepresentation());
			}*/
        }

        private void AddNewQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            QuizBlock newQ = FileManager.CurrentFile.AddBlock();
            newQ.quizElements["question"].AddComponent<TextComponent>();
            newQ.quizElements["answer"].AddComponent<TextComponent>();

            MainWindow.ShowPage(new QuestionEditor(newQ));
        }

        private void EditQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            //edit the selected queston
           // MainWindow.ShowPage(new QuestionEditor());
        }
    }
}