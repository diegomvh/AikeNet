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
    public partial class Altura
    {
        public const string ALTURA_DOMICILIO = "domicilio";
        public const string ALTURA_RANGO = "rango";
        public const string ALTURA_ALTURAS = "alturas";
        public const int PAR = 1;
        public const int IMPAR = 2;
        public const int AMBAS = 3;

        public string Tipo
        {
            get {
                if (this.desde != this.hasta)
                    return Altura.ALTURA_RANGO;
                else if (this.desde == this.hasta && this.desde != 0)
                    return Altura.ALTURA_DOMICILIO;
                else
                {
                    return Altura.ALTURA_ALTURAS;
                }
            }
        }

        public void Update(NameValueCollection data, string tipo)
        {
            // TODO: Esto del barrio esta feo
            this.barrio = "";
            this.zona = short.Parse(data["zona"]);
            if (tipo == Altura.ALTURA_DOMICILIO)
            {
                this.desde = this.hasta = int.Parse(data["desde"]);
                this.vereda = Altura.AMBAS;
            }
            else if (tipo == Altura.ALTURA_RANGO)
            {
                this.desde = int.Parse(data["desde"]);
                this.hasta = int.Parse(data["hasta"]);
                this.vereda = short.Parse(data["vereda"]);
            }
            else if (tipo == Altura.ALTURA_ALTURAS)
            {
                this.desde = this.hasta = 0;
                this.vereda = 3;
            }
        }

        public object ToJson()
        {
            
            return new
            {
                aId = this.id,
                desde = this.desde,
                hasta = this.hasta,
                vereda = this.vereda,
                zona = this.zona,
                optionsAltura = this.Tipo
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
