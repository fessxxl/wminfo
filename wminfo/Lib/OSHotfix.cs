﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wminfo.Lib
{
    class OSHotfix
    {
        public string HotfixID = "";
        public string Description = "";
        public string Caption = ""; //Microsoft KB URL
        public string InstalledBy = "";
        public string InstalledOn = "";

        public OSHotfix()
        {

        }

        public string ToTxt()
        {
            string result = "";
            result += "\n" + HotfixID;
            result += "\n--------------------\n";

            foreach (var propertyInfo in this.GetType().GetFields())
            {
                string name = propertyInfo.Name.PadRight(30);
                string value = propertyInfo.GetValue(this).ToString();
                result += name + " - " + value + "\n";
            }

            return result;
        }
    }
}
