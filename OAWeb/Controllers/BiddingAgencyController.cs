using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using Business.RMBusiness;
using Model.SystemModel;

namespace OAWeb.Controllers
{
    public class BiddingAgencyController : BaseController
    {
        BiddingAgencyBusiness service = new BiddingAgencyBusiness();
        // GET: BiddingAgency
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddIndex()
        {
            return View();
        }
        public ActionResult Add(RM_BiddingAgency entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.CreateUser = CurrentUser.Id;
            }
            else
            {
                entity.UpdateUser = CurrentUser.Id;
            }
            return Json(new JsonMessage(service.AddBiddingAgency(entity)));
        }


        public ActionResult GetModel(Guid id)
        {
            return EnumJson(service.GetModel(id));
        }
    }
}