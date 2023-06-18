using Microsoft.Xna.Framework.Content;
#if DEBUG
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
#endif
using System.Xml;

namespace Resources.Dialogs
{
    public class Conversation_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Conversation", FlattenContent = true)]
        public ConversationItem_Serial[] Conversations;

#if DEBUG
        public void Save(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            IntermediateSerializer.Serialize(writer, this, null);
        }
#endif
    }
    public class ConversationItem_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string ID;

        [ContentSerializer(AllowNull = false, Optional = false, CollectionItemName = "Dialog")]
        public Dialog_Serial[] Dialogs;
    }
}
