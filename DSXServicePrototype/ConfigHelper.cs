using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype
{
    public static class ConfigHelper
    {
        static readonly AppSettingsReader Settings = new AppSettingsReader();

        public static int GetIntValue(string settingName)
        {
            return int.Parse(Settings.GetValue(settingName, type: Type.GetType("System.Int32")).ToString());
        }

        public static string GetStringValue(string settingName)
        {
            return Settings.GetValue(settingName, type: Type.GetType("System.String")).ToString();
        }
    }
}
