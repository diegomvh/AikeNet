using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Tools
{
	public class Text
	{
        private static List<List<char>> MISSPELL_CHARS = new List<List<char>>{ 
			{new List<char> {'a', 'á'}},
			{new List<char> {'e', 'é'}},
			{new List<char> {'i', 'í'}},
			{new List<char> {'o', 'ó'}},
			{new List<char> {'u', 'ú', 'ü'}} };

		public Text ()
		{

		}

		public static string misspellRegexpSafeString(string text) {
            StringBuilder sb = new StringBuilder();
            foreach (var character in text.ToLower().ToCharArray())
            {
                bool ready = false;
                foreach (var characters in MISSPELL_CHARS)
                {
                    if (characters.Contains(character)) {
                        sb.Append("[" + String.Join("", characters) + "]");
                        ready = true;
                        break;
                    }
                }
                if (! ready)
                    sb.Append(character);
            }
			return sb.ToString();
		}
	}
}

