using GryphonSecurity_v2_2.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.DataSource
{
    public class DBFacade
    {
        //DummyDB connection = new DummyDB();
        Mapper connection = new Mapper();
        LocalStorage localStorage = new LocalStorage();

        public Boolean createUser(User user)
        {
            return localStorage.createUser(user);
        }

        public async Task<User> getUser(long id)
        {
            return await connection.getEmployee(id);
        }

        public User getLocalStorageUser()
        {
            return localStorage.getUser();
        }

        public async Task<Customer> getCustomer(long id)
        {
            return await connection.getCustomer(id);
        }

        public async Task<Boolean> createAlarmReport(AlarmReport alarmReport)
        {
            return await connection.createAlarmReport(alarmReport);
        }

        public async Task<Boolean> createAlarmReports(List<AlarmReport> alarmReports)
        {
            return await connection.createAlarmReports(alarmReports);
        }

        public Boolean createTempLocalStorageAlarmReport(AlarmReport alarmReport)
        {
            return localStorage.createTempAlarmReport(alarmReport);
        }

        public async Task<Address> getAddress(String id)
        {
            return await connection.getAddress(id);
        }

        public async Task<Boolean> createNFC(NFC nfc)
        {
            return await connection.createNFC(nfc);
        }

        public async Task<Boolean> createNFCs(List<NFC> nfcs)
        {
            return await connection.createNFCs(nfcs);
        }

        public Boolean createLocalStorageNFCs(double presentLatitude, double presentLongitude, String tagAddress)
        {
            return localStorage.createNFC(presentLatitude, presentLongitude, tagAddress);
        }

        public Boolean createLocalStorageAlarmReport(AlarmReport alarmReport)
        {
            return localStorage.createAlarmReport(alarmReport);
        }

        public List<List<String>> getLocalStorageNFCs()
        {
            return localStorage.getNFCs();
        }

        public List<AlarmReport> getLocalStorageAlarmReports()
        {
            return localStorage.getAlarmReports();
        }

        public List<AlarmReport> getLocalStorageTempAlarmReports()
        {
            return localStorage.getTempAlarmReports();
        }

        public AlarmReport getLocalTempAlarmReport(long id)
        {
            return localStorage.getTempAlarmReport(id);
        }

        public Boolean removeLocalStorageNFCs()
        {
            return localStorage.removeNFCs();
        }

        public Boolean removeLocalStorageAlarmReports()
        {
            return localStorage.removeAlarmReports();
        }

        public int getLocalStorageNumberOfNFCs()
        {
            return localStorage.currentNumberOfNFCs();
        }

        public int getLocalStorageNumberOfAlarmReports()
        {
            return localStorage.currentNumberOfAlarmReports();
        }

        public Boolean removeLocalStorageTempSelectedAlarmReport(long id)
        {
            return localStorage.removeTempAlarmReport(id);
        }

        public async Task<Boolean> createCustomer(Customer customer)
        {
            
            return await connection.createCustomer(customer);
        }

        public Boolean createLocalStorageCustomer(Customer customer)
        {
            return localStorage.createCustomer(customer);
        }

        public List<Customer> getLocalStorageCustomers()
        {
            return localStorage.getCustomers();
        }

        public Boolean removeLocalStorageCustomers()
        {
            return localStorage.removeCustomers();
        }

        public async Task<Boolean> createCustomers(List<Customer> customers)
        {
            return await connection.createCustomers(customers);
        }

    }
}