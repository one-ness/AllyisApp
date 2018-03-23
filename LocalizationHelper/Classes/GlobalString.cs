using System.Collections.Generic;

namespace LocalizationHelper.Classes
{
	public class GlobalString
    {
        public string Key {
            get; set;
        }                             //String Key
        public Dictionary<string, string> langStrings {
            get; set;
        } //Language - String

        public GlobalString()
        {
            langStrings = new Dictionary<string, string>();
        }
    }
}
