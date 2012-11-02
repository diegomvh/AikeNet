using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreSQL.Services;
using Admin.Extensions;
using Admin.Tools;
using Admin.Models;

namespace Admin.Controllers
{
	[Authorize(Roles="ggDptoDesarrollo")]
    public class CalleController : Controller
    {
        private readonly CalleService _calleService;

        public CalleController()
        {
            this._calleService = new CalleService();
        }

        //
        // GET: /Calle/

        public ActionResult Index(int pageIndex = 0, int pageSize = 10, string quickFilter = "")
        {
            var calles = (quickFilter == "") ? this._calleService.GetCalles() : this._calleService.BuscarCalles(quickFilter);
            var Paginator = new Paginator<CoreSQL.Entities.Calle>(calles, pageIndex, pageSize);
            if (quickFilter != "")
                Paginator.RouteQuery = new Dictionary<string, object>() {
                    {"quickFilter", quickFilter}
                };
            Paginator.BaseUrl = Url.RouteUrl(this.RouteData.Values);
            this.ViewBag.QuickFilter = quickFilter;
            return View("Index", Paginator);
        }

		public ActionResult Edit(int id)
		{
            var model = new EditCalleModel();
			model.Calle = this._calleService.GetCalle(id);
            model.Alturas = this._calleService.GetAlturas(model.Calle);
            return this.View("Edit", model);
		}

        [HttpPost]
        public ActionResult CreateEdit(FormCollection data)
        {
            try
            {
				var calle = (data["cId"] != null && data["cId"].Trim() != "")? this._calleService.GetCalle(int.Parse(data["cId"])) : this._calleService.Create();

                if (data["nombres"] != null)
                    calle.agregarNombreAlternativo(data["nombres"]);
                else if (data["nombre"] != null)
                    calle.nombre = data["nombre"];
				var errors = calle.Validate(new ValidationContext(calle, null, null));
				foreach (var error in errors)
					foreach (var memeber in error.MemberNames)
						ModelState.AddModelError(memeber, error.ErrorMessage);
				
				if (ModelState.IsValid) {
					this._calleService.Save(calle);
					return Json(new { Status = "ok",
                                      Url = this.Url.Action("Edit", "Calle", new { id = calle.id })
                    }, JsonRequestBehavior.AllowGet);
				} else {
					return this.Json(new { Status = "error", Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
				}
            }
            catch (Exception ex)
            {
                return View();
            }
        }
	
  		public ActionResult Delete(int id)
        {
            this._calleService.Delete(id);
            return this.RedirectToAction("Index");
        }

		[HttpPost]
		public ActionResult AgregarNombre(int id, FormCollection data)
		{
			try
			{
				var calle = this._calleService.GetCalle(id);
				calle.agregarNombreAlternativo(data["nombre"]);
				if (ModelState.IsValid) {
					this._calleService.Save(calle);
					return this.Json(new { Status = "ok", }, JsonRequestBehavior.AllowGet);
				} else {
					return this.Json(new { Status = "error", Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
				}
			}
			catch
			{
				return View();
			}
		}

        [HttpPost]
        public ActionResult DeleteZona(int id, int alturaId)
        {
            try
            {
                this._calleService.EliminarAltura(id, alturaId);
                var model = new Admin.Models.EditCalleModel();
                model.Calle = this._calleService.GetCalle(id);
                model.Alturas = this._calleService.GetAlturas(model.Calle);
                return PartialView("CalleAlturas", model);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult JsonAltura(int id, int alturaId)
        {
            try
            {
                var zona = this._calleService.GetAltura(id, alturaId);
                return Json(zona.ToJson(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Zonas(int id) {
            try
            {
                var model = new Admin.Models.EditCalleModel();
                model.Calle = this._calleService.GetCalle(id);
                model.Alturas = this._calleService.GetAlturas(model.Calle);
                return PartialView("CalleAlturas", model);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateEditZona(int id, FormCollection data)
        {
            try
            {
                var calle = this._calleService.GetCalle(id);
                var altura = (data["aId"] != null && data["aId"].Trim() != "") ? this._calleService.GetAltura(calle, int.Parse(data["aId"])) : this._calleService.CreateAltura(calle);

                altura.Update(data, data["optionsAltura"]);

                var errors = altura.Validate(new ValidationContext(altura, null, null));
                foreach (var error in errors)
                    foreach (var memeber in error.MemberNames)
                        ModelState.AddModelError(memeber, error.ErrorMessage);
                
                if (ModelState.IsValid) {
                    this._calleService.Save(altura);
                    return Json(new { Status = "ok", }, JsonRequestBehavior.AllowGet);
                } else {
                    return Json(new { Status = "error", Errors = ModelState.Errors() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
