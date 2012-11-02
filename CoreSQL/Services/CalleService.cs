using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CoreSQL.Entities;

namespace CoreSQL.Services
{
	public class CalleService
	{

        private readonly AikeEntities _db;

		public CalleService ()
		{
            this._db = AikeEntities.Instance();
        }

        public Calle Create()
        {
            return new CoreSQL.Entities.Calle();
        }

        public Altura CreateAltura(Calle calle)
        {
            var altura = new CoreSQL.Entities.Altura();
            altura.calle_id = calle.id;
            return altura;
        }

        public void Save(Calle calle)
        {
            if (calle.id == 0)
            {
                if (calle.nombres == null)
                    calle.nombres = "";
                this._db.zonas_calle.AddObject(calle);
            }
            this._db.SaveChanges();
        }

        public void Save(Altura altura)
        {
            // TODO: Hacer un update
            if (altura.id == 0) {
                if (altura.barrio == null)
                    altura.barrio = "";
                this._db.zonas_altura.AddObject(altura);
            }
            this._db.SaveChanges();
        }

        public void Delete(int cid)
        {
            try
            {
                var calle = this._db.zonas_calle.First(c => c.id == cid);
                this._db.zonas_calle.DeleteObject(calle);
                this._db.SaveChanges();
            }
            catch (Exception ex) { 
                //TODO
            }
        }

        public void EliminarAltura(int cid, int alturaId) {
            try
            {
                var altura = this._db.zonas_altura.First(a => a.id == alturaId);
                this._db.zonas_altura.DeleteObject(altura);
                this._db.SaveChanges();
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        public Calle GetCalle(int cid)
        {
            return this._db.zonas_calle.First(c => c.id == cid);
        }


        public long TotalCalles() {
            /* return this._calles.Collection.Count(); */
            return 10;
        }

        public IQueryable<CoreSQL.Entities.Calle> BuscarCalles(string nombre)
        {
            return this._db.zonas_calle.Where(c => c.nombre.Contains(nombre)).OrderBy(c => c.nombre);
        }

        public IQueryable<CoreSQL.Entities.AlturasCalle> BuscarAlturas(string consulta)
        {
			var altura = -1;
			var nombres = new List<string>();
			var parts = consulta.Split(' ');
			Array.Reverse(parts);

			foreach (string part in parts) {
				var tok = part.Trim();
				var match = Regex.Match(tok, @"\d+");

				if (match.Success) {
					altura = int.Parse(tok);
				}
				else if (tok != "") {
					nombres.Add(tok);
				}
			}
			nombres.Reverse();

			return (altura != -1) ?
                this.GetAlturas(String.Join(" ", nombres), altura) :
                this.GetAlturas(String.Join(" ", nombres));
        }

        public IQueryable<CoreSQL.Entities.AlturasCalle> GetAlturas(string nombre, int numero = -1)
        {
            var data = this._db.GetAlturasByRango(numero, nombre).ToList();
            if (data.Count() == 0 && numero != -1) { 
				data = this._db.GetAlturasByProximidad(numero - 100, numero + 100, nombre).ToList();
            }
            return data.AsQueryable();
        }

        public IQueryable<Altura> GetAlturas(Calle calle)
        {
            return from a in this._db.zonas_altura where a.calle_id == calle.id orderby a.desde select a;
        }

		public IQueryable<Calle> GetCalles()
        {
            return this._db.zonas_calle.OrderBy( c => c.nombre);
        }

        public Altura GetAltura(int calleId, int alturaId) {
            return this._db.zonas_altura.First(a => a.calle_id == calleId && a.id == alturaId);
        }
        public Altura GetAltura(Calle calle, int alturaId)
        {
            return this.GetAltura(calle.id, alturaId);
        }
	}
}

