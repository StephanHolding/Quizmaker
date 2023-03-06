using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker
{

	public static class TypeHolder
	{
		public static Type[] types =
{
			typeof(TextQuestion),
			typeof(Answer),
			typeof(TextAnswer)
		};
	}

	[System.Serializable]
	public class Question
	{


		public Answer correctAnswer;
		public List<Answer> incorrectAnswers;
	}

	[System.Serializable]
	public class TextQuestion : Question 
	{
		public string question;
	}

	[System.Serializable]
	public class Answer
	{

	}

	[System.Serializable]
	public class TextAnswer : Answer
	{
		public string answer;
	}

}
