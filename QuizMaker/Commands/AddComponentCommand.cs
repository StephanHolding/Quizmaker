using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Commands
{
	internal class AddComponentCommand : ICommand
	{

		private readonly QuizElement componentOwner;
		private readonly Type typeToAdd;

		private QuizComponent createdInstance;

		public AddComponentCommand(QuizElement componentOwner, Type typeToAdd)
		{
			this.componentOwner = componentOwner;
			this.typeToAdd = typeToAdd;
		}

		public void Execute()
		{
			createdInstance = (QuizComponent)Activator.CreateInstance(typeToAdd);
			createdInstance.InjectReference(componentOwner);
			componentOwner.components.Add(createdInstance);
			componentOwner.RaiseDataChangedEvent();
		}

		public void Undo()
		{
			componentOwner.components.Remove(createdInstance);
			createdInstance = null;
			componentOwner.RaiseDataChangedEvent();
		}
	}

	internal class RemoveComponentCommand : ICommand
	{
		private readonly QuizComponent instanceToRemove;
		private readonly QuizElement componentOwner;

		public RemoveComponentCommand(QuizComponent componentToRemove, QuizElement componentOwner)
		{
			this.instanceToRemove = componentToRemove;
			this.componentOwner = componentOwner;
		}

		public void Execute()
		{
			componentOwner.components.Remove(instanceToRemove);
			componentOwner.RaiseDataChangedEvent();
		}

		public void Undo()
		{
			componentOwner.components.Add(instanceToRemove);
			componentOwner.RaiseDataChangedEvent();
		}
	}
}
