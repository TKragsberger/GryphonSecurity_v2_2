using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using GryphonSecurity_v2_2.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using GryphonSecurity_v2_2.Domain;
using System.Device.Location;

namespace GryphonSecurityUnitTest
{
    [TestClass]
    class UnitTestControllerNoNetwork
    {
        //her mike
        Controller control = Controller.Instance;
        User userTest = new User(1000, "firstnameTest", "lastnameTest");
        AlarmReport alarmReportTest;

        [TestMethod]
        public async Task TestMethodOnLocationScanNoConnection()
        {
            //its supposed to save on local storage
            String expectedResult = "lyngby St.";
            String actualResult;
            actualResult = await control.onLocationScan(expectedResult, false);
            Debug.WriteLine("tagadress: " + actualResult);
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

    }
    
}
