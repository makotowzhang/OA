using Model.SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RMModel
{
    public class BiddingAgencyFilter : PageModel
    {
        public List<string> TenderType { get; set; }
        public List<string> TenderWay { get; set; }
        public string AgentMan { get; set; }
        public string Salesman { get; set; }
        public string ProjectName { get; set; }
    }
}
