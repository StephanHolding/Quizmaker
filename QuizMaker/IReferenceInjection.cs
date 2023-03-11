using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker
{
	internal interface IReferenceInjection
	{
		void InjectReference(params object[] args);
	}
}
