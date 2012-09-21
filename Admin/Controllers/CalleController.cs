using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Services;
using Admin.Extensions;
using Admin.Tools;

namespace Admin.Controllers
{
	[Authorize(Users="dvanhaaster")]
    public class CalleController : Controller
    {
        private readonly CalleService _calleService;
        private readonly ZonaService _zonaService;

        public CalleController()
        {
            this._calleService = new CalleService();
            this._zonaService = new ZonaService();
        }

        //
        // GET: /Calle/

        public ActionResult Index(int pageIndex = 0, int pageSize = 10, string quickFilter = "")
        {
            var calles = (quickFilter == "") ? this._calleService.GetCalles() : this._calleService.BuscarCalles(quickFilter);
            var Paginator = new Paginator<Core.Models.Calle>(calles, pageIndex, pageSize);
            if (quickFilter != "")
                Paginator.RouteQuery = new Dictionary<string, object>() {
                    {"quickFilter", quickFilter}
                };
            Paginator.BaseUrl = Url.RouteUrl(this.RouteData.Values);
            this.ViewBag.QuickFilter = quickFilter;
            return View("Index", Paginator);
        }

		public ActionResult Edit(string id)
		{
			var calle = this._calleService.GetCalle(id);
			return this.View("Edit", calle);
		}

        [HttpPost]
        public ActionResult CreateEdit(FormCollection data)
        {
            try
            {
				var calle = (data["cId"] != null && data["cId"].Trim() != "")? this._calleService.GetCalle(data["cId"]) : this._calleService.Create();

				calle.nombre = data["nombre"];
				var errors = calle.Validate(new ValidationContext(calle, null, null));
				foreach (var error in errors)
					foreach (var memeber in error.MemberNames)
						ModelState.AddModelError(memeber, error.ErrorMessage);
				
				if (ModelState.IsValid) {
					this._calleService.Save(calle);
					return Json(new { Status = "ok",
                                      Url = this.Url.Action("Edit", "Calle", new { id = calle.idString })
                    }, JsonRequestBehavior.AllowGet);
				} else {
					return Json(new { Status = "error", Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
				}
            }
            catch
            {
                return View();
            }
        }
	
  		public ActionResult Delete(string id)
        {
            this._calleService.Delete(id);
            return this.Index();
        }

        [HttpPost]
        public ActionResult DeleteZona(string id, string zonaId)
        {
            try
            {
                this._zonaService.RemoveZona(id, zonaId);
                var calle = this._calleService.GetCalle(id);
                return PartialView("CalleZonas", calle);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult JsonZona(string id, string zonaId)
        {
            try
            {
                var zona = this._zonaService.GetZona(id, zonaId);
                return Json(zona.ToJson(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Zonas(string id) {
            try
            {
                var calle = this._calleService.GetCalle(id);
                return PartialView("CalleZonas", calle);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateEditZona(string id, FormCollection data)
        {
            try
            {
                var calle = this._calleService.GetCalle(id);
                var zona = (data["zId"] != null && data["zId"].Trim() != "")? calle.BuscarZona(data["zId"]) : this._zonaService.Create(calle);
                
                var altura = data["alturas"];
                zona.Update(data, (altura != null && altura != "false"));
                
                var errors = zona.Validate(new ValidationContext(zona, null, null));
                foreach (var error in errors)
                    foreach (var memeber in error.MemberNames)
                        ModelState.AddModelError(memeber, error.ErrorMessage);
                
                if (ModelState.IsValid) {
                    this._calleService.Save(calle);
                    return Json(new { Status = "ok", }, JsonRequestBehavior.AllowGet);
                } else {
                    return Json(new { Status = "error", Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
