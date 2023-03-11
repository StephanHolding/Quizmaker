
namespace QuizMaker.Commands
{
	internal interface ICommand
	{
		void Execute();
		void Undo();

	}
}
