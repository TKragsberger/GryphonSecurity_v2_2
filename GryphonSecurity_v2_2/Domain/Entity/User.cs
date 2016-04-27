using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.Domain.Entity
{
    public class User
    {
        private long id;
        private String firstname;
        private String lastname;

        public User(long id, String firstname, String lastname)
        {
            this.id = id;
            this.firstname = firstname;
            this.lastname = lastname;

        }

        public long Id
        {
            get { return id; }
        }

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public String toString()
        {
            return id + firstname + lastname;
        }
    }
}
