#if DEBUG
using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Resources;
using Resources;
using Resources.Dialogs;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HorrorShorts_Game.Tests
{
    public class Test1 : TestBase
    {
        public override void LoadContent1()
        {
            Textures.ReLoad(
                nameof(Textures.Mario),
                nameof(Textures.Megaman),
                nameof(Textures.Girl1));

            SpriteSheets.ReLoad(
                nameof(SpriteSheets.Mario),
                nameof(SpriteSheets.Megaman),
                nameof(SpriteSheets.Girl1));

            Animations.ReLoad(
                nameof(Animations.Megaman));

            //SpriteSheet_Serial ss = new SpriteSheet_Serial();
            //ss.Texture = "Mario";
            //ss.Sheets = new SingleSheet_Serial[2]
            //{
            //    new SingleSheet_Serial() { Name = "Sheet1", Source = new() },
            //    new SingleSheet_Serial() { Name = "Sheet1", Source = new() }
            //};
            ////ss.Save("Test.xml");


            //Conversation_Serial conversation = new();
            //conversation.Conversations = new ConversationItem_Serial[2];
            //conversation.Conversations[0] = new ConversationItem_Serial()
            //{
            //    ID = "Conversación 1",
            //    Dialogs = new Dialog_Serial[3]
            //    {
            //        new()
            //        {
            //            Character = Characters.Girl1,
            //            Face = FaceType.Angry,
            //            Location = Locations.Bottom,
            //            //Speak = SpeakType.Speak2,
            //            //SpeakSpeed = 4,
            //            //Text = "Thanks, that works. Im converting an old xna game to monogame. For some reason my game runs faster in xna then monogame. Is this normal. That is why Im trying to lock to 30 fps."
            //            Text = "Thanks, that works. Im converting an old xna game to {co:FF0000}monogame{co:FFFFFF}.{dy:500} For some reason my game runs faster in xna then monogame. {lb}Is this normal. That is why Im trying to lock to 30 fps."
            //            //Text = "¡Hola {co:FF0000}Axel{co:FFFFFF}! ¿Cómo estas? Yo bien. Andaría necesitando que me pases la imagen para poder terminar de armar esto."
            //            //Text = "Su marido la miró un momento, con {fs:3}{co:FF0000}brutal{co:FFFFFF}{fs:2} deseo de insultarla."
            //        },
            //        new()
            //        {
            //            Character = Characters.Girl1,
            //            Text = "Un saludo a la familia {co:FF0000}{ts:32}XDDDDDDDDDDDDDDDDDD",
            //            Face = FaceType.Happy
            //        },
            //        new()
            //        {
            //            Character = Characters.Girl1,
            //            Face = FaceType.Happy
            //        }
            //    }
            //};
            //conversation.Conversations[1] = new ConversationItem_Serial()
            //{
            //    ID = "Conversación 2",
            //    Dialogs = new Dialog_Serial[3]
            //    {
            //        new()
            //        {
            //            Character = Characters.Girl1,
            //            Face = FaceType.Happy
            //        },
            //        new()
            //        {
            //            Character = Characters.Girl1,
            //            Face = FaceType.Happy
            //        },
            //        new()
            //        {
            //            Character = Characters.Girl1,
            //            Face = FaceType.Happy
            //        }
            //    }
            //};
            //conversation.Save("Test.xml");

            //db.LoadContent();
            //db.Show(new Dialog(conversation.Conversations[0].Dialogs[0]));

            //Dialog[] dialogs = (Dialog[])Array.ConvertAll(conversation.Conversations[0].Dialogs, x => new Dialog(x));
            Core.DialogManagement.Start(Dialogs.Test["Conversación 1"]);
        }
        public override void Update1()
        {
            //db.Update();
        }
        public override void PreDraw1()
        {
            //db.PreDraw();
        }
        public override void Draw1()
        {
            //db.Draw();
        }
    }
}
#endif