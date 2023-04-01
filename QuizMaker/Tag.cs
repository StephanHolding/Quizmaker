using System.Runtime.Serialization;

namespace QuizMaker
{
    [DataContract]
    public class Tag
    {
        public Tag(string tag)
        {
            this.tag = tag;
        }

        public void Set(string tagValue)
        {
            tag = tagValue;
        }

        public bool IsSet()
        {
	        return !string.IsNullOrWhiteSpace(tag);
        }

        [DataMember]
        public string tag;
    }
}