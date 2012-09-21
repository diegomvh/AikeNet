using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;

namespace Admin.Tools
{
    public class User {
        
        public string UserName { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string Email { get; set;}
        public List<string> Groups { get; set;}
        public List<string> Organizations { get; set; }

        public string GetFullName() {
            var s = new System.Text.StringBuilder();
            if (this.FirstName != "") {
                s.Append(this.FirstName);
                s.Append(" ");
                if (this.LastName != "")
                    s.Append(this.LastName);
            }
            return s.ToString();
        }
        
        public string SerializeGroups() {
            return string.Join("|", this.Groups);
        }
    }
    
    public class UserRepository
    {
		private string _server = ConfigurationManager.ConnectionStrings["LDAPJuschubut"].ConnectionString;
		private string _proxyUser = ConfigurationManager.AppSettings["LDAPProxyUser"];
		private string _proxyPassword = ConfigurationManager.AppSettings["LDAPProxyPassword"];

        public User GetUser(string userName) {
            var root = new DirectoryEntry(this._server, this._proxyUser, this._proxyPassword);
            var search = new DirectorySearcher(root);

            search.SearchScope = SearchScope.Subtree;
            search.Filter = "(sAMAccountName=" + userName.Substring(userName.IndexOf("\\") + 1) + ")";
            var result = search.FindAll();

            return this._GetUser(result[0].Path);
        }

		public User GetUserByFullName(string fullName) {
			var root = new DirectoryEntry(this._server, this._proxyUser, this._proxyPassword);
			var search = new DirectorySearcher(root);
			
			search.SearchScope = SearchScope.Subtree;
			search.Filter = "(displayName=" + fullName + ")";
			var results = search.FindAll();

			if (results.Count > 0)
				return this._GetUser(results[0].Path);
			return null;

		}
		public IQueryable<User> GetMembers(string groupPath)
		{
			var root = new DirectoryEntry (this._server, this._proxyUser, this._proxyPassword);
			var search = new DirectorySearcher (root);
			var members = new List<User> ();
			search.SearchScope = SearchScope.Subtree;
			search.Filter = "(memberOf=" + groupPath + ")";
			var results = search.FindAll ();

			foreach (SearchResult result in results) {
				members.Add(this._GetUser(result.Path));
			}
			root.Close();
			return members.AsQueryable();
		}

        public User Authenticate(string userName, string password) {
            var root = new DirectoryEntry(this._server, userName, password);
            var search = new DirectorySearcher(root);

			search.SearchScope = SearchScope.Subtree;
            search.Filter = "(sAMAccountName=" + userName + ")";

            var result = search.FindAll();
            if (result.Count > 0) {
                return this._GetUser(result[0].Path);
            }
            return null;
        }

        private User _GetUser(string userPath) {
            var entry = new DirectoryEntry(userPath, this._proxyUser, this._proxyPassword);
            var user = new User();

            user.UserName = entry.Properties["sAMAccountName"].Value.ToString();
            foreach (var propName in entry.Properties.PropertyNames)
            {
                var propValue = entry.Properties[propName.ToString()].Value.ToString();
            }
            //"displayName"
            if (entry.Properties.Contains("givenName"))
                user.FirstName = entry.Properties["givenName"].Value.ToString();
            if (entry.Properties.Contains("sn"))
                user.LastName = entry.Properties["sn"].Value.ToString();
            if (entry.Properties.Contains("mail"))
                user.Email = entry.Properties["mail"].Value.ToString();
            
            user.Groups = new List<string>();
            foreach (var gourp in entry.Properties["memberOf"])
                user.Groups.Add(this._GetGroup(gourp.ToString()));
            user.Organizations = this._GetOrganizations(entry.Properties["distinguishedName"].Value.ToString());
            entry.Close();
            return user;
        }

        private string _GetGroup(string path) {
            var value = "";
            var index1 = path.IndexOf("=", 1);
            var index2 = path.IndexOf(",", 1);
            if (!(index1 == -1)) {
                value = path.Substring((index1 + 1), (index2 - index1) - 1);
            }
            return value;
        }

        private List<string> _GetOrganizations(string path)
        {
            var values = new List<string>();
            
            foreach (var token in path.Split(','))
                if (token.IndexOf("OU") == 0)
                    values.Add(token.Substring(3, token.Length - 3));
            return values;
        }

    }
}
