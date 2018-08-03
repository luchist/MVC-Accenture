using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_accenture.Controllers
{
    public class ParcialController : Controller
    {
        //
        // GET: /Parcial/

        public ActionResult Index()
        {
            ViewBag.Hora = DateTime.Now.ToString();
            return View();
        }

        public ActionResult vistaParcial()
        {
            ViewBag.Hora = DateTime.Now.ToString();
            return PartialView("_VistaParcial");
        }

    }

}