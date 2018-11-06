using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SystemModel;

namespace Model.PMModel
{
    public class MaterielModel:CommonModel
    {
        public int Id { get; set; }
        public string MaterielCode { get; set; }
        public string MaterielName { get; set; }
        public Guid MaterielType { get; set; }
        public int ResidualQuantity { get; set; }
        public int Sort { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDel { get; set; }

        public string MaterielTypeDesc { get; set; }
    }

    public class MaterielFilter : PageModel
    {
        public string MaterielCode { get; set; }
        public string MaterielName { get; set; }
        public List<Guid> MaterielType { get; set; }
    }
}
