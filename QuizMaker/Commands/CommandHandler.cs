using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Commands
{
	internal static class CommandHandler
	{

		private static readonly Stack<ICommand> commands = new Stack<ICommand>();
		private static readonly Stack<ICommand> redoStack = new Stack<ICommand>();

		public static void ExecuteCommand(ICommand commandInstance)
		{
			redoStack.Clear();
			ExecuteAndPush(commandInstance);
		}

		public static void Undo()
		{
			if (commands.Count > 0)
			{
				ICommand toUndo = commands.Pop();
				toUndo.Undo();
				redoStack.Push(toUndo);
			}
		}

		public static void Redo()
		{
			if (redoStack.Count > 0)
			{
				ICommand toRedo = redoStack.Pop();
				ExecuteAndPush(toRedo);
			}
		}

		public static void ClearCommandStack()
		{
			commands.Clear();
		}

		private static void ExecuteAndPush(ICommand commandInstance)
		{
			commandInstance.Execute();
			commands.Push(commandInstance);
		}

	}
}
