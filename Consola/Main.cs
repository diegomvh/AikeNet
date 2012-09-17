using System;
using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;
using Core;

namespace Consola
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            var calleService = new Core.Services.CalleService();
            foreach (Core.Models.Calle calle in calleService.BuscarCalles("martín"))
			{
                //System.Console.WriteLine(calle.nombre);
				foreach (Core.Models.Zona zona in calle.zonas) {
                    //System.Console.WriteLine(zona);
				}
			}
            foreach (Core.Models.Calle calle in calleService.GetCalles(10, 4))
            {
                System.Console.WriteLine(calle.nombre);
            }
            System.Console.WriteLine(Core.Tools.Text.misspellRegexpSafeString("martín"));
            System.Console.Read();
		}
	}
}
