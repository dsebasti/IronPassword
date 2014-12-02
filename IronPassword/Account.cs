using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPassword
{
    class Account
    {
        public String Resource { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }

        public DateTime CreationTime { get; private set; }

        public Account(String resource, String username, String password)
        {
            this.Resource = resource;
            this.Username = username;
            this.Password = password;
            this.CreationTime = DateTime.Now;
        }

        private Account(String resource, String username, String password, DateTime creationTime)
        {
            this.Resource = resource;
            this.Username = username;
            this.Password = password;
            this.CreationTime = creationTime;
        }

        public static Account Deserialize(String data) {
            String[] fields = data.Split('\n');
            return new Account(fields[0], fields[1], fields[2], Convert.ToDateTime(fields[3]));
        }

        public String Serialize()
        {
            return Resource + "\n" + Username + "\n" + Password + "\n" + Convert.ToString(CreationTime);
        }
    }
}
