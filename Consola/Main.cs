using System;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;
using CoreMongo;

namespace Consola
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            MainClass.TestCoreSQL();
            System.Console.Read();
		}

        public static void TestCoreMongo() {
            var calleService = new CoreMongo.Services.CalleService();
            foreach (CoreMongo.Models.Calle calle in calleService.BuscarCalles("martín"))
            {
                //System.Console.WriteLine(calle.nombre);
                foreach (CoreMongo.Models.Zona zona in calle.zonas)
                {
                    //System.Console.WriteLine(zona);
                }
            }
            foreach (CoreMongo.Models.Calle calle in calleService.GetCalles(10, 4))
            {
                System.Console.WriteLine(calle.nombre);
            }
            System.Console.WriteLine(CoreMongo.Tools.Text.misspellRegexpSafeString("martín"));
        }

        public static void TestCoreSQL()
        {
            var db = new CoreSQL.Entities.AikeEntities();
            IQueryable<CoreSQL.Entities.Localidad> localidades = from l in db.zonas_localidad select l;
            foreach (var localidad in localidades)
                System.Console.WriteLine(localidad.nombre);

        }
	}
}
