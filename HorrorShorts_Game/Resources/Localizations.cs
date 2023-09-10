using HorrorShorts_Game.Controls.UI.Dialogs;
using HorrorShorts_Game.Controls.UI.Questions;
using Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HorrorShorts_Game.Resources
{
    public class Localizations
    {
        public static GlobalLocalization Global { get; private set; }

        [ResourceAttribute("Data\\Localizations\\Story1")]
        public static LocalizationGroup Story1 { get; private set; }
#if DEBUG
        [ResourceAttribute("Data\\Localizations\\Test")]
        public static LocalizationGroup Test { get; private set; }
#endif

        public static void Init()
        {
            string path = GetPath("Global");
            Global = new(Core.Content.Load<GlobalLocalization_Serial>(path));
        }
        public static void ReLoad(string[] dialogs)
        {
            List<string> localizationsToLoad = new();
            List<string> localizationsToUnload = new();

            //Check localizations
            PropertyInfo[] props = typeof(Localizations).GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType != typeof(LocalizationGroup)) continue;

                LocalizationGroup d = (LocalizationGroup)props[i].GetValue(null);

                if (dialogs.Contains(props[i].Name))
                {
                    if (d == null)
                        localizationsToLoad.Add(props[i].Name);
                }
                else
                {
                    if (d != null)
                        localizationsToUnload.Add(props[i].Name);
                }
            }

            //todo: add in parallel task
            //Unload dialogs
            for (int i = 0; i < localizationsToUnload.Count; i++)
            {
                PropertyInfo propInfo = typeof(Localizations).GetProperty(localizationsToUnload[i]);
                propInfo.SetValue(null, null);
            }

            //Load dialogs
            for (int i = 0; i < localizationsToLoad.Count; i++)
            {
                PropertyInfo propInfo = typeof(Localizations).GetProperty(localizationsToLoad[i]);
                string path = ((ResourceAttribute)propInfo.GetCustomAttribute(typeof(ResourceAttribute), true)).Path;

                Localization_Serial ds = Core.Content.Load<Localization_Serial>(path);

                Dictionary<string, Dialog[]> dialogsData = new();
                foreach (Conversation_Serial cis in ds.Conversations)
                {
                    dialogsData.Add(cis.ID, new Dialog[cis.Dialogs.Length]);

                    for (int j = 0; j < cis.Dialogs.Length; j++)
                        dialogsData[cis.ID][j] = new(cis.Dialogs[j]);
                }

                Dictionary<string, Question> questionsData = new();
                foreach (QuestionGroup_Serial cis in ds.Questions)
                    questionsData.Add(cis.ID, new Question(cis.Question));

                LocalizationGroup data = new(dialogsData, questionsData);

                propInfo.SetValue(null, data);
            }
        }

        private static string GetPath(string name)
        {
            return $"Data\\Localizations\\{Core.Settings.Language}\\{name}";
        }
    }

    public class GlobalLocalization
    {
        public readonly string NewGame;
        public readonly string Continue;
        public readonly string Options;
        public readonly string Exit;

        public GlobalLocalization(GlobalLocalization_Serial serial)
        {
            FieldInfo[] localFields = GetType().GetFields();
            FieldInfo[] serialFields = serial.GetType().GetFields();

            for (int i = 0; i < serialFields.Length; i++)
            {
                FieldInfo field = Array.Find(localFields, x => x.Name == serialFields[i].Name);
                if (field == null)
                {
                    Logger.Warning($"Field '{serialFields[i].Name}' not found for Global localization");
                    continue;
                }

                field.SetValue(this, serialFields[i].GetValue(serial));
            }
        }
    }
    public class LocalizationGroup
    {
        public readonly Dictionary<string, Dialog[]> Dialogs;
        public readonly Dictionary<string, Question> Questions;

        public LocalizationGroup(Dictionary<string, Dialog[]> dialogs, Dictionary<string, Question> questions)
        {
            Dialogs = dialogs;
            Questions = questions;
        }
    }
}
