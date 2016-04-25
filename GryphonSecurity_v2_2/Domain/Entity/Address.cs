using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.Domain.Entity
{
    public class Address
    {
        private String addressId;
        private String addressName;
        private double latitude;
        private double longtitude;

        public Address(String addressId, String addressName, double latitude, double longtitude)
        {
            this.addressId = addressId;
            this.addressName = addressName;
            this.latitude = latitude;
            this.longtitude = longtitude;
        }

        public String AddressID
        {
            get { return addressId; }
            set { addressId = value; }
        }

        public String AddressName
        {
            get { return addressName; }
            set { addressName = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }
    }
}
