using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.Category.Response
{
    public class CategoryResponse : ServerResponse
    {
        public CategoryResponse()
        {
            tubelessException.code = 200;
            tubelessException.message = "ok";
        }
        public List<CategoryItem> catlist = new List<CategoryItem>();
    }
}