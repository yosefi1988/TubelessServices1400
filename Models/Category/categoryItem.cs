using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Category
{
    public class CategoryItem
    {
        public int ID { get; set; }
        public int? HID { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Statment { get; set; }
        public bool? Selectable { get; set; }
        public string SelectableS { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
    }
}