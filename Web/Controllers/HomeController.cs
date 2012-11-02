using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CoreSQL.Services;

namespace Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly CalleService _calleService;

        public HomeController()
        {
            this._calleService = new CalleService();
        }

		public ActionResult Index ()
		{
			return View ();
		}

        public ActionResult About()
        {
            return View();
        }

		[HttpGet]
        public ActionResult BuscarCalle(string query)
        {
            if (query.Length > 4)
            {
                var alturas = this._calleService.BuscarAlturas(query);
                return PartialView("Calles", alturas);
            }
            else
            {
                return PartialView("Calles", new List<CoreSQL.Entities.AlturasCalle>());
            }

        }

		public ActionResult Calles()
        {
            return View("Calles", _calleService.GetCalles());
        }
	}
}
