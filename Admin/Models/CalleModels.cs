using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using CoreSQL.Entities;
using System.Linq;

namespace Admin.Models
{

    public class EditCalleModel
    {
        private Calle _calle = null;
        public Calle Calle
        {
            get {
                if (this._calle == null)
                    this._calle = new CoreSQL.Entities.Calle();
                return this._calle;

            }
            set {
                this._calle = value;
            }
        }
        public IQueryable<Altura> Alturas { get; set; }

        [ScaffoldColumn(false)]
        public bool Segmentada
        {
            get
            {
                if (this.Alturas.Count() > 1)
                    return true;
                if (this.Alturas.Count() == 1)
                    return this.Alturas.First().Tipo != Altura.ALTURA_ALTURAS;
                return false;
            }
        }


        [ScaffoldColumn(false)]
        public string DescripcionHtml
        {
            get
            {
                if (this.Calle.nombre != null && this.Calle.nombre != "")
                    return this.Calle.nombre + "<p class='text-info'><small>" + this.Calle.nombres + "</small></p>";
                return this.Calle.nombre;
            }
        }

    }
}
