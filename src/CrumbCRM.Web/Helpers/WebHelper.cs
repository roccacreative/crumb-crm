using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CrumbCRM.Web.Helpers
{
    public static class WebHelper
    {
        public static void SelectListEnumViewData<T>(this ViewDataDictionary viewData, string valueField, bool autoAddSpaces = true, int? selectedValue = null) where T : struct
        {
            var values = from T e in Enum.GetValues(typeof(T))
                         select new
                         {
                             ID = (int)Enum.Parse(typeof(T), e.ToString()),
                             Name = autoAddSpaces ? e.ToString().AddSpacesToSentence() : e.ToString()
                         };

            viewData[valueField] = new SelectList(values, "ID", "Name", selectedValue);
        }
    }
}