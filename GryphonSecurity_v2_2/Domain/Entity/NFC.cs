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
        private String tagAddress;
        private DateTime time;
        private User user;

        public NFC(Boolean rangeCheck, String tagAddress, DateTime time, User user)
        {
            this.rangeCheck = rangeCheck;
            this.tagAddress = tagAddress;
            this.time = time;
            this.user = user;
        }

        public Boolean RangeCheck
        {
            get
            {
                return rangeCheck;
            }

            set
            {
                rangeCheck = value;
            }
        }

        public string TagAddress
        {
            get
            {
                return tagAddress;
            }

            set
            {
                tagAddress = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public User User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }
    }
}
