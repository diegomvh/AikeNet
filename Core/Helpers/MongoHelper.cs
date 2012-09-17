using System.Configuration;
using MongoDB.Driver;

namespace Core.Helpers
{
    public class MongoHelper<T> where T : class
    {
        public MongoCollection<T> Collection { get; private set; }

        public MongoHelper(string name)
        {
            var con = new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);

            var server = MongoServer.Create(con);
            var db = server.GetDatabase(con.DatabaseName);
            Collection = db.GetCollection<T>(name);
        }

		public MongoHelper() : this(typeof(T).Name.ToLower())
		{
        }
    }
}
