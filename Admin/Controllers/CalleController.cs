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
            return View(Paginator);
        }

        //
        // GET: /Calle/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Calle/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Calle/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Calle/Edit/5

        public ActionResult Edit(string id)
        {

            return View(this._calleService.GetCalle(id));
        }

        //
        // POST: /Calle/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Calle/Delete/5

        public ActionResult Delete(string id)
        {
            this._calleService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteZona(string id, string zonaId)
        {
            try
            {
                this._zonaService.RemoveZona(id, zonaId);
                var calle = this._calleService.GetCalle(id);
                return PartialView("ZonaTableRow", calle.zonas);
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
                return PartialView("ZonaTableRow", calle);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditZona(string id, FormCollection data)
        {
            try
            {
                var calle = this._calleService.GetCalle(id);
                var zona = (data["zId"] != null && data["zId"].Trim() != "")? calle.BuscarZona(data["zId"]) : this._zonaService.Create(calle);
                
                var altura = data["alturas"];
                if (altura != null && bool.Parse(altura))
                    zona.LimpiarAlturas();
                else
                {
                    zona.Update(data);
                }
                var errors = zona.Validate(new ValidationContext(zona, null, null));
                foreach (var error in errors)
                    foreach (var memeber in error.MemberNames)
                        ModelState.AddModelError(memeber, error.ErrorMessage);
                
                if (ModelState.IsValid) {
                    this._calleService.Edit(calle);
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
