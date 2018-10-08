using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SystemModel
{
    public class PageModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Skip { get { return (Page - 1) * PageSize; } }

        public int BeginRow { get {return (Page - 1) * PageSize + 1; } }

        public int EndRow { get { return Page * PageSize ; } }
    }
}
