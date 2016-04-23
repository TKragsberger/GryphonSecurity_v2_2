using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.Domain.Entity
{
    public class Customer
    {
        String customerName;
        long customerNumber;
        String streeAndHouseNumber;
        int zipCode;
        String city;
        long phonenumber;

        public Customer(String customerName, long customerNumber, String streetAndHouseNumber, int zipCode, String city, long phonenumber)
        {
            this.customerName = customerName;
            this.customerNumber = customerNumber;
            this.streeAndHouseNumber = streetAndHouseNumber;
            this.zipCode = zipCode;
            this.city = city;
            this.phonenumber = phonenumber;
        }

        public String CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public long CustomerNumber
        {
            get { return customerNumber; }
            set { customerNumber = value; }
        }

        public String StreetAndHouseNumber
        {
            get { return streeAndHouseNumber; }
            set { streeAndHouseNumber = value; }
        }

        public int ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        public String City
        {
            get { return city; }
            set { city = value; }
        }

        public long Phonenumber
        {
            get { return phonenumber; }
            set { phonenumber = value; }
        }

    }
}
