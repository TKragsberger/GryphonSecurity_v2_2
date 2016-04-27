using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.Domain.Entity
{
    public class NFC
    {
        private Boolean rangeCheck;
        private String addressId;
        private DateTime time;
        private long employeeId;

        public NFC(Boolean rangeCheck, String addressId, DateTime time, long employeeId)
        {
            this.rangeCheck = rangeCheck;
            this.addressId = addressId;
            this.time = time;
            this.employeeId = employeeId;
        }

        public Boolean RangeCheck
        {
            get { return rangeCheck; }
            set { rangeCheck = value; }
        }

        public string AddressId
        {
            get { return addressId; }
            set { addressId = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public long EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }
    }
}
