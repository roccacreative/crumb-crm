using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CrumbCRM.Web.Helpers
{
    public static class StringHelper
    {
        public static string AddSpacesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}