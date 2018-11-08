using System;
using System.Collections.Generic;
using System.Linq;
using Model.PMModel;
using Entity;

namespace Data.PMData
{
    public class MaterielApplyData
    {
        public PM_MaterielApply GetPMAModel(DataProvider dp,Guid id)
        {
            return dp.PM_MaterielApply.FirstOrDefault(m => m.Id == id);
        }

        //public List<MaterielApplyModel> GetPMAList(DataProvider dp, MaterielApplyFilter filter, out int total, bool isPage = true)
        //{
        //    var list=from pma in dp.PM_MaterielApply
        //             join su in dp.System_User on pma.CreateUser equals su.Id
        //             join 
        //}
    }
}
