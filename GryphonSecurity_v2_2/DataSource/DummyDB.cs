using GryphonSecurity_v2_2.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2.DataSource
{
    public class DummyDB
    {
        private static IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        private Boolean dummyDBStatus = false;

        private String KEY_DUMMY_FIRSTNAME = "DUMMY_FIRSTNAME";
        private String KEY_DUMMY_LASTNAME = "DUMMY_LASTNAME";

        private String KEY_DUMMY_ID_NFC = "DUMMY_ID_NFC";
        private String KEY_DUMMY_ID_ALARMREPORT = "DUMMY_ID_ALARMREPORT";

        private String KEY_DUMMY_CURRENTNUMBER_OF_ALARMREPORTS = "DUMMY_CURRENTNUMBER_OF_ALARMREPORTS";
        private String KEY_DUMMY_CURRENTNUMBER_OF_NFCS = "DUMMY_CURRENTNUMBER_OF_NFCS";

        private String KEY_DUMMY_REPORT_CUSTOMERNAME = "DUMMY_REPORT_CUSTOMERNAME";
        private String KEY_DUMMY_REPORT_CUSTOMERNUMBER = "DUMMY_REPORT_CUSTOMERNUMBER";
        private String KEY_DUMMY_REPORT_STREET_AND_HOUSENUMBER = "DUMMY_REPORT_STREET_AND_HOUSENUMBER";
        private String KEY_DUMMY_REPORT_ZIPCODE = "DUMMY_REPORT_ZIPCODE";
        private String KEY_DUMMY_REPORT_CITY = "DUMMY_REPORT_CITY";
        private String KEY_DUMMY_REPORT_PHONENUMBER = "DUMMY_REPORT_PHONENUMBER";
        private String KEY_DUMMY_REPORT_DATE = "DUMMY_REPORT_DATE";
        private String KEY_DUMMY_REPORT_TIME = "DUMMY_REPORT_TIME";
        private String KEY_DUMMY_REPORT_ZONE = "DUMMY_REPORT_ZONE";
        private String KEY_DUMMY_REPORT_BURGLARYVANDALISM = "DUMMY_REPORT_BURGLARYVANDALISM";
        private String KEY_DUMMY_REPORT_WINDOWDOORCLOSED = "DUMMY_REPORT_WINDOWDOORCLOSED";
        private String KEY_DUMMY_REPORT_APPREHENDEDPERSON = "DUMMY_REPORT_APPREHENDEDPERSON";
        private String KEY_DUMMY_REPORT_STAFFERROR = "DUMMY_REPORT_STAFFERROR";
        private String KEY_DUMMY_REPORT_NOTHINGTOREPORT = "DUMMY_REPORT_NOTHINGTOREPORT";
        private String KEY_DUMMY_REPORT_TECHNICALERROR = "DUMMY_REPORT_TECHNICALERROR";
        private String KEY_DUMMY_REPORT_UNKNOWNREASON = "DUMMY_REPORT_UNKNOWNREASON";
        private String KEY_DUMMY_REPORT_OTHER = "DUMMY_REPORT_OTHER";
        private String KEY_DUMMY_REPORT_CANCELDURINGEMERGENCY = "DUMMY_REPORT_CANCELDURINGEMERGENCY";
        private String KEY_DUMMY_REPORT_COVERMADE = "DUMMY_REPORT_COVERMADE";
        private String KEY_DUMMY_REPORT_REMARK = "DUMMY_REPORT_REMARK";
        private String KEY_DUMMY_REPORT_NAME = "DUMMY_REPORT_NAME";
        private String KEY_DUMMY_REPORT_INSTALLER = "DUMMY_REPORT_INSTALLER";
        private String KEY_DUMMY_REPORT_CONTROLCENTER = "DUMMY_REPORT_CONTROLCENTER";
        private String KEY_DUMMY_REPORT_GUARDRADIOEDDATE = "DUMMY_REPORT_GUARDRADIOEDDATE";
        private String KEY_DUMMY_REPORT_GUARDRADIOEDFROM = "DUMMY_REPORT_GUARDRADIOEDFROM";
        private String KEY_DUMMY_REPORT_GUARDRADIOEDTO = "DUMMY_REPORT_GUARDRADIOEDTO";
        private String KEY_DUMMY_REPORT_ARRIVEDAT = "DUMMY_REPORT_ARRIVEDAT";
        private String KEY_DUMMY_REPORT_DONE = "DUMMY_REPORT_DONE";
        private String KEY_DUMMY_REPORT_USER_ID = "DUMMY_REPORT_USER_ID";

        private String KEY_DUMMY_NFC_USER_ID = "DUMMY_USER_ID";
        private String KEY_DUMMY_NFC_RANGECHECK = "DUMMY_RANGECHECK";
        private String KEY_DUMMY_NFC_TAGADDRESS = "DUMMY_TAGADDRESS";
        private String KEY_DUMMY_NFC_TIME = "DUMMY_TIME";

        private String KEY_DUMMY_ADDRESS_NAME = "DUMMY_ADDRESS_NAME";
        private String KEY_DUMMY_ADDRESS_LONGTITUDE = "DUMMY_LONGTITUDE";
        private String KEY_DUMMY_ADDRESS_LATITUDE = "DUMMY_LATITUDE";

        private String KEY_DUMMY_CUSTOMER_NAME = "DUMMY_CUSTOMER_NAME";
        private String KEY_DUMMY_CUSTOMER_NUMBER = "DUMMY_CUSTOMER_NUMBER";
        private String KEY_DUMMY_CUSTOMER_STREET_AND_HOUSE_NUMBER = "DUMMY_CUSTOMER_STREET_AND_HOUSE_NUMBER";
        private String KEY_DUMMY_CUSTOMER_ZIP_CODE = "DUMMY_CUSTOMER_ZIP_CODE";
        private String KEY_DUMMY_CUSTOMER_CITY = "DUMMY_CUSTOMER_CITY";
        private String KEY_DUMMY_CUSTOMER_PHONENUMBER = "DUMMY_CUSTOMER_PHONENUMBER";

        private List<String> address = new List<String>();

        public DummyDB()
        {
            createAddresses();
            createCustomers();
            createUser();
        }

        public void createUser()
        {
            try
            {
                if (!appSettings.Contains(1 + KEY_DUMMY_FIRSTNAME))
                {
                    appSettings.Add(1 + KEY_DUMMY_FIRSTNAME, "Thomas");
                    appSettings.Add(1 + KEY_DUMMY_LASTNAME, "Kragsberger");
                    appSettings.Add(2 + KEY_DUMMY_FIRSTNAME, "Jannik");
                    appSettings.Add(2 + KEY_DUMMY_LASTNAME, "Vangsgaard");
                    appSettings.Add(3 + KEY_DUMMY_FIRSTNAME, "Mike");
                    appSettings.Add(3 + KEY_DUMMY_LASTNAME, "Heerwagen");
                    appSettings.Save();
                }
            }
            catch (IsolatedStorageException)
            {
                Debug.WriteLine("Users did not get saved in dummyDB");
            }
        }

        public async Task<User> getUser(long id)
        {
            if (appSettings.Contains(id + KEY_DUMMY_FIRSTNAME))
            {
                String firstname = appSettings[id + KEY_DUMMY_FIRSTNAME] as String;
                String lastname = appSettings[id + KEY_DUMMY_LASTNAME] as String;
                return new User(id, firstname, lastname);
            }
            else
            {
                return null;
            }
        }

        public async Task<Boolean> createAlarmReports(List<AlarmReport> alarmReports)
        {
            Boolean check = false;

            foreach (AlarmReport alarmReport in alarmReports)
            {
                check = await createAlarmReport(alarmReport);
                if (!check)
                {
                    return check;
                }
            }
            return check;
        }

        public async Task<Boolean> createAlarmReport(AlarmReport alarmReport)
        {
            long id = getNextAlarmReportId();

            try
            {
                appSettings.Add(id + KEY_DUMMY_REPORT_CUSTOMERNAME, alarmReport.CustomerName);
                appSettings.Add(id + KEY_DUMMY_REPORT_CUSTOMERNUMBER, alarmReport.CustomerNumber);
                appSettings.Add(id + KEY_DUMMY_REPORT_STREET_AND_HOUSENUMBER, alarmReport.StreetAndHouseNumber);
                appSettings.Add(id + KEY_DUMMY_REPORT_ZIPCODE, alarmReport.ZipCode);
                appSettings.Add(id + KEY_DUMMY_REPORT_CITY, alarmReport.City);
                appSettings.Add(id + KEY_DUMMY_REPORT_PHONENUMBER, alarmReport.Phonenumber);
                appSettings.Add(id + KEY_DUMMY_REPORT_DATE, alarmReport.Date);
                appSettings.Add(id + KEY_DUMMY_REPORT_TIME, alarmReport.Time);
                appSettings.Add(id + KEY_DUMMY_REPORT_ZONE, alarmReport.Zone);
                appSettings.Add(id + KEY_DUMMY_REPORT_BURGLARYVANDALISM, alarmReport.BurglaryVandalism);
                appSettings.Add(id + KEY_DUMMY_REPORT_WINDOWDOORCLOSED, alarmReport.WindowDoorClosed);
                appSettings.Add(id + KEY_DUMMY_REPORT_APPREHENDEDPERSON, alarmReport.ApprehendedPerson);
                appSettings.Add(id + KEY_DUMMY_REPORT_STAFFERROR, alarmReport.StaffError);
                appSettings.Add(id + KEY_DUMMY_REPORT_NOTHINGTOREPORT, alarmReport.NothingToReport);
                appSettings.Add(id + KEY_DUMMY_REPORT_TECHNICALERROR, alarmReport.TechnicalError);
                appSettings.Add(id + KEY_DUMMY_REPORT_UNKNOWNREASON, alarmReport.UnknownReason);
                appSettings.Add(id + KEY_DUMMY_REPORT_OTHER, alarmReport.Other);
                appSettings.Add(id + KEY_DUMMY_REPORT_CANCELDURINGEMERGENCY, alarmReport.CancelDuringEmergency);
                appSettings.Add(id + KEY_DUMMY_REPORT_COVERMADE, alarmReport.CoverMade);
                appSettings.Add(id + KEY_DUMMY_REPORT_REMARK, alarmReport.CoverMade);
                appSettings.Add(id + KEY_DUMMY_REPORT_NAME, alarmReport.Name);
                appSettings.Add(id + KEY_DUMMY_REPORT_INSTALLER, alarmReport.Installer);
                appSettings.Add(id + KEY_DUMMY_REPORT_CONTROLCENTER, alarmReport.ControlCenter);
                appSettings.Add(id + KEY_DUMMY_REPORT_GUARDRADIOEDDATE, alarmReport.GuardRadioedDate);
                appSettings.Add(id + KEY_DUMMY_REPORT_GUARDRADIOEDFROM, alarmReport.GuardRadioedFrom);
                appSettings.Add(id + KEY_DUMMY_REPORT_GUARDRADIOEDTO, alarmReport.GuardRadioedTo);
                appSettings.Add(id + KEY_DUMMY_REPORT_ARRIVEDAT, alarmReport.ArrivedAt);
                appSettings.Add(id + KEY_DUMMY_REPORT_DONE, alarmReport.Done);
                appSettings.Add(id + KEY_DUMMY_REPORT_USER_ID, alarmReport.User.Id);
                appSettings.Save();
                dummyDBStatus = true;
            }
            catch
            {
                dummyDBStatus = false;
            }
            return dummyDBStatus;
        }

        public async Task<List<AlarmReport>> getAlarmReports()
        {
            List<AlarmReport> alarmReports = new List<AlarmReport>();
            int length = currentNumberOfAlarmReports();
            long id = 0;

            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    id = i + 1;
                    AlarmReport alarmReport = await getAlarmReport(id);
                    alarmReports.Add(alarmReport);
                }
            }

            return alarmReports;
        }

        public async Task<AlarmReport> getAlarmReport(long id)
        {
            if (appSettings.Contains(id + KEY_DUMMY_REPORT_CUSTOMERNAME))
            {
                String customerName = appSettings[id + KEY_DUMMY_REPORT_CUSTOMERNAME] as String;
                long customerNumber = Convert.ToInt64(appSettings[id + KEY_DUMMY_REPORT_CUSTOMERNUMBER] as String);
                String streetAndHouseNumber = appSettings[id + KEY_DUMMY_REPORT_STREET_AND_HOUSENUMBER] as String;
                int zipCode = Convert.ToInt32(appSettings[id + KEY_DUMMY_REPORT_ZIPCODE] as String);
                String city = appSettings[id + KEY_DUMMY_REPORT_CITY] as String;
                long phonenumber = Convert.ToInt64(appSettings[id + KEY_DUMMY_REPORT_PHONENUMBER] as String);
                DateTime date = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_DATE] as String, CultureInfo.InvariantCulture);
                DateTime time = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_TIME] as String, CultureInfo.InvariantCulture);
                String zone = appSettings[id + KEY_DUMMY_REPORT_ZONE] as String;
                Boolean burglaryVandalism = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_BURGLARYVANDALISM] as String);
                Boolean windowDoorClosed = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_WINDOWDOORCLOSED] as String);
                Boolean apprehendedPerson = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_APPREHENDEDPERSON] as String);
                Boolean staffError = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_STAFFERROR] as String);
                Boolean nothingToReport = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_NOTHINGTOREPORT] as String);
                Boolean technicalError = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_TECHNICALERROR] as String);
                Boolean unknownReason = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_UNKNOWNREASON] as String);
                Boolean other = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_OTHER] as String);
                Boolean cancelDuringEmergency = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_CANCELDURINGEMERGENCY] as String);
                Boolean coverMade = Convert.ToBoolean(appSettings[id + KEY_DUMMY_REPORT_COVERMADE] as String);
                String remark = appSettings[id + KEY_DUMMY_REPORT_REMARK] as String;
                String name = appSettings[id + KEY_DUMMY_REPORT_NAME] as String;
                String installer = appSettings[id + KEY_DUMMY_REPORT_INSTALLER] as String;
                String controlCenter = appSettings[id + KEY_DUMMY_REPORT_CONTROLCENTER] as String;
                DateTime guardRadioedDate = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_GUARDRADIOEDDATE] as String, CultureInfo.InvariantCulture);
                DateTime guardRadioedFrom = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_GUARDRADIOEDFROM] as String, CultureInfo.InvariantCulture);
                DateTime guardRadioedTo = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_GUARDRADIOEDTO] as String, CultureInfo.InvariantCulture);
                DateTime arrivedAt = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_ARRIVEDAT] as String, CultureInfo.InvariantCulture);
                DateTime done = DateTime.Parse(appSettings[id + KEY_DUMMY_REPORT_DONE] as String, CultureInfo.InvariantCulture);
                User user = await getUser(Convert.ToInt64(appSettings[id + KEY_DUMMY_REPORT_USER_ID] as String));
                return new AlarmReport(customerName, customerNumber, streetAndHouseNumber, zipCode, city, phonenumber, date, time, zone, burglaryVandalism,
                                        windowDoorClosed, apprehendedPerson, staffError, nothingToReport, technicalError, unknownReason, other, cancelDuringEmergency, coverMade,
                                        remark, name, installer, controlCenter, guardRadioedDate, guardRadioedFrom, guardRadioedTo, arrivedAt, done, user);
            }
            else
            {
                return null;
            }
        }

        public Boolean createAddresses()
        {

            try
            {
                if (!appSettings.Contains(1 + KEY_DUMMY_ADDRESS_NAME))
                {
                    appSettings.Add(1 + KEY_DUMMY_ADDRESS_NAME, "Lyngby st.");
                    appSettings.Add(1 + KEY_DUMMY_ADDRESS_LONGTITUDE, "12,505161499999986");
                    appSettings.Add(1 + KEY_DUMMY_ADDRESS_LATITUDE, "55,767944");
                    appSettings.Add(2 + KEY_DUMMY_ADDRESS_NAME, "København hovedbanegård");
                    appSettings.Add(2 + KEY_DUMMY_ADDRESS_LONGTITUDE, "12,566796599999975");
                    appSettings.Add(2 + KEY_DUMMY_ADDRESS_LATITUDE, "55,6713363");
                    appSettings.Add(3 + KEY_DUMMY_ADDRESS_NAME, "Farum st.");
                    appSettings.Add(3 + KEY_DUMMY_ADDRESS_LONGTITUDE, "12,373533899999984");
                    appSettings.Add(3 + KEY_DUMMY_ADDRESS_LATITUDE, "55,8120275");
                    appSettings.Add(4 + KEY_DUMMY_ADDRESS_NAME, "Kokkedal st.");
                    appSettings.Add(4 + KEY_DUMMY_ADDRESS_LONGTITUDE, "12,502057000000036");
                    appSettings.Add(4 + KEY_DUMMY_ADDRESS_LATITUDE, "55,90348789999999");
                    appSettings.Add(5 + KEY_DUMMY_ADDRESS_NAME, "Buddinge st.");
                    appSettings.Add(5 + KEY_DUMMY_ADDRESS_LONGTITUDE, "12,493978999999968");
                    appSettings.Add(5 + KEY_DUMMY_ADDRESS_LATITUDE, "55,7469736");
                    appSettings.Save();
                }
                return true;
            }
            catch (IsolatedStorageException)
            {
                Debug.WriteLine("Addresses did not get saved in dummyDB");
                return false;
            }

        }

        public void createCustomers()
        {
            try
            {
                if (!appSettings.Contains(1 + KEY_DUMMY_CUSTOMER_NAME))
                {
                    appSettings.Add(1 + KEY_DUMMY_CUSTOMER_NAME, "Thomas Kragsberger");
                    appSettings.Add(1 + KEY_DUMMY_CUSTOMER_NUMBER, "1");
                    appSettings.Add(1 + KEY_DUMMY_CUSTOMER_STREET_AND_HOUSE_NUMBER, "Bybækterrasserne 137 D");
                    appSettings.Add(1 + KEY_DUMMY_CUSTOMER_ZIP_CODE, "3520");
                    appSettings.Add(1 + KEY_DUMMY_CUSTOMER_CITY, "Farum");
                    appSettings.Add(1 + KEY_DUMMY_CUSTOMER_PHONENUMBER, "27708834");
                    appSettings.Add(2 + KEY_DUMMY_CUSTOMER_NAME, "Jannik Vangsgaard");
                    appSettings.Add(2 + KEY_DUMMY_CUSTOMER_NUMBER, "2");
                    appSettings.Add(2 + KEY_DUMMY_CUSTOMER_STREET_AND_HOUSE_NUMBER, "Hovedgade 40");
                    appSettings.Add(2 + KEY_DUMMY_CUSTOMER_ZIP_CODE, "2860");
                    appSettings.Add(2 + KEY_DUMMY_CUSTOMER_CITY, "Søborg");
                    appSettings.Add(2 + KEY_DUMMY_CUSTOMER_PHONENUMBER, "22250898");
                    appSettings.Add(3 + KEY_DUMMY_CUSTOMER_NAME, "Mike Heerwagen");
                    appSettings.Add(3 + KEY_DUMMY_CUSTOMER_NUMBER, "3");
                    appSettings.Add(3 + KEY_DUMMY_CUSTOMER_STREET_AND_HOUSE_NUMBER, "Kollegiebakken 9");
                    appSettings.Add(3 + KEY_DUMMY_CUSTOMER_ZIP_CODE, "2800");
                    appSettings.Add(3 + KEY_DUMMY_CUSTOMER_CITY, "Lyngby");
                    appSettings.Add(3 + KEY_DUMMY_CUSTOMER_PHONENUMBER, "41836990");
                    appSettings.Save();
                }
            }
            catch (IsolatedStorageException)
            {
                Debug.WriteLine("Customers did not get saved in dummyDB");
            }
        }

        public async Task<Customer> getCustomer(long id)
        {
            if (appSettings.Contains(id + KEY_DUMMY_CUSTOMER_NUMBER))
            {
                String customerName = appSettings[id + KEY_DUMMY_CUSTOMER_NAME] as String;
                long customerNumber = Convert.ToInt64(appSettings[id + KEY_DUMMY_CUSTOMER_NUMBER] as String);
                String streetHouseNumber = appSettings[id + KEY_DUMMY_CUSTOMER_STREET_AND_HOUSE_NUMBER] as String;
                int zipCode = Convert.ToInt32(appSettings[id + KEY_DUMMY_CUSTOMER_ZIP_CODE] as String);
                String city = appSettings[id + KEY_DUMMY_CUSTOMER_CITY] as String;
                long phonenumber = Convert.ToInt64(appSettings[id + KEY_DUMMY_CUSTOMER_PHONENUMBER] as String);
                return new Customer(customerName, customerNumber, streetHouseNumber, zipCode, city, phonenumber);
            }
            return null;
        }

        public List<String> getAddress(String id)
        {
            if (appSettings.Contains(id + KEY_DUMMY_ADDRESS_NAME))
            {
                address.Add(appSettings[id + KEY_DUMMY_ADDRESS_NAME] as String);
                address.Add(appSettings[id + KEY_DUMMY_ADDRESS_LONGTITUDE] as String);
                address.Add(appSettings[id + KEY_DUMMY_ADDRESS_LATITUDE] as String);
                return address;
            }

            return null;
        }

        public Boolean createNFCs(List<NFC> nfcs)
        {
            Boolean check = false;
            foreach (NFC nfc in nfcs)
            {
                check = createNFC(nfc);
                if (!check)
                {
                    return check;
                }
            }
            return check;
        }

        public Boolean createNFC(NFC nfc)
        {
            long id = getNextNfcId();

            try
            {
                appSettings.Add(id + KEY_DUMMY_NFC_USER_ID, nfc.User.Id);
                appSettings.Add(id + KEY_DUMMY_NFC_RANGECHECK, nfc.RangeCheck);
                appSettings.Add(id + KEY_DUMMY_NFC_TAGADDRESS, nfc.TagAddress);
                appSettings.Add(id + KEY_DUMMY_NFC_TIME, nfc.Time);

                appSettings.Save();
                return true;
            }
            catch (IsolatedStorageException)
            {
                Debug.WriteLine("Addresses did not get saved in dummyDB");
                return false;
            }

        }

        public async Task<List<NFC>> getNFCs()
        {
            List<NFC> nfcs = new List<NFC>();
            int length = currentNumberOfNFCs();
            long id = 0;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    id = i + 1;
                    NFC nfc = await getNFC(id);
                    nfcs.Add(nfc);
                }
            }
            return nfcs;
        }

        public async Task<NFC> getNFC(long id)
        {
            if (appSettings.Contains(id + KEY_DUMMY_NFC_TAGADDRESS))
            {
                long userId = Convert.ToInt64(appSettings[id + KEY_DUMMY_NFC_USER_ID] as String);
                Boolean rangeCheck = Convert.ToBoolean(appSettings[id + KEY_DUMMY_NFC_RANGECHECK] as String);
                String tagAddress = appSettings[id + KEY_DUMMY_NFC_TAGADDRESS] as String;
                DateTime time = DateTime.Parse(appSettings[id + KEY_DUMMY_NFC_TIME] as String, CultureInfo.InvariantCulture);
                User user = await getUser(userId);
                return new NFC(rangeCheck, tagAddress, time, user);
            }
            else
            {
                return null;
            }
        }

        private long getCurrentNfcId()
        {
            if (!appSettings.Contains(KEY_DUMMY_ID_NFC))
            {
                appSettings.Add(KEY_DUMMY_ID_NFC, "0");
                appSettings.Save();
            }
            return Convert.ToInt64(appSettings[KEY_DUMMY_ID_NFC] as String);
        }

        private long getNextNfcId()
        {
            try
            {
                long test = getCurrentNfcId();
                long nextId = test + 1;
                appSettings.Remove(KEY_DUMMY_ID_NFC);
                appSettings.Add(KEY_DUMMY_ID_NFC, nextId + "");
                appSettings.Save();

                return nextId;
            }
            catch (IsolatedStorageException)
            {
                Debug.WriteLine("error");
                return 1111111111111111111;
            }

        }

        public int currentNumberOfNFCs()
        {
            if (!appSettings.Contains(KEY_DUMMY_CURRENTNUMBER_OF_NFCS))
            {
                appSettings.Add(KEY_DUMMY_CURRENTNUMBER_OF_NFCS, "0");
                appSettings.Save();
            }
            return Convert.ToInt32(appSettings[KEY_DUMMY_CURRENTNUMBER_OF_NFCS] as String);
        }

        public void addNumberOfNFCs()
        {
            try
            {
                int next = currentNumberOfNFCs() + 1;
                if (appSettings.Contains(KEY_DUMMY_CURRENTNUMBER_OF_NFCS))
                {
                    appSettings.Remove(KEY_DUMMY_CURRENTNUMBER_OF_NFCS);
                }
                appSettings.Add(KEY_DUMMY_CURRENTNUMBER_OF_NFCS, next + "");
                appSettings.Save();
            }
            catch (IsolatedStorageException)
            {
                return;
            }
        }

        private long getCurrentAlarmReportId()
        {
            if (!appSettings.Contains(KEY_DUMMY_ID_ALARMREPORT))
            {
                appSettings.Add(KEY_DUMMY_ID_ALARMREPORT, "0");
                appSettings.Save();
            }
            return Convert.ToInt64(appSettings[KEY_DUMMY_ID_ALARMREPORT] as String);
        }

        private long getNextAlarmReportId()
        {
            long nextId = getCurrentAlarmReportId() + 1;
            appSettings.Remove(KEY_DUMMY_ID_ALARMREPORT);
            appSettings.Add(KEY_DUMMY_ID_ALARMREPORT, nextId + "");
            appSettings.Save();
            return nextId;
        }

        public int currentNumberOfAlarmReports()
        {
            if (!appSettings.Contains(KEY_DUMMY_CURRENTNUMBER_OF_ALARMREPORTS))
            {
                appSettings.Add(KEY_DUMMY_CURRENTNUMBER_OF_ALARMREPORTS, "0");
                appSettings.Save();
            }
            return Convert.ToInt32(appSettings[KEY_DUMMY_CURRENTNUMBER_OF_ALARMREPORTS] as String);
        }

        public void addNumberOfAlarmReports()
        {
            int next = currentNumberOfAlarmReports() + 1;
            appSettings.Add(KEY_DUMMY_CURRENTNUMBER_OF_ALARMREPORTS, next + "");
            appSettings.Save();
        }

    }
}
