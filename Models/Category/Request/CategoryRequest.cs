using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TubelessServices.Models.Category.Request
{
    public class CategoryRequest
    {
        public int CategoryCode;
        public int ParentID;
        public int BrothersID;
        public int SelectableCategory;
    }

    public class CategoryRequest2
    {
        public int id;
        public int pageIndex;
        public int pageSize; 
    }
}