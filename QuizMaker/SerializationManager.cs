using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker
{
	internal static class SerializationManager
	{

		public static SaveFile ActiveFile { get; private set; }

		public static void SaveCurrent()
		{

		}

		public static bool Load(string path)
		{
			return true;
		}

	}

	internal class SaveFile
	{

	}
}
