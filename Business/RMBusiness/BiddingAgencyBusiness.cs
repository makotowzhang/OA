using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Model.RMModel;
using Common;

namespace Business.RMBusiness
{
    public class BiddingAgencyBusiness
    {
        public bool AddBiddingAgency(RM_BiddingAgency entity)
        {
            using (DataProvider dp = new DataProvider())
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreateTime = DateTime.Now;
                    dp.RM_BiddingAgency.Add(entity);
                }
                else
                {
                    var model = dp.RM_BiddingAgency.FirstOrDefault(m => m.Id == entity.Id);
                    model.TenderType = entity.TenderType;
                    model.TenderWay = entity.TenderWay;
                    model.AgentMan = entity.AgentMan;
                    model.Salesman = entity.Salesman;
                    model.ProjectName = entity.ProjectName;
                    model.EntrustingParty = entity.EntrustingParty;
                    model.EPMan = entity.EPMan;
                    model.EPTel = entity.EPTel;
                    model.OpenTime = entity.OpenTime;
                    model.SignUpCount = entity.SignUpCount;
                    model.WinningUnit = entity.WinningUnit;
                    model.AuthorizedMan = entity.AuthorizedMan;
                    model.AuthorizedTel = entity.AuthorizedTel;
                    model.WinningAmount = entity.WinningAmount;
                    model.WinningM = entity.WinningM;
                    model.WorkTime = entity.WorkTime;
                    model.WorkDesc = entity.WorkDesc;
                    model.EnclosureName = entity.EnclosureName;
                    model.EnclusurePath = entity.EnclusurePath;
                    model.RegFee = entity.RegFee;
                    model.RegFeeWay = entity.RegFeeWay;
                    model.AgencyFee = entity.AgencyFee;
                    model.AgencyUnit = entity.AgencyUnit;
                    model.AppraisalFee = entity.AppraisalFee;
                    model.ChargeStatus = entity.ChargeStatus;
                    model.InvoiceStatus = entity.InvoiceStatus;
                    model.UpdateTime = DateTime.Now;
                }
                try
                {
                    dp.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public RM_BiddingAgency GetModel(Guid id)
        {
            using (DataProvider dp = new DataProvider())
            {
                return dp.RM_BiddingAgency.FirstOrDefault(m => m.Id == id);
            }
        }

        public List<RM_BiddingAgency> GetList(BiddingAgencyFilter filter, Guid userid, out int total)
        {
            using (DataProvider dp = new DataProvider())
            {

                //Guid? depid = dp.PM_Department.FirstOrDefault(m => m.DepName == "招标代理部")?.Id;
                //if (!dp.PM_Employee.Any(x => x.RelateUserId == userid && x.DepartmentId == depid))
                //{
                //    total = 0;
                //    return new List<RM_BiddingAgency>();
                //}
                var list = dp.RM_BiddingAgency.Where(m => true);
                if (filter.TenderType.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.TenderWay.Contains(m.TenderWay));
                }

                if (filter.TenderType.IsNotNullAndCountGtZero())
                {
                    list = list.Where(m => filter.TenderType.Contains(m.TenderType));
                }

                if (filter.AgentMan.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.AgentMan.Contains(filter.AgentMan));
                }
                if (filter.Salesman.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.Salesman.Contains(filter.Salesman));
                }
                if (filter.ProjectName.IsNotNullOrWhiteSpace())
                {
                    list = list.Where(m => m.ProjectName.Contains(filter.ProjectName));
                }
                list = list.OrderBy(m => m.CreateTime);
                total = list.Count();
                return list.Skip(filter.Skip).Take(filter.PageSize).ToList();
            }
        }
    }
}
