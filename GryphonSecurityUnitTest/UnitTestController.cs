using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using GryphonSecurity_v2_2.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using GryphonSecurity_v2_2.Domain;
using System.Device.Location;
using GryphonSecurity_v2_2.DataSource;

namespace GryphonSecurityTest
{
    [TestClass]
    public class UnitTestController
    {
        
        Controller control = Controller.Instance;
        Mapper mapper = new Mapper();
        User userTest = new User(1000, "firstnameTest", "lastnameTest");
        Customer customerTest = new Customer("mike", 123, "hervej123", 2800, "Lyngby", 41836990);
        NFC nfcTest = new NFC(true, "noget", DateTime.Now, 2);
        LocalStorage localStorage = new LocalStorage();
        AlarmReport alarmReportTest;

        [TestMethod]
        public void TestMethodStartup()
        {
            Boolean notExpected = control.getStartup();

            //Starting the phone, this is the method we are testing.
            control.startUp();
            //The Boolean shouldnt be the same. before startup = true, after startup = false.
            Assert.AreNotSame(notExpected, control.getStartup());
        }

        [TestMethod]
        public void TestMethodCreateUser()
        {
            //creating a user, this is the method we are testing. It returns a boolean if it can save the user.


            control.createUser(userTest);
            String actualResult = control.getUser().toString();
            String expectedResult = userTest.toString();
            //Boolean actualResult = true;
            //Boolean expectedResult = false;
            //Testing if they are both the same, its testing if the created user is saved.
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodCreateUserFail()
        {
            //creating a user, this is the method we are testing. It returns a boolean if it can save the user.
            control.createUser(userTest);
            User userFail = new User(12234, "mike", "niiiiice");
            String actualResult = control.getUser().toString();
            String expectedResult = userFail.toString();
            //Boolean actualResult = true;
            //Boolean expectedResult = false;
            //Testing if they are both the same, its testing if the created user is saved.
            Assert.AreNotEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodCreateAlarmReport()
        {
            //Setting up alarm report object.
            setupAlarmReport("Test");
            //This is the method we are testing. We will need internet connection for it to be able to save it, if this is false it will save it locally (this will be tested later).
            var task = control.createAlarmReport(alarmReportTest);
            task.Wait();
            var response = task.Result;
            //We expect it to be true, since we have internet connection.
            Assert.AreEqual(true, response);
        }
        [TestMethod]
        public void TestMethodCreateAlarmReportFail()
        {
            //Setting up alarm report object.
            setupFailAlarmReport("Test");
            //This is the method we are testing. We will need internet connection for it to be able to save it, if this is false it will save it locally (this will be tested later).
            var task = control.createAlarmReport(alarmReportTest);
            task.Wait();
            var response = task.Result;
            //We expect it to be true, since we have internet connection.
            Assert.AreNotEqual(true, response);
        }
        [TestMethod]
        public void TestMethodCreateTempAlarmReport()
        {
            //Setting up alarm report object.
            setupAlarmReport("Test");
            //This is the method we are testing. If we have no internet Connection it need to save it on local storage.
            Boolean actualResult = control.createTempAlarmReport(alarmReportTest);
            Assert.AreEqual(true, actualResult);
        }
        [TestMethod]
        public void TestMethodCreateTempAlarmReportFail()
        {
            //Setting up alarm report object.
            setupFailTempAlarmReport("Test");
            //This is the method we are testing. If we have no internet Connection it need to save it on local storage.
            Boolean actualResult = control.createTempAlarmReport(alarmReportTest);
            Assert.AreNotEqual(true, actualResult);
        }
        [TestMethod]
        public void TestMethodGetLocalStorageTempAlarmReports()
        {
            //This require to setup some alarm report objects and save it locally first.
            setupAlarmReport("Test1");
            control.createTempAlarmReport(alarmReportTest);
            String expectedResult = alarmReportTest.CustomerName;
            setupAlarmReport("Test2");
            control.createTempAlarmReport(alarmReportTest);
            //This is the method we are testing.
            List<AlarmReport> alarmReports = control.getLocalStorageTempAlarmReports();

            String actualResult = "";
            //we are looking for the alarm report with the name "Test1"
            foreach (AlarmReport alarmReport in alarmReports)
            {
                if (alarmReport.CustomerName.Equals("Test1"))
                    actualResult = alarmReport.CustomerName;
            }
            Assert.AreSame(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodGetLocalStorageTempAlarmReportsFail()
        {
            //This require to setup some alarm report objects and save it locally first.
            setupAlarmReport("Test1");
            control.createTempAlarmReport(alarmReportTest);
            String expectedResult = "Test3Test4";
            setupAlarmReport("Test2");
            control.createTempAlarmReport(alarmReportTest);
            //This is the method we are testing.
            List<AlarmReport> alarmReports = control.getLocalStorageTempAlarmReports();

            String actualResult = "";
            //we are looking for the alarm report with the name "Test1"
            foreach (AlarmReport alarmReport in alarmReports)
            {
                if (alarmReport.CustomerName.Equals("Test1")|| alarmReport.CustomerName.Equals("Test2"))
                    actualResult += alarmReport.CustomerName;
            }
            Assert.AreNotEqual(expectedResult, actualResult);

        }
        //[TestMethod]
        //public void TestMethodReadDataFromNFCTag()
        //{
        //    String expectedResult = null;

        //    Boolean isConnected = control.checkNetworkConnection();
        //    String actualResult = control.readDataFromNFCTag(null, isConnected);

        //    Assert.AreSame(expectedResult, actualResult);
        //}
        [TestMethod]
        public void testGetDistance()
        {
            control.createUser(userTest);
            GeoCoordinate targetCoordinate = new GeoCoordinate(55.767944, 12.505161499999986);
            GeoCoordinate presentCoordinate = new GeoCoordinate(55.7705618401085, 12.5117938768867);
            Boolean expectedResult = true;
            var task = control.getDistance(presentCoordinate, targetCoordinate, "Lyngby st.");
            task.Wait();
            var actualResult = task.Result;
            Debug.WriteLine("getDistence: " + actualResult);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void testGetDistanceFail()
        {
            control.createUser(userTest);
            GeoCoordinate targetCoordinate = new GeoCoordinate(55.767944, 12.505161499999986);
            GeoCoordinate presentCoordinate = new GeoCoordinate(55.7705618401085, 12.5117938768867);
            Boolean expectedResult = true;
            var task = control.getDistance(presentCoordinate, targetCoordinate, null);
            task.Wait();
            var actualResult = task.Result;
            Debug.WriteLine("getDistence: " + actualResult);
            Assert.AreNotEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestMethodOnLocationScan()
        {
            //its supposed to save on database
            String expectedResult = "Lyngby st.";
            var task = control.onLocationScan(expectedResult, true);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodOnLocationScanFail()
        {
            //its supposed to save on database
            String expectedResult = "Lyngby st.";
            var task = control.onLocationScan(expectedResult, false);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreNotEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodcalcPosition()
        {
            String expectedResult = "Alarmpanel stuen gammel bygning";
            GeoCoordinate presentCoordinate = new GeoCoordinate(55.767944, 12.505161499999986);
            var task = control.calcPosition("10c5bf4758f64559d4c2ca6adcd8fd08", presentCoordinate, true);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodcalcPositionFail()
        {
            String expectedResult = "Alarmpanel stuen gammel bygning";
            GeoCoordinate presentCoordinate = new GeoCoordinate(55.767944, 12.505161499999986);
            var task = control.calcPosition("NotAHashCode", presentCoordinate, true);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreNotEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestCheckNetworkConnection()
        {
            Boolean expectedResult = true;
            Boolean actualResult = control.checkNetworkConnection();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestSendPendingNFCs()
        {

            control.createUser(userTest);
            control.createLocalStorageNFCsTest(55.767944, 12.505161499999986, "10c5bf4758f64559d4c2ca6adcd8fd08");
            control.createLocalStorageNFCsTest(55.6713363, 12.566796599999975, "10c5bf4758f64559d4c2ca6adcd8fd08");
            int expectedResult = 2;
            Assert.AreEqual(expectedResult, control.getLocalStorageNFCs());
            var task = control.sendPendingNFCs();
            task.Wait();
            Assert.AreEqual(0, control.getLocalStorageNFCs());

        }
        [TestMethod]
        public void TestsendPendingAlarmReports()
        {
            control.createUser(userTest);
            setupAlarmReport("test");
            control.createLocalStorageAlarmReport(alarmReportTest);
            setupAlarmReport("test1");
            control.createLocalStorageAlarmReport(alarmReportTest);
            int expectedResult = 2;
            Assert.AreEqual(expectedResult, control.getLocalStorageAlarmReports());
            var task = control.sendPendingAlarmReports();
            task.Wait();
            Assert.AreEqual(0, control.getLocalStorageAlarmReports());

        }

        [TestMethod]
        public async Task TestMethodOnLocationScanNoConnection()
        {
            //its supposed to save on local storage
            String expectedResult = "10c5bf4758f64559d4c2ca6adcd8fd08";
            String actualResult = await control.onLocationScan("10c5bf4758f64559d4c2ca6adcd8fd08", false);
            Assert.AreEqual(expectedResult, actualResult);
        }



       

        //Mapper test

        [TestMethod]
        public void TestMethodMapperGetEmployee()
        {
            User expectedResult = new User(1,"Per","Ørving");
            var task = mapper.getEmployee(1);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(expectedResult.toString(), actualResult.toString());
        }
        [TestMethod]
        public void TestMethodMapperGetEmployeeFail()
        {
            Boolean expectedResult = true;
            var task = mapper.getEmployee(89);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreNotEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperGetCustomer()
        {
            Customer expectedResult = new Customer("Mike", 2, "Kollegiebakken", 2800, "Lyngby", 41836990);
            var task = mapper.getCustomer(2);
            task.Wait();
            var actualResult = task.Result;
            Debug.WriteLine("1: " + actualResult.ToString());
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
        }
        [TestMethod]
        public void TestMethodMapperSendAlarmReport()
        {
            setupAlarmReport("TestMethodMapperCreateAlarmReport");
            var task = mapper.sendAlarmReport(alarmReportTest);
            task.Wait();
            var actualResult = task.Result;
            Boolean expectedResult = true;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperSendAlarmReports()
        {
            List<AlarmReport> alarmReports = new List<AlarmReport>();
            setupAlarmReport("TestMethodMapperCreateAlarmReports1");
            alarmReports.Add(alarmReportTest);
            setupAlarmReport("TestMethodMapperCreateAlarmReports2");
            alarmReports.Add(alarmReportTest);
            var task = mapper.sendAlarmReports(alarmReports);
            task.Wait();
            var actualResult = task.Result;
            Boolean expectecResult = true;
            Assert.AreEqual(expectecResult, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperSendCustomers()
        {
            Customer testCustomer1 = new Customer("Thomas", 90, "Farum St.", 4400, "Farum", 23458678);
            Customer testCustomer2 = new Customer("Mike", 99, "Kollegiebakken", 4560, "Kgs. Lyngby", 12387547);
            List<Customer> testCustomers = new List<Customer>();
            testCustomers.Add(testCustomer1);
            testCustomers.Add(testCustomer2);
            var task = mapper.sendCustomers(testCustomers);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(true, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperSendCustomer()
        {
            Customer testCustomer1 = new Customer("Thomas", 2345678, "Farum St.", 4400, "Farum", 23458678);
            var task = mapper.sendCustomer(testCustomer1);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(true, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperSendNFCs()
        {
            NFC testNFC1 = new NFC(false, "Lyngby St.", DateTime.Now, 1000);
            NFC testNFC2 = new NFC(false, "Alarmpanel stuen gammel bygning.", DateTime.Now, 1000);
            List<NFC> testNFCs = new List<NFC>();
            testNFCs.Add(testNFC1);
            testNFCs.Add(testNFC2);
            var task = mapper.sendNFCs(testNFCs);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(true, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperSendNFC()
        {
            NFC testNFC1 = new NFC(false, "Lyngby St.", DateTime.Now, 1000);
            var task = mapper.sendNFC(testNFC1);
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(true, actualResult);
        }
        [TestMethod]
        public void TestMethodMapperGetAddress()
        {
            Address testAddress = new Address("10c5bf4758f64559d4c2ca6adcd8fd08", "Alarmpanel stuen gammel bygning", 55.652763, 12.540680000000066);
            var task = mapper.getAddress("10c5bf4758f64559d4c2ca6adcd8fd08");
            task.Wait();
            var actualResult = task.Result;
            Assert.AreEqual(testAddress.AddressID, actualResult.AddressID);
        }
        [TestMethod]
        public void TestMethodMapperGetAddressFail()
        {
            Address testAddress = new Address("10c5bf4758f64559d4c2ca6adcd8fd08", "Alarmpanel stuen gammel bygning", 55.652763, 12.540680000000066);
            var task = mapper.getAddress("NotAdressID");
            task.Wait();
            var actualResult = task.Result;
            Assert.AreNotEqual(testAddress.AddressID, actualResult.AddressID);
        }
        //[TestMethod]
        //public void TestMethodMapperGetAddressFail()
        //{
        //    var task = mapper.getAddress("1");
        //    task.Wait();
        //    var actualResult = task.Result;
        //    Assert.AreNotEqual(true, actualResult);
        //}

        //LocalStorage Test

        [TestMethod]
        public void TestMethodLocalStorageSaveUser()
        {
            Boolean acutalResult = localStorage.saveUser(userTest);
            Boolean expectedResult = true;
            Assert.AreEqual(expectedResult, acutalResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageSaveUserFail()
        {
            localStorage.saveUser(userTest);
            User actualResult = userTest;
            User testUser = new User(1234556, "ib", "ibsen");
            localStorage.saveUser(testUser);
            User expectedResult = localStorage.getUser();
            Assert.AreNotEqual(expectedResult.toString(), actualResult.toString());
        }
        [TestMethod]
        public void TestMethodLocalStorageGetUser()
        {
            localStorage.saveUser(userTest);
            var acutalResult = localStorage.getUser();
            var expectedResult = userTest;
            Assert.AreEqual(expectedResult.ToString(), acutalResult.ToString());
        }
        [TestMethod]
        public void TestMethodLocalStorageGetUserFail()
        {
            localStorage.saveUser(userTest);
            var acutalResult = localStorage.getUser();
            User expectedResult = new User(666, "bo", "boesen");
            Assert.AreEqual(expectedResult.ToString(), acutalResult.ToString());
        }
        [TestMethod]
        public void TestMethodLocalStorageGetCurrentAlarmReportId()
        {
            long acutalResult = localStorage.getCurrentAlarmReportId();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, acutalResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetNextAlarmReportId()
        {
            long acutalResult = localStorage.getCurrentAlarmReportId();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, acutalResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageCurrentNumberOfAlarmReport()
        {
            long actualResult = localStorage.currentNumberOfAlarmReports();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageAddNumberOfAlarmReports()
        {
            localStorage.addNumberOfAlarmReports();
            long expectedResult = 1;
            long actualResult = localStorage.currentNumberOfAlarmReports();
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageGetCurrentTempAlarmReportId()
        {
            long actualResult = localStorage.getCurrentTempAlarmReportId();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageGetNextTempAlarmReportId()
        {
            long actualResult = localStorage.getNextTempAlarmReportId();
            long expectedResult = 1;
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageCurrentNumberOfTempAlarmReports()
        {
            long actualResult = localStorage.currentNumberOfTempAlarmReports();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageAddNumberOfTempAlarmReports()
        {
            localStorage.addNumberOfTempAlarmReports();
            long expectedResult = 1;
            long actualResult = localStorage.currentNumberOfTempAlarmReports();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetCurrentNfcId()
        {
            long actualResult = localStorage.getCurrentNfcId();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetNextNfcId()
        {
            long actualResult = localStorage.getNextNfcId();
            long expectedResult = 1;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageCurrentNumberOfNFCs()
        {
            long actualResult = localStorage.currentNumberOfNFCs();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageAddNumberOfNFCs()
        {
            localStorage.addNumberOfNFCs();
            long expectedResult = 1;
            long actualResult = localStorage.getCurrentNfcId();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetCurrentCustomerId()
        {
            long actualResult = localStorage.getCurrentCustomerId();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetNextCustomerId()
        {
            long actualResult = localStorage.getNextCustomerId();
            long expectedResult = 1;
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageCurrentNumberOfCustomers()
        {
            long actualResult = localStorage.currentNumberOfCustomers();
            long expectedResult = 0;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageAddNumberOfCustomers()
        {
            localStorage.addNumberOfCustomers();
            long expectedResult = 1;
            long actualResult = localStorage.getCurrentCustomerId();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageSaveAlarmReport()
        {
            setupAlarmReport("TestMethodLocalStorageCreateAlarmReport");
            Boolean actualResult = localStorage.saveAlarmReport(alarmReportTest);
            Boolean expectedResult = true;
            localStorage.removeAlarmReports();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageSaveAlarmReportFail()
        {
            setupAlarmReport("TestMethodLocalStorageCreateAlarmReport");
            localStorage.saveAlarmReport(alarmReportTest);
            int actualResult = localStorage.getAlarmReports().Count;
            int expectedResult = 0;
            localStorage.removeAlarmReports();
            Assert.AreNotEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetAlarmReports()
        {
            control.createUser(userTest);
            setupAlarmReport("localGetAlarmReports");
            localStorage.saveAlarmReport(alarmReportTest);
            List<AlarmReport> list = localStorage.getAlarmReports();
            int actualResult = list.Count;
            int expectedResult = 1;
            localStorage.removeAlarmReports();
            Assert.AreEqual(expectedResult, actualResult);
          
        }
        [TestMethod]
        public void TestMethodLocalStorageGetAlarmReportsFail()
        {
            control.createUser(userTest);
            setupAlarmReport("localGetAlarmReports");
            localStorage.saveAlarmReport(alarmReportTest);
            List<AlarmReport> list = localStorage.getAlarmReports();
            int actualResult = list.Count;
            int expectedResult = 0;
            localStorage.removeAlarmReports();
            Assert.AreNotEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageRemoveAlarmReports()
        {
            setupAlarmReport("TestMethodLocalStorageRemoveAlarmReports");
            localStorage.saveAlarmReport(alarmReportTest);
            Boolean actualResult = localStorage.removeAlarmReports();
            Boolean expectedResult = true;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageSaveNFC()
        {
            Boolean actualResult = localStorage.saveNFC(52, 12, "localCreateNFC");
            Boolean expectedResult = true;
            localStorage.removeNFCs();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestMethodLocalStorageGetNFCs()
        {
            localStorage.saveNFC(52, 12, "LocalGetNFCs");
            List<List<String>> list = localStorage.getNFCs();
            int acutalResult = list.Count;
            int expectedResult = 1;
            localStorage.removeNFCs();
            Assert.AreEqual(expectedResult, acutalResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetNFCsFail()
        {
            localStorage.saveNFC(52, 12, "LocalGetNFCs");
            List<List<String>> list = localStorage.getNFCs();
            int acutalResult = list.Count;
            int expectedResult = 0;
            localStorage.removeNFCs();
            Assert.AreNotEqual(expectedResult, acutalResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageRemoveNFCs()
        {
            localStorage.saveNFC(52, 12, "LocalGetNFCs");
            Boolean acutalResult = localStorage.removeNFCs();
            Boolean expectedResult = true;
            Assert.AreEqual(expectedResult, acutalResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageCreateTempAlarmReport()
        {
            setupAlarmReport("TestMethodLocalStorageCreateTempAlarmReport");
            Boolean actualResult = localStorage.createTempAlarmReport(alarmReportTest);
            Boolean expectedResult = true;
            localStorage.removeAlarmReports();
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageGetTempAlarmReports()
        {
            control.createUser(userTest);
            setupAlarmReport("TestMethodLocalStorageGetTempAlarmReports");
            localStorage.createTempAlarmReport(alarmReportTest);
            setupAlarmReport("TestMethodLocalStorageGetTempAlarmRepots1");
            localStorage.createTempAlarmReport(alarmReportTest);
            List<AlarmReport> list = localStorage.getTempAlarmReports();
            int actualResult = list.Count;
            int expectedResult = 2;
            localStorage.removeAlarmReports();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetTempAlarmReport()
        {
            setupAlarmReport("TestMethodLocalStorageGetTempAlarmReport");
            AlarmReport expectedResult = alarmReportTest;
            localStorage.saveAlarmReport(alarmReportTest);
            AlarmReport actuaclResult = localStorage.getTempAlarmReport(1);
            localStorage.removeAlarmReports();
            Assert.AreEqual(expectedResult.Name, actuaclResult.Name);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetTempAlarmReportFail()
        {
            setupAlarmReport("TestMethodLocalStorageGetTempAlarmReport");
            AlarmReport expectedResult = alarmReportTest;
            localStorage.saveAlarmReport(alarmReportTest);
            AlarmReport actuaclResult = localStorage.getTempAlarmReport(69);
            localStorage.removeAlarmReports();
            Assert.AreNotEqual(expectedResult.Name, actuaclResult.Name);
        }
        [TestMethod]
        public void TestMethodLocalStorageRemoveTempAlarmReport()
        {
            control.createUser(userTest);
            setupAlarmReport("localRemoveTempAlarmReport");
            localStorage.createTempAlarmReport(alarmReportTest);
            Boolean actualResult = localStorage.removeTempAlarmReport(1);
            Boolean expectedResult = true;
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageAddRemaningTempAlarmReports()
        {
            control.createUser(userTest);
            List<AlarmReport> list = new List<AlarmReport>();
            setupAlarmReport("addRemaningTempAlarmReports");
            list.Add(alarmReportTest);
            setupAlarmReport("addRemaningTempAlarmReports1");
            list.Add(alarmReportTest);
            Boolean acutalResult = localStorage.addRemaningTempAlarmReports(list);
            Boolean expectedResult = true;
            localStorage.removeTempAlarmReport(1);
            localStorage.removeTempAlarmReport(2);
            Assert.AreEqual(expectedResult, acutalResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageSaveCustomer()
        {
            Boolean actualResult = localStorage.saveCustomer(customerTest);
            Boolean expectedResult = true;
            localStorage.removeCustomers();
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void TestMethodLocalStorageGetCustomers()
        {
            localStorage.saveCustomer(customerTest);
            List<Customer> list = localStorage.getCustomers();
            int actualResult = list.Count;
            int expectedResult = 1;
            localStorage.removeCustomers();
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodLocalStorageGetCustomersFail()
        {
            localStorage.saveCustomer(customerTest);
            List<Customer> list = localStorage.getCustomers();
            int actualResult = list.Count;
            int expectedResult = 0;
            localStorage.removeCustomers();
            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void TestMethodLocalStorageRemoveCustomers()
        {
            localStorage.saveCustomer(customerTest);
            Boolean actualResult = localStorage.removeCustomers();
            Boolean expectedResult = true;
            Assert.AreEqual(expectedResult, actualResult);

        }




        private void setupAlarmReport(String name)
        {
            //random time needed for alarm report object.
            DateTime date = new DateTime(1337, 1, 1);
            String dateTest = date.ToString("yyyy:MM:dd");
            DateTime time = new DateTime(1337, 1, 1);
            String timeTest = time.ToString("H:mm:ss");
            DateTime cancelDuringEmergencyTime = new DateTime(1337, 1, 1);
            String cancelDuringEmergencyTimeTest = cancelDuringEmergencyTime.ToString("H:mm:ss");
            DateTime guardRadioedDate = new DateTime(1337, 1, 1);
            String guardRadioedDateTest = guardRadioedDate.ToString("yyyy:MM:dd");
            DateTime guardRadioedFrom = new DateTime(1337, 1, 1);
            String guardRadioedFromTest = guardRadioedFrom.ToString("H:mm:ss");
            DateTime guardRadioedTo = new DateTime(1337, 1, 1);
            String guardRadioedToTest = guardRadioedTo.ToString("H:mm:ss");
            DateTime arrivedAt = new DateTime(1337, 1, 1);
            String arrivedAtTest = arrivedAt.ToString("H:mm:ss");
            DateTime done = new DateTime(1337, 1, 1);
            String doneTest = done.ToString("H:mm:ss");
            //ends here.
            alarmReportTest = new AlarmReport(name, 1234567123, "streetAndHouseNumberTest", 1234, "cityTest", 12345678, dateTest,
                timeTest, "zoneTest", false, false, false, false, false, false, false, false, "000", false, cancelDuringEmergencyTimeTest, false, "", "remarkTest",
                "nameTest", "installerTest", "controlCenterTest", guardRadioedDateTest, guardRadioedFromTest, guardRadioedToTest,
                arrivedAtTest, doneTest, userTest.Id);
        }

        private void setupFailAlarmReport(String name)
        {
            //random time needed for alarm report object.
            DateTime date = new DateTime(1337, 1, 1);
            String dateTest = date.ToString("yyyy:MM:dd");
            DateTime time = new DateTime(1337, 1, 1);
            String timeTest = time.ToString("H:mm:ss");
            DateTime cancelDuringEmergencyTime = new DateTime(1337, 1, 1);
            String cancelDuringEmergencyTimeTest = cancelDuringEmergencyTime.ToString("H:mm:ss");
            DateTime guardRadioedDate = new DateTime(1337, 1, 1);
            String guardRadioedDateTest = guardRadioedDate.ToString("yyyy:MM:dd");
            DateTime guardRadioedFrom = new DateTime(1337, 1, 1);
            String guardRadioedFromTest = guardRadioedFrom.ToString("H:mm:ss");
            DateTime guardRadioedTo = new DateTime(1337, 1, 1);
            String guardRadioedToTest = guardRadioedTo.ToString("H:mm:ss");
            DateTime arrivedAt = new DateTime(1337, 1, 1);
            String arrivedAtTest = arrivedAt.ToString("H:mm:ss");
            DateTime done = new DateTime(1337, 1, 1);
            String doneTest = done.ToString("H:mm:ss");
            //ends here.
            alarmReportTest = new AlarmReport(name, 1234567123, null, 1234, "cityTest", 12345678, dateTest,
                timeTest, "zoneTest", false, false, false, false, false, false, false, false, "000", false, cancelDuringEmergencyTimeTest, false, "", "remarkTest",
                "nameTest", "installerTest", "controlCenterTest", guardRadioedDateTest, guardRadioedFromTest, guardRadioedToTest,
                arrivedAtTest, doneTest, userTest.Id);
        }

        private void setupFailTempAlarmReport(String name)
        {
            //random time needed for alarm report object.
            DateTime date = new DateTime(1337, 1, 1);
            String dateTest = date.ToString("yyyy:MM:dd");
            DateTime time = new DateTime(1337, 1, 1);
            String timeTest = time.ToString("H:mm:ss");
            DateTime cancelDuringEmergencyTime = new DateTime(1337, 1, 1);
            String cancelDuringEmergencyTimeTest = cancelDuringEmergencyTime.ToString("H:mm:ss");
            DateTime guardRadioedDate = new DateTime(1337, 1, 1);
            String guardRadioedDateTest = guardRadioedDate.ToString("yyyy:MM:dd");
            DateTime guardRadioedFrom = new DateTime(1337, 1, 1);
            String guardRadioedFromTest = guardRadioedFrom.ToString("H:mm:ss");
            DateTime guardRadioedTo = new DateTime(1337, 1, 1);
            String guardRadioedToTest = guardRadioedTo.ToString("H:mm:ss");
            DateTime arrivedAt = new DateTime(1337, 1, 1);
            String arrivedAtTest = arrivedAt.ToString("H:mm:ss");
            DateTime done = new DateTime(1337, 1, 1);
            String doneTest = done.ToString("H:mm:ss");
            //ends here.
            alarmReportTest = new AlarmReport(null, 1234567123, "streetAndHouseNumberTest", 1234, "cityTest", 12345678, dateTest,
                timeTest, "zoneTest", false, false, false, false, false, false, false, false, "000", false, cancelDuringEmergencyTimeTest, false, "", "remarkTest",
                "nameTest", "installerTest", "controlCenterTest", guardRadioedDateTest, guardRadioedFromTest, guardRadioedToTest,
                arrivedAtTest, doneTest, userTest.Id);
        }


    }
}
