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
        DummyDB connection = new DummyDB();
        LocalStorage localStorage = new LocalStorage();

        public Boolean createUser(User user)
        {
            return localStorage.createUser(user);
        }

        public User getUser(long id)
        {
            return connection.getUser(id);
        }

        public User getLocalStorageUser()
        {
            return localStorage.getUser();
        }

        public Customer getCustomer(long id)
        {
            return connection.getCustomer(id);
        }

        public Boolean createAlarmReport(AlarmReport alarmReport)
        {
            return connection.createAlarmReport(alarmReport);
        }

        public Boolean createAlarmReports(List<AlarmReport> alarmReports)
        {
            return connection.createAlarmReports(alarmReports);
        }

        public Boolean createTempLocalStorageAlarmReport(AlarmReport alarmReport)
        {
            return localStorage.createTempAlarmReport(alarmReport);
        }

        public List<String> getAdress(String id)
        {
            return connection.getAddress(id);
        }

        public Boolean createNFC(NFC nfc)
        {
            return connection.createNFC(nfc);
        }

        public Boolean createNFCs(List<NFC> nfcs)
        {
            return connection.createNFCs(nfcs);
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

    }
}