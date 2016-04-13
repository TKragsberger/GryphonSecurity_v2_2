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
        private String address;
        private long phonenumber;
        private String username;
        private String password;

        public User(long id, String firstname, String lastname, String address, long phonenumber, String username, String password)
        {
            this.id = id;
            this.firstname = firstname;
            this.lastname = lastname;
            this.address = address;
            this.phonenumber = phonenumber;
            this.username = username;
            this.password = password;
        }

        public long Id
        {
            get
            {
                return id;
            }
        }

        public string Firstname
        {
            get
            {
                return firstname;
            }

            set
            {
                firstname = value;
            }
        }

        public string Lastname
        {
            get
            {
                return lastname;
            }

            set
            {
                lastname = value;
            }
        }

        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public long Phonenumber
        {
            get
            {
                return phonenumber;
            }

            set
            {
                phonenumber = value;
            }
        }

        public String Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public String Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        public String toString()
        {
            return id + firstname + lastname + address + phonenumber + username + password;
        }
    }
}
