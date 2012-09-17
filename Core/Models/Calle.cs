using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Models
{
    public class CalleSegmentada : Exception { }

	[BsonIgnoreExtraElements]
	public class Calle {
		[ScaffoldColumn(false)]
        [BsonId]
	    public MongoDB.Bson.ObjectId id { get; set; }

		[Required]
	    public string nombre { get; set; }

		[ScaffoldColumn(false)]
	    public string[] nombres { get; set; }

	 	[ScaffoldColumn(false)]
	    public IList<Zona> zonas { get; set; }

        [ScaffoldColumn(false)]
        public bool Segmentada {
            get {
                if (this.zonas.Count > 1)
                    return true;
                return false;
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
                if (this.Segmentada)
                    throw new CalleSegmentada();
                return this.zonas[0].zona;
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
	}
}
