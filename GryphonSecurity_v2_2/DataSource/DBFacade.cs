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
        //DummyDB is used for localDatabase testing.
        //DummyDB connection = new DummyDB();
        Mapper connection = new Mapper();
        LocalStorage localStorage = new LocalStorage();

        public Boolean createUser(User user)
        {
            return localStorage.saveUser(user);
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
            return await connection.sendAlarmReport(alarmReport);
        }

        public async Task<Boolean> createAlarmReports(List<AlarmReport> alarmReports)
        {
            return await connection.sendAlarmReports(alarmReports);
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
            return await connection.sendNFC(nfc);
        }

        public async Task<Boolean> createNFCs(List<NFC> nfcs)
        {
            return await connection.sendNFCs(nfcs);
        }

        public Boolean createLocalStorageNFCs(double presentLatitude, double presentLongitude, String tagAddress)
        {
            return localStorage.saveNFC(presentLatitude, presentLongitude, tagAddress);
        }

        public Boolean createLocalStorageAlarmReport(AlarmReport alarmReport)
        {
            return localStorage.saveAlarmReport(alarmReport);
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

        public int getLocalStorageNumberOfCustomers()
        {
            return localStorage.currentNumberOfCustomers();
        }

        public Boolean removeLocalStorageTempSelectedAlarmReport(long id)
        {
            return localStorage.removeTempAlarmReport(id);
        }

        public async Task<Boolean> createCustomer(Customer customer)
        {
            
            return await connection.sendCustomer(customer);
        }

        public Boolean createLocalStorageCustomer(Customer customer)
        {
            return localStorage.saveCustomer(customer);
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
            return await connection.sendCustomers(customers);
        }

    }
}