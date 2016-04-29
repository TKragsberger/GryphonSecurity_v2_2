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
        

        public async Task<User> getEmployee(long id)
        {
            try
            {
            using (HttpClient client = new HttpClient())
            {
                
                String json = JsonConvert.SerializeObject(id);

                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/getEmployee.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getEmployee.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);

                resultWebservice.EnsureSuccessStatusCode();
 
                User user = JsonConvert.DeserializeObject<User>(await resultWebservice.Content.ReadAsStringAsync());     
                    return user;
            }
        }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in getEmployee");
                return null;
            }
        }

        public async Task<Customer> getCustomer(long id)
        {
            try
            {   
            using (HttpClient client = new HttpClient())
            {

                String json = JsonConvert.SerializeObject(id);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/getCustomer.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getCustomer.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Customer customer = JsonConvert.DeserializeObject<Customer>(await resultWebservice.Content.ReadAsStringAsync());
                Debug.WriteLine("GetCustomer");
                return customer;
            }
        }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in getCustomer");
                return null;
            }
        }

        public async Task<Boolean> createAlarmReport(AlarmReport alarmReport)
        {
            try
            { 
            using (HttpClient client = new HttpClient())
            {
                Debug.WriteLine("createAlarmRapport");
                String json = JsonConvert.SerializeObject(alarmReport);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/createAlarmReport.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createAlarmReport.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                    Debug.WriteLine("her createalarm: " + resultWebservice);
                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                return result;
            }
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in createAlarmReport");
                return false;
            }
            
        }

        public async Task<Boolean> createAlarmReports(List<AlarmReport> alarmReports)
        {
            try
            {
            using (HttpClient client = new HttpClient())
            {

                String json = JsonConvert.SerializeObject(alarmReports);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/createAlarmReports.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createAlarmReports.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);

                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                return result;
            }
        }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in createAlarmReports");
                return false;
            }
}


        public async Task<Address> getAddress(String id)
        {
            try
            {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(id);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/getAddress.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/getAddress.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Address result = JsonConvert.DeserializeObject<Address>(await resultWebservice.Content.ReadAsStringAsync());
                return result;

                }
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<Boolean> createNFC(NFC nfc)
        {
            Debug.WriteLine("NFC "+ nfc.Time);
            try
            {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(nfc);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/createNFC.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createNFC.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                Debug.WriteLine("Result from createNFC " + result);
                return result;
            }
            } catch(Exception ex)
            {
                Debug.WriteLine("catch in createNFC");
                return false;
            }
        }

        public async Task<Boolean> createNFCs(List<NFC> nfcs)
        {
            try
            {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(nfcs);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/createNFCs.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createNFCs.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                Debug.WriteLine("Result from createNFCs "+result);
                return result;
            }
        }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in createNFCs");
                return false;
            }
        }

        public async Task<Boolean> createCustomer(Customer customer)
        {
            try
            {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(customer);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/createCustomer.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createCustomer.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Debug.WriteLine("her: " + resultWebservice);
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                Debug.WriteLine("createCustomer + result " + result);
                return result;
            }
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in createCustomer");
                return false;
            }

        }

        public async Task<Boolean> createCustomers(List<Customer> customers)
        {
            try
            {
            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(customers);
                    var resultWebservice = await client.PostAsync("http://gryphon.dk/GryphonSecurityRestFullWebservice/webServices/createCustomers.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                    //var resultWebservice = await client.PostAsync("http://kragsberger.dk/GryphonSecurityRestFullWebservice/webServices/createCustomers.php/", new StringContent(json, Encoding.UTF8, "application/json"));
                //var resultWebservice = await client.GetAsync("http://kragsberger.dk/rest/" + name);
                resultWebservice.EnsureSuccessStatusCode();
                Boolean result = Convert.ToBoolean(JsonConvert.DeserializeObject<String>(await resultWebservice.Content.ReadAsStringAsync()));
                Debug.WriteLine("createCustomers + result " + result);
                return result;
            }
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine("catch in createCustomers");
                return false;
            }

        }


    }
}
