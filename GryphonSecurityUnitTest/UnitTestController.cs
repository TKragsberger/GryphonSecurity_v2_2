using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using GryphonSecurity_v2_2.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using GryphonSecurity_v2_2.Domain;

namespace GryphonSecurityTest
{
    [TestClass]
    public class UnitTestController
    {

        Controller control = Controller.Instance;
        User userTest = new User(1000, "firstnameTest", "lastnameTest", "addressTest", 12345678, "usernameTest", "passwordTest");
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
            Debug.WriteLine("user " + control.getUser().toString());
            Debug.WriteLine("expected Result " + expectedResult);
            //Testing if they are both the same, its testing if the created user is saved.
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodCreateAlarmReport()
        {
            //Setting up alarm report object.
            setupAlarmReport("Test");
            //This is the method we are testing. We will need internet connection for it to be able to save it, if this is false it will save it locally (this will be tested later).
            Boolean actualResult = control.createAlarmReport(alarmReportTest);
            //We expect it to be true, since we have internet connection.
            Assert.AreEqual(true, actualResult);
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
        public void TestMethodReadDataFromNFCTag()
        {
            String expectedResult = null;

            Boolean isConnected = control.checkNetworkConnection();
            String actualResult = control.readDataFromNFCTag(null, isConnected);

            Assert.AreSame(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodOnLocationScan()
        {
            Boolean expectedResult = false;
            Boolean isConnected = control.checkNetworkConnection();
            Task<String> actualResult = control.onLocationScan("Lyngby St.", isConnected);
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestMethodcalcPosition()
        {
            Boolean expectedResult = false;
            Boolean isConnect = control.checkNetworkConnection();
            String actualResult = control.calcPosition("Lyngby St.", null, isConnect);
            Assert.AreSame(expectedResult, actualResult);
        }

        //These two method does not exsist anymore.
        //
        //[TestMethod]
        //public void TestMethodCreateAlarmReport()
        //{
        //    //setting up a Alarm Report.
        //    setupAlarmReport();
        //    //creating a Alarm Report, this is the method we are testing.
        //    instance.createAlarmReport(alarmReportTest);
        //    //the Expected Result
        //    String expectedResult = "customerNameTest";
        //    //the Actual Result
        //    String actualResult = instance.
        //    //
        //    Assert.AreSame(expectedResult, actualResult);

        //}
        //[TestMethod]
        //public void TestMethodLogin()
        //{
        //    //setting up username and password.
        //    String loginUsername = "UsernameTest";
        //    String loginPassword = "passwordTest";
        //    //this is the method we are testing.
        //    instance.login(loginUsername, loginPassword);
        //    String expectedResult = loginUsername;
        //    String actualResult = instance.getUsername();
        //    Assert.AreSame(expectedResult, actualResult);

        //}






        private void setupAlarmReport(String name)
        {
            //random time needed for alarm report object.
            DateTime dateTest = new DateTime(1337, 1, 1);
            DateTime timeTest = new DateTime(1337, 1, 1);
            DateTime guardRadioedDateTest = new DateTime(1337, 1, 1);
            DateTime guardRadioedFromTest = new DateTime(1337, 1, 1);
            DateTime guardRadioedToTest = new DateTime(1337, 1, 1);
            DateTime arrivedAtTest = new DateTime(1337, 1, 1);
            DateTime doneTest = new DateTime(1337, 1, 1);
            //ends here.
            alarmReportTest = new AlarmReport(name, 1234567123, "streetAndHouseNumberTest", 1234, "cityTest", 12345678, dateTest,
                timeTest, "zoneTest", false, false, false, false, false, false, false, false, false, false, "remarkTest",
                "nameTest", "installerTest", "controlCenterTest", guardRadioedDateTest, guardRadioedFromTest, guardRadioedToTest,
                arrivedAtTest, doneTest, userTest);
        }


    }
}
