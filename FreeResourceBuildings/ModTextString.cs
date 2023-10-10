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
                var local = Localization.GetLocale();
                var lang = Localization.Language.Unspecified;
                if (null != local)
                {
                    lang = local.Lang;
                }
                string modFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                string languageFileName = "";
                switch (lang)
                {
                    case Localization.Language.Chinese:
                        languageFileName += "zh_CN.json";
                        break;
                    case Localization.Language.Japanese:
                        languageFileName += "ja.json";
                        break;
                    case Localization.Language.Korean:
                        languageFileName += "ko.json";
                        break;
                    case Localization.Language.Russian:
                        languageFileName += "ru.json";
                        break;
                    default:
                        languageFileName += "en.json";
                        break;
                }
                string filePath = System.IO.Path.Combine(modFolderPath, "Language");
                string languageFile = System.IO.Path.Combine(filePath, languageFileName);
                string jsonString = System.IO.File.ReadAllText(languageFile);
                ModItemInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ItemInfo>>(jsonString);
            }
            catch (Exception ex)
            {
                Debug.LogError($"语言文件读取异常:{ex}");
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
