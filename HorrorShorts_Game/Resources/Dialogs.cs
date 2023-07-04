using HorrorShorts.Controls.Sprites;
using HorrorShorts.Controls.UI.Dialogs;
using Resources.Dialogs;
using Resources.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts.Resources
{
    public static class Dialogs
    {
        [ResourceAttribute("Data/Localization/Story1")]
        public static Dictionary<string, Dialog[]> Story1 { get; private set; }

#if DEBUG
        [ResourceAttribute("Data/Localization/Test")]
        public static Dictionary<string, Dialog[]> Test { get; private set; }
#endif

        public static void ReLoad(string[] dialogs)
        {
            List<string> dialogsToLoad = new();
            List<string> dialogsToUnload = new();

            //Check dialogs
            PropertyInfo[] props = typeof(Dialogs).GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType != typeof(Dictionary<string, Dialog[]>)) continue;

                Dictionary<string, Dialog[]> d = (Dictionary<string, Dialog[]>)props[i].GetValue(null);

                if (dialogs.Contains(props[i].Name))
                {
                    if (d == null)
                        dialogsToLoad.Add(props[i].Name);
                }
                else
                {
                    if (d != null)
                        dialogsToUnload.Add(props[i].Name);
                }
            }

            //todo: add in parallel task
            //Unload dialogs
            for (int i = 0; i < dialogsToUnload.Count; i++)
            {
                PropertyInfo propInfo = typeof(Dialogs).GetProperty(dialogsToUnload[i]);
                propInfo.SetValue(null, null);
            }

            //Load dialogs
            for (int i = 0; i < dialogsToLoad.Count; i++)
            {
                PropertyInfo propInfo = typeof(Dialogs).GetProperty(dialogsToLoad[i]);
                string path = ((ResourceAttribute)propInfo.GetCustomAttribute(typeof(ResourceAttribute), true)).Path;

                Conversation_Serial ds = Core.Content.Load<Conversation_Serial>(path);
                Dictionary<string, Dialog[]> s = new();

                foreach (ConversationItem_Serial cis in ds.Conversations)
                {
                    s.Add(cis.ID, new Dialog[cis.Dialogs.Length]);

                    for (int j = 0; j < cis.Dialogs.Length; j++)
                        s[cis.ID][j] = new(cis.Dialogs[j]);
                }

                propInfo.SetValue(null, s);
            }
        }
    }
}
