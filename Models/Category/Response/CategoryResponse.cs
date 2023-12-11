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
        public List<Viw_Site_Cat_level_1> cat1list = new List<Viw_Site_Cat_level_1>();
        public List<Viw_Site_Cat_level_2> cat2list = new List<Viw_Site_Cat_level_2>();
    }
}