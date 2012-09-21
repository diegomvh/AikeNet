using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Core.Models;
using Core.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Core.Services
{
	public class CalleService
	{

		private readonly MongoHelper<Calle> _calles;

		public CalleService ()
		{
			this._calles = new MongoHelper<Calle>("calles");
		}

        public Calle Create()
        {
			var calle = new Calle();
            calle.zonas = new List<Zona>();
			calle.id = ObjectId.GenerateNewId();
			return calle;
        }

		public void Save(Calle calle)
		{
			this._calles.Collection.Save(calle);
		}

        public void Delete(string sid)
        {
            var id = new ObjectId(sid);
            this._calles.Collection.Remove(Query.EQ("_id", id));
        }


        public Calle GetCalle(string sid)
        {
            var id = new ObjectId(sid);
            return this._calles.Collection.FindOne(Query.EQ("_id", id));
        }


        public long TotalCalles() {
            return this._calles.Collection.Count();
        }

		public IEnumerable<Calle> BuscarCalles(string consulta)
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

			var cc = (altura != -1) ? 
				this.GetCalles(String.Join(" ", nombres), altura) :
				this.GetCalles(String.Join(" ", nombres));

			if (altura != -1) {
				var calles = new List<Core.Models.Calle>();
				foreach (Core.Models.Calle calle in cc) {
					var zonas = new List<Core.Models.Zona>();
					foreach (Core.Models.Zona zona in calle.zonas)
						if (zona.desde <= altura && 
						    zona.hasta >= altura && 
						    Convert.ToBoolean(zona.vereda & ((altura % 2 == 0) ? 1 : 2)))
							zonas.Add(zona);
					calle.zonas = zonas;
					calles.Add(calle);
				}
				return calles;
			}else {
				return cc;
			}
        }

		public IEnumerable<Calle> GetCalles(string nombre, int altura = 0)
        {
			nombre = Core.Tools.Text.misspellRegexpSafeString(nombre);
			var query = Query.Matches("nombre", BsonRegularExpression.Create(nombre, "i"));
			if (altura != 0)
				query = Query.And(query,
                    Query.Or(
                        Query.And(
					        Query.LTE("zonas.desde", altura),
					        Query.GTE("zonas.hasta", altura)
                        ),
                        Query.And(
                            Query.EQ("zonas.desde", 0),
                            Query.EQ("zonas.hasta", 0)
                        )
                    )
				);
                
			return this._calles.Collection.Find(query);
        }

		public IEnumerable<Calle> GetCalles()
        {
			return this._calles.Collection.FindAll().SetSortOrder(SortBy.Ascending("zonas.desde"));
        }

		public IEnumerable<Calle> GetCalles(int skip, int limit)
		{
			return this._calles.Collection.FindAll().SetFields(Fields.Exclude("zonas")).SetSkip(skip).SetLimit(limit);
		}

	}
}

