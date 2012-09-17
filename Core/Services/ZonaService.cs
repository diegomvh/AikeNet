using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Models;
using Core.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Core.Services
{
    public class ZonaService
    {
        private readonly MongoHelper<Calle> _calles;

        public ZonaService()
        {
            this._calles = new MongoHelper<Calle>("calles");
        }

        public Zona Create(Calle calle) { 
            var zona = new Zona();
            zona.id = ObjectId.GenerateNewId();
            calle.zonas.Add(zona);
            return zona;
        }

        public void AddZona(string calleId, Zona zona)
        {
            var cId = new ObjectId(calleId);
            this._calles.Collection.Update(Query.EQ("_id", calleId),
                Update.PushWrapped("zonas", zona));
        }

        public void RemoveZona(string calleId, string zonaId)
        {
            var cId = new ObjectId(calleId);
            var zId = new ObjectId(zonaId);
            this._calles.Collection.Update(Query.EQ("_id", cId),
                Update.Pull("zonas", Query.EQ("_id", zId)));
        }

        public IEnumerable<Zona> GetZonas(string calleId)
        {
            var cId = new ObjectId(calleId);
            var calle = this._calles.Collection.FindOne(Query.EQ("_id", cId));
            //return calle.zonas.OrderByDescending(c => c.desde);
            return calle.zonas;
        }

        public Zona GetZona(string calleId, string zonaId)
        {
            var zId = new ObjectId(zonaId);
            var zonas = this.GetZonas(calleId);
            foreach (var zona in zonas) {
                if (zona.id == zId)
                    return zona;
            }
            return null;
        }

        public int GetTotalZonas(ObjectId calleId)
        {
            var post = this._calles.Collection.FindOne(Query.EQ("_id", calleId));
            return post.zonas.Count();
        }
    }
}
