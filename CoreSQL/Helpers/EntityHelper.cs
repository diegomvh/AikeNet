using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Configuration;

namespace CoreSQL.Helpers
{
	public static class EntityHelper<T> where T : ObjectContext
	{
		public static T CreateInstance()
		{
			// get the connection string from config file
			string connectionString = ConfigurationManager.ConnectionStrings[typeof(T).Name].ConnectionString;
			
			// parse the connection string
			var csBuilder = new EntityConnectionStringBuilder(connectionString);
			
			// replace * by the full name of the containing assembly
			csBuilder.Metadata = csBuilder.Metadata.Replace(
				"res://*/",
				string.Format("res://{0}/", typeof(T).Assembly.FullName));
			
			// return the object
			return Activator.CreateInstance(typeof(T), csBuilder.ToString()) as T;
		}
	}
}

