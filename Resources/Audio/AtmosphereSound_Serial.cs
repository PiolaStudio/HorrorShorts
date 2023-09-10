using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Resources.Audio
{
    public class AtmosphereSound_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public SoundType Sound = SoundType.Field1_Atmosphere;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public float? LoopDelay = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public float? BaseVolume = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public float? PitchMin = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public float? PitchMax = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public bool? PanAllowed = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public bool? IsGlobalSound = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public int? PlayRange = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public Point? Origin = null;
        [ContentSerializer(AllowNull = true, Optional = true)]
        public bool? StopWhenNotUse = null;

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
