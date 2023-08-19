using Microsoft.Xna.Framework.Content;
#if DEBUG
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
#endif
using System.Xml;

namespace Resources.Localizations
{
    public class Localization_Serial
    {
        [ContentSerializer(AllowNull = true, Optional = false, CollectionItemName = "Conversation")]
        public Conversation_Serial[] Conversations;

        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Question")]
        public QuestionGroup_Serial[] Questions;

#if DEBUG
        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
#endif
    }
}
