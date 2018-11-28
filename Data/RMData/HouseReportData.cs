using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.RMModel;
using Entity;

namespace Data.RMData
{
    public class HouseReportData
    {
        public RM_HouseReport GetEntity(DataProvider dp ,Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            else
            {
                return dp.RM_HouseReport.FirstOrDefault(m => m.Id == id);
            }
        }
    }
}
