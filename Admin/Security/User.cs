using System;
using System.Security.Principal;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Admin.Security
{
	public class User : GenericPrincipal{

		public string UserName { get; set;}
		public string FirstName { get; set;}
		public string LastName { get; set;}
		public string Email { get; set;}
		public List<string> Groups { get; set;}
		public List<string> Organizations { get; set; }

		public User(string userName, string[] groups) : base(new GenericIdentity(userName, "LdapAuthentication"), groups)
		{
			this.UserName = userName;
		}

		public static User UserFactory(string userName, string jsonData)
		{
			var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
			var groups = new List<string>(((JArray)userData["Groups"]).Values<string>());
			var user = new User(userName, groups.ToArray());
			if (userData.ContainsKey("FirstName") && userData["FirstName"] != null)
                user.FirstName = userData["FirstName"].ToString();
			if (userData.ContainsKey("LastName") && userData["LastName"] != null)
                user.LastName = userData["LastName"].ToString();
			if (userData.ContainsKey("Email") && userData["Email"] != null)
                user.Email = userData["Email"].ToString();
            user.Groups = groups;
			if (userData.ContainsKey("Organizations") && userData["Organizations"] != null)
                user.Organizations = new List<string>(((JArray)userData["Organizations"]).Values<string>());
			return user;
		}

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
		
		public string ToJson() {
			return JsonConvert.SerializeObject(new
			{
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
				Groups = this.Groups,
				Organizations = this.Organizations
			});
		}
	}
}

