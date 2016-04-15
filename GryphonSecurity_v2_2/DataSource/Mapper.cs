using GryphonSecurity_v2_2.Domain.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.DataSource
{
    public class Mapper
    {
        

        public async Task<User> getUser(long id)
        {
            using (HttpClient client = new HttpClient())
            {
                
                String json = JsonConvert.SerializeObject(id);

                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getUser.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);

                resultWebservice.EnsureSuccessStatusCode();

                User user = JsonConvert.DeserializeObject<User>(await resultWebservice.Content.ReadAsStringAsync());
                return user;
            }
        }

        public async Task<Customer> getCustomer(long id)
        {
            using (HttpClient client = new HttpClient())
            {

                String json = JsonConvert.SerializeObject(id);
                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getCustomer.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);

                resultWebservice.EnsureSuccessStatusCode();
                Customer customer = JsonConvert.DeserializeObject<Customer>(await resultWebservice.Content.ReadAsStringAsync());
                return customer;
            }
        }

        public Boolean createAlarmReport(AlarmReport alarmReport)
        {
            return false;
        }

        public Boolean createAlarmReports(List<AlarmReport> alarmReports)
        {
            return false;
        }


        public List<String> getAddress(String id)
        {
            return null;
        }

        public Boolean createNFC(NFC nfc)
        {
            return false;
        }

        public Boolean createNFCs(List<NFC> nfcs)
        {
            return false;
        }

        
    }
}
