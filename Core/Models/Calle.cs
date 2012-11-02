using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CoreMongo.Models
{
	[BsonIgnoreExtraElements]
	public class Calle : IValidatableObject {
		[ScaffoldColumn(false)]
        [BsonId]
	    public MongoDB.Bson.ObjectId id { get; set; }

		[Required]
	    public string nombre { get; set; }

		[ScaffoldColumn(false)]
		public IList<string> nombres { get; set; }

	 	[ScaffoldColumn(false)]
	    public IList<Zona> zonas { get; set; }

        [ScaffoldColumn(false)]
        public string idString {
            get {
                return this.id.ToString();
            }
        }

        [ScaffoldColumn(false)]
        public bool Segmentada {
            get {
                if (this.zonas.Count > 1)
                    return true;
                return false;
            }
        }

		[ScaffoldColumn(false)]
		public string DescripcionHtml {
			get {
				if (this.nombres != null && this.nombres.Count > 0)
                    return this.nombre + "<p class='text-info'><small>" + string.Join(", ", this.nombres) + "</small></p>";
				return this.nombre;
			}
		}

        [ScaffoldColumn(false)]
        public bool Alturas {
            get {
                if (this.zonas.Count == 1)
                {
                    var zona = this.zonas[0];
                    return (zona.desde != 0) && (zona.hasta != 0);
                }
                return this.zonas.Count > 1;
            }
        }
                

        [ScaffoldColumn(false)]
        public int Zona {
            get {
				if (this.zonas.Count > 0)
                	return this.zonas[0].zona;
				return 0;
            }
        }

        public Zona BuscarZona(string id) {
            var zid = MongoDB.Bson.ObjectId.Parse(id);
            foreach (var zona in this.zonas) {
                if (zona.id == zid)
                    return zona;
            }
            return null;
        }

		public void agregarNombreAlternativo(string nombre)
		{
			if (this.nombres == null)
				this.nombres = new List<string>();
			this.nombres.Add(nombre);
		}

		public void quitarNombreAlternativo(string nombre)
		{
			this.nombres.Remove(nombre);
			if (this.nombres.Count == 0)
				this.nombres = null;
		}



		public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			/* if (this.vereda == Zona.PAR && ((this.desde % 2) != 0 || (this.hasta % 2) != 0))
				yield return new ValidationResult("Para veredas pares las alturas deben ser números pares", new [] { "Vereda" });
			else if (this.vereda == Zona.IMPAR && ((this.desde % 2) != 1 || (this.hasta % 2) != 1))
				yield return new ValidationResult("Para veredas impares las alturas deben ser números impares", new[] { "Vereda" });
				*/
			return new List<ValidationResult>();
		}
	}
}
