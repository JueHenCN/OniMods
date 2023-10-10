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
            
            var lang = Localization.GetLocale().Lang;
            switch (lang)
            {
                case Localization.Language.Chinese:
                    ModItemInfos = new Dictionary<string, ItemInfo>()
                    {
                        {
                            "FreeEnergyGenerator", new ItemInfo("FreeEnergyGenerator", "Power Box", "Keep generating energy for free",
                            "It allows you to have a free source of electricity")
                        },
                        {
                            "FreeLiquidSource", new ItemInfo("FreeLiquidSource", "Liquid Source", "Keep providing you the selected liquid",
                            "It allows you to have a free source of liquids\nYou can change how much and the temperature of that liquid")
                        },
                        {
                            "FreeLiquidSink", new ItemInfo("FreeLiquidSink", "Liquid Sink", "A way to flush unwanted liquids",
                            "It allows you to have a sink of liquids\nAll liquids that you send here, will disappear")
                        },
                        {
                            "FreeGasSource", new ItemInfo("FreeGasSource", "Gas Source", "Keep providing you the selected liquid",
                            "It allows you to have a free source of gases\nYou can change how much and the temperature of that gas")
                        },
                        {
                            "FreeGasSink", new ItemInfo("FreeGasSink", "Gas Sink", "A way to vanish unwanted gases",
                            "It allows you to have a sink of gases\nAll gases that you send here, will disappear")
                        },
                        {
                            "StorageGenerator", new ItemInfo("StorageGenerator", "Magic Storage", "Keep generating a generous amount of selected items",
                            "It allows you to have a free source of items\nYou can customize which items it generates")
                        },
                        {
                            "MagicFeeder", new ItemInfo("MagicFeeder", "Magic Feeder", "Keep generating a generous amount food for your critter",
                            "It provides you a free source of food for critters")
                        },
                        {
                            "MagicFishFeeder", new ItemInfo("MagicFishFeeder", "Magic Fish Feeder", "Keep generating a generous amount food for your pacus",
                            "It provides you a free source of food for pacus")
                        },
                        {
                            "FoodStorageGenerator", new ItemInfo("FoodStorageGenerator", "Magic Refrigerator", "Keep generating a generous amount of selected food",
                            "It allows you to have a free source of food, you can customize which items it generates")
                        },
                        {
                            "WardobreStorage", new ItemInfo("WardobreStorage", "Magic Wardrobe", "Provides you clothes and suits",
                            "It provides you a free source of clothes and suits, mark collect only to avoid of loose your other items")
                        },
                        {
                            "FirstAidBoxStorage", new ItemInfo("FirstAidBoxStorage", "FirstAid Box", "Provides you free meds",
                            "It provides you a free source of meds, mark collect only to avoid of loose your other items\nHealth is an important thing")
                        },
                        {
                            "FreeRadboltStorage", new ItemInfo("FreeRadboltStorage", "Improvised Radbolt", "Keep generating radbolts for free",
                            "It allows you to have a free source of radbolts\nYou can customize how many radbolts it generates per cycle")
                        },
                        {
                            "FarmerStorageStorage", new ItemInfo("FarmerStorageStorage", "Farmer Shelf", "A free source of eggs, seeds and agriculture stuff",
                            "It allows you to have a free source of eggs, seeds and agriculture stuff\nYou can customize what items you want")
                        },
                        {
                            "TrashcanStorage", new ItemInfo("TrashcanStorage", "Trash Can", "A trash can, you can trashout your useless items",
                            "It allows you to trashout your your useless items, you need to select on the filter what items you want to trashout\n<b>Mark collect only to avoid unwanted destruction</b>")
                        },
                    };
                    break;
                default:
                    ModItemInfos = new Dictionary<string, ItemInfo>()
                    {
                        {
                            "FreeEnergyGenerator", new ItemInfo("FreeEnergyGenerator", "Power Box", "Keep generating energy for free",
                            "It allows you to have a free source of electricity")
                        },
                        {
                            "FreeLiquidSource", new ItemInfo("FreeLiquidSource", "Liquid Source", "Keep providing you the selected liquid",
                            "It allows you to have a free source of liquids\nYou can change how much and the temperature of that liquid")
                        },
                        {
                            "FreeLiquidSink", new ItemInfo("FreeLiquidSink", "Liquid Sink", "A way to flush unwanted liquids",
                            "It allows you to have a sink of liquids\nAll liquids that you send here, will disappear")
                        },
                        {
                            "FreeGasSource", new ItemInfo("FreeGasSource", "Gas Source", "Keep providing you the selected liquid",
                            "It allows you to have a free source of gases\nYou can change how much and the temperature of that gas")
                        },
                        {
                            "FreeGasSink", new ItemInfo("FreeGasSink", "Gas Sink", "A way to vanish unwanted gases",
                            "It allows you to have a sink of gases\nAll gases that you send here, will disappear")
                        },
                        {
                            "StorageGenerator", new ItemInfo("StorageGenerator", "Magic Storage", "Keep generating a generous amount of selected items",
                            "It allows you to have a free source of items\nYou can customize which items it generates")
                        },
                        {
                            "MagicFeeder", new ItemInfo("MagicFeeder", "Magic Feeder", "Keep generating a generous amount food for your critter",
                            "It provides you a free source of food for critters")
                        },
                        {
                            "MagicFishFeeder", new ItemInfo("MagicFishFeeder", "Magic Fish Feeder", "Keep generating a generous amount food for your pacus",
                            "It provides you a free source of food for pacus")
                        },
                        {
                            "FoodStorageGenerator", new ItemInfo("FoodStorageGenerator", "Magic Refrigerator", "Keep generating a generous amount of selected food",
                            "It allows you to have a free source of food, you can customize which items it generates")
                        },
                        {
                            "WardobreStorage", new ItemInfo("WardobreStorage", "Magic Wardrobe", "Provides you clothes and suits",
                            "It provides you a free source of clothes and suits, mark collect only to avoid of loose your other items")
                        },
                        {
                            "FirstAidBoxStorage", new ItemInfo("FirstAidBoxStorage", "FirstAid Box", "Provides you free meds",
                            "It provides you a free source of meds, mark collect only to avoid of loose your other items\nHealth is an important thing")
                        },
                        {
                            "FreeRadboltStorage", new ItemInfo("FreeRadboltStorage", "Improvised Radbolt", "Keep generating radbolts for free",
                            "It allows you to have a free source of radbolts\nYou can customize how many radbolts it generates per cycle")
                        },
                        {
                            "FarmerStorageStorage", new ItemInfo("FarmerStorageStorage", "Farmer Shelf", "A free source of eggs, seeds and agriculture stuff",
                            "It allows you to have a free source of eggs, seeds and agriculture stuff\nYou can customize what items you want")
                        },
                        {
                            "TrashcanStorage", new ItemInfo("TrashcanStorage", "Trash Can", "A trash can, you can trashout your useless items",
                            "It allows you to trashout your your useless items, you need to select on the filter what items you want to trashout\n<b>Mark collect only to avoid unwanted destruction</b>")
                        },
                    };
                    break;
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
