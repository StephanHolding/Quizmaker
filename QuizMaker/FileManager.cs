using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        public static void SaveCurrent(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveFile), TypeHolder.types);

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                xmlSerializer.Serialize(streamWriter, CurrentFile);
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
                    CurrentFile = loaded;
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    public class SaveFile
    {
        public SaveFile()
        {
            allBlocks = new List<QuizBlock>();
            tags = new List<Tag>();
        }

        public List<QuizBlock> allBlocks;
        public List<Tag> tags;

        public QuizBlock AddBlock()
        {
            QuizBlock toReturn = new QuizBlock();
            allBlocks.Add(toReturn);
            return toReturn;
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