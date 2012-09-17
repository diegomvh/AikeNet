using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class Zona : IValidatableObject {

        public const int PAR = 1;
        public const int IMPAR = 2;
        public const int AMBAS = 3;

        [ScaffoldColumn(false)]
        [BsonId]
        public MongoDB.Bson.ObjectId id { get; set; }
		public int desde { get; set; }
		public int hasta { get; set; } 
		public int zona { get; set; }
		public int vereda { get; set; }

        public object ToJson() {
            return new
            {
                zId = this.id.ToString(),
                alturas = (this.desde != 0) && (this.hasta != 0),
                desde = this.desde,
                hasta = this.hasta,
                vereda = this.vereda,
                zona = this.zona
            };
        }

        public void LimpiarAlturas() {
            this.hasta = this.desde = 0;
            this.vereda = 3;
        }

        public void Update(NameValueCollection data) {
            // TODO: Reflection
            this.desde = int.Parse(data["desde"]);
            this.hasta = int.Parse(data["hasta"]);
            this.zona = int.Parse(data["zona"]);
            this.vereda = int.Parse(data["vereda"]);
        }

		public override string ToString ()
		{
			return string.Format ("[Zona: desde={0}, hasta={1}, zona={2}, vereda={3}]", desde, hasta, zona, vereda);
		}

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.vereda == Zona.PAR && ((this.desde % 2) != 0 || (this.hasta % 2) != 0))
                yield return new ValidationResult("Para veredas pares las alturas deben ser números pares", new [] { "Vereda" });
            else if (this.vereda == Zona.IMPAR && ((this.desde % 2) != 1 || (this.hasta % 2) != 1))
                yield return new ValidationResult("Para veredas impares las alturas deben ser números impares", new[] { "Vereda" });
        }
    }
}
