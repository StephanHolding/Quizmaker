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

        [DataMember]
        public string tag;
    }
}