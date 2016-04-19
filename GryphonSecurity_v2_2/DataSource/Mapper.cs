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

        public async Task<Boolean> createAlarmReport(AlarmReport alarmReport)
        {
            using (HttpClient client = new HttpClient())
            {

                String json = JsonConvert.SerializeObject(alarmReport);
                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createAlarmReport.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);

                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                return result;
            }
            
        }

        public async Task<Boolean> createAlarmReports(List<AlarmReport> alarmReports)
        {
            using (HttpClient client = new HttpClient())
            {

                String json = JsonConvert.SerializeObject(alarmReports);
                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createAlarmReports.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);

                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                return result;
            }
        }


        public async Task<Address> getAddress(String id)
        {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(id);
                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getAddress.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Address result = JsonConvert.DeserializeObject<Address>(await resultWebservice.Content.ReadAsStringAsync());
                return result;
            }
        }

        public async Task<Boolean> createNFC(NFC nfc)
        {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(nfc);
                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getAddress.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                return result;
            }
        }

        public async Task<Boolean> createNFCs(List<NFC> nfcs)
        {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(nfcs);
                var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getAddress.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                return result;
            }
        }

        public async Task<Boolean> createCustomer(Customer customer)
        {

            return true;
        }

        public async Task<Boolean> createCustomers(List<Customer> customers)
        {
            return true;
        }


    }
}
