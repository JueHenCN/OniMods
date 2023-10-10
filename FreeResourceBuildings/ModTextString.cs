using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeResourceBuildings
{
    public class ModTextString
    {
        private static Dictionary<string, ItemInfo> ModItemInfos;

        public static ItemInfo GetModStrings(string itemID)
        {
            if (null == ModItemInfos || ModItemInfos.Count < 1)
            {
                SetShowText();
            }
            return ModItemInfos[itemID];
        }

        public static void SetShowText()
        {
            try
            {
                var lang = Localization.GetLocale().Lang;
                string modFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string languageFileName = "";
                switch (lang)
                {
                    case Localization.Language.Chinese:
                        languageFileName += "zh_CN.Json";
                        break;
                    default:
                        languageFileName += "en.Json";
                        break;
                }

                string filePath = System.IO.Path.Combine(modFolderPath, "Language");
                string languageFile = System.IO.Path.Combine(filePath, languageFileName);
                string json = System.IO.File.ReadAllText(languageFile);
                ModItemInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ItemInfo>>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"读取语言文件失败:{e.Message}");
            }
        }
    }

    public class ItemInfo
    {
        public string ID;
        public string Name;
        public string Effect;
        public string Description;

        public ItemInfo(string id, string name, string effect, string description)
        {
            ID = id;
            Name = name;
            Effect = effect;
            Description = description;
        }
    }
}
