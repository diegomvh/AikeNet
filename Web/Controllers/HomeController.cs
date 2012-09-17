using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Core.Services;

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
			// TODO: Validar que no sea cadena en blanco
			return PartialView("Calles", this._calleService.BuscarCalles(query));
        }

		public ActionResult Calles()
        {
            return View("Calles", _calleService.GetCalles());
        }
	}
}
