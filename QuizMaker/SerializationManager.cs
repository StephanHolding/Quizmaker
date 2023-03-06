using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QuizMaker
{
	internal static class SerializationManager
	{

		public static SaveFile ActiveFile { get; private set; }

		public const string EXTENTION_FILTER = "Quiz files (*.quiz)|*.quiz";

		public static void CreateMockFile()
		{
			ActiveFile = new SaveFile()
			{
				questions = new List<Question>()
				{
					new TextQuestion() { question = "TEST"}
				}
			};
		}

		public static void SaveCurrent(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveFile), TypeHolder.types);
			
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				xmlSerializer.Serialize(streamWriter, ActiveFile);
			}
		}

		public static bool Load(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveFile), TypeHolder.types);
			using (StreamReader streamReader = new StreamReader(path))
			{
				SaveFile loaded = (SaveFile)xmlSerializer.Deserialize(streamReader);

				if (loaded != null)
				{
					ActiveFile = loaded;
					return true;
				}
			}

			return false;
		}

	}

	[System.Serializable]
	public class SaveFile
	{
		public List<Question> questions;
	}
}
