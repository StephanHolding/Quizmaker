using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace QuizMaker
{
	internal static class FileManager
	{
		public static SaveFile CurrentFile { get; private set; }

		public const string EXTENSION_FILTER = "Quiz files (*.quiz)|*.quiz";

		public static void NewFile()
		{
			CurrentFile = new SaveFile();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		public static void SaveCurrent(string path)
		{
			DataContractSerializer serializer = new DataContractSerializer(typeof(SaveFile), TypeHolder.types);

			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(streamWriter))
				{
					xmlTextWriter.Formatting = Formatting.Indented;
					serializer.WriteObject(xmlTextWriter, CurrentFile);
					xmlTextWriter.Flush();
				}
			}
		}

		public static bool Load(string path)
		{
			DataContractSerializer serializer = new DataContractSerializer(typeof(SaveFile), TypeHolder.types);
			using (StreamReader streamReader = new StreamReader(path))
			{
				using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
				{
					SaveFile loaded = (SaveFile)serializer.ReadObject(xmlTextReader);

					if (loaded != null)
					{
						CurrentFile = loaded;
						CurrentFile.OnLoaded();
						return true;
					}
				}
			}
			return false;
		}
	}

	[DataContract]
	public class SaveFile
	{
		public SaveFile()
		{
			allBlocks = new List<QuizBlock>();
			tags = new List<Tag>();
		}

		[DataMember]
		public List<QuizBlock> allBlocks;
		[DataMember]
		public List<Tag> tags;

		public void OnLoaded()
		{
			foreach (QuizBlock block in allBlocks)
			{
				block.InjectReferences();
			}
		}

		public QuizBlock AddBlock()
		{
			QuizBlock toReturn = new QuizBlock();
			allBlocks.Add(toReturn);
			return toReturn;
		}

		public QuizBlock GetBlock(int index)
		{
			return allBlocks[index];
		}

		public QuizBlock GetLatest()
		{
			try
			{
				return allBlocks[allBlocks.Count - 1];
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}