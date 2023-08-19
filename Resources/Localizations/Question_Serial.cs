using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Localizations
{
    public class Question_Serial
    {
        [ContentSerializer(CollectionItemName = "Option", AllowNull = false, Optional = false)]
        public string[] Options = new string[] { "Option 1", "Option 2" };

        [ContentSerializer(AllowNull = true, Optional = true)]
        public sbyte? DefaultOption = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public QuestionBoxLocation? Location = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public HorizontalAlignament? TextAlign = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public FontType? Font = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public Color? SelectedTextColor = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public Color? OverTextColor = null;

        [ContentSerializer(AllowNull = true, Optional = true)]
        public Color? UnselectedTextColor = null;
    }

    public class QuestionGroup_Serial
    {
        [ContentSerializer(AllowNull = false, Optional = false)]
        public string ID;

        [ContentSerializer(AllowNull = false, Optional = false, FlattenContent = true)]
        public Question_Serial Question;
    }
}
