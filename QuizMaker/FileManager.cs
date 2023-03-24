using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Data;
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
			allAvailableTags = new List<Tag>();
		}

		[DataMember]
		public List<QuizBlock> allBlocks;
		[DataMember]
		public List<Tag> allAvailableTags;

		public delegate void DataEvent();

		public event DataEvent OnTagsChanged;

		public void OnLoaded()
		{
			foreach (QuizBlock block in allBlocks)
			{
				block.InjectReferences();
			}
		}

		public QuizBlock AddBlock()
		{
			QuizBlock toReturn = new QuizBlock("Untitled Question", allBlocks.Count);
			allBlocks.Add(toReturn);
			return toReturn;
		}

		public void RemoveBlock(int index)
		{
			allBlocks.RemoveAt(index);
		}

		public QuizBlock GetBlock(int index)
		{
			return allBlocks[index];
		}

		public void AddAvailableTag(string tagValue)
		{
			allAvailableTags.Add(new Tag(tagValue));
			OnTagsChanged?.Invoke();
		}

		public void RemoveAvailableTag(int index)
		{
			allAvailableTags.RemoveAt(index);
			OnTagsChanged?.Invoke();
		}

		public void ChangeAvailableTagValue(int index, string newValue)
		{
			allAvailableTags[index].Set(newValue);
			OnTagsChanged?.Invoke();
		}

		public int GetTagIndex(string tagName)
		{
			for (int i = 0; i < allAvailableTags.Count; i++)
			{
				if (allAvailableTags[i].tag == tagName)
				{
					return i;
				}
			}

			return -1;
		}

		public void ReinsertQuizblock(int oldIndex, int newIndex)
		{
			QuizBlock toInsert = allBlocks[oldIndex];
			allBlocks.RemoveAt(oldIndex);
			allBlocks.Insert(newIndex, toInsert);
		}

	}
}