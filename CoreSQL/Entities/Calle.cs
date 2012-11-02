using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using CoreSQL.Entities;

namespace CoreSQL.Entities 
{
    public partial class Calle
    {
        public List<string> OtrosNombres() {
            if (this.nombres.IndexOf(';') != -1)
                return new List<string>(this.nombres.Split(';'));
            else if (this.nombres == "")
                return new List<string>();
            else
                return new List<string>(new string[] {this.nombres});
        }

        public void agregarNombreAlternativo(string nombre) {
            var nombres = this.OtrosNombres();
            nombres.Add(nombre);
            this.nombres = String.Join(";", nombres);
        }
        
        public void Update(NameValueCollection data, bool todasLasAlturas)
        {
            if (todasLasAlturas)
            {
                
            }
            else
            {
                
            }
        }

        public object ToJson()
        {
            return new
            {
                
            };
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
