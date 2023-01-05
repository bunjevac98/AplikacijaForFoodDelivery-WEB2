using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;



namespace Web2.Controllers
{
    public class EmailsController : Controller
    {
        // GET: Emails
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewCommentEmail mail)
        {


            return RedirectToAction("Index");
        }





    }
}