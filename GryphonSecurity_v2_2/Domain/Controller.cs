using GryphonSecurity_v2_2.DataSource;
using GryphonSecurity_v2_2.Domain.Entity;
using GryphonSecurity_v2_2.Resources;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Geolocation;
using Windows.Networking.Proximity;
using Windows.Storage.Streams;

namespace GryphonSecurity_v2_2.Domain
{
    public class Controller
    {
        private Windows.Networking.Proximity.ProximityDevice device;
        private DBFacade dBFacade;
        private Boolean startup = true;
        private GeoCoordinate presentCoordinate;
        private GeoCoordinate targetCoordinate;
        private static Controller instance;
        private Boolean check = false;

        private Controller()
        {
            dBFacade = new DBFacade();
        }
        public static Controller Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Controller();
                }
                return instance;
            }
        }

        public void startUp()
        {
            startup = false;
        }

        public Boolean getStartup()
        {
            return startup;
        }

        public Boolean createUser(User user)
        {
            return dBFacade.createUser(user);
        }

        public User getUser()
        {
            return dBFacade.getLocalStorageUser();
        }

        public Boolean createAlarmReport(AlarmReport alarmReport)
        {
            if (checkNetworkConnection())
            {
                Debug.WriteLine("DB");
                return dBFacade.createAlarmReport(alarmReport);
            }
            else
            {
                Debug.WriteLine("TEMP");
                return dBFacade.createLocalStorageAlarmReport(alarmReport);
            }

        }
        public Boolean createTempAlarmReport(AlarmReport alarmReport)
        {
            return dBFacade.createTempLocalStorageAlarmReport(alarmReport);
        }
        public List<AlarmReport> getLocalStorageTempAlarmReports()
        {
            return dBFacade.getLocalStorageTempAlarmReports();
        }
        private Boolean initializeProximitySample()
        {
            Boolean deviceProxomity = true;
            device = Windows.Networking.Proximity.ProximityDevice.GetDefault();
            if (device == null)
            {
                Debug.WriteLine("Failed to initialized proximity device.\n" +
                                 "Your device may not have proximity hardware.");
                deviceProxomity = false;
            }
            return deviceProxomity;

        }

        public String readDataFromNFCTag(ProximityMessage message, Boolean isConnected)
        {
            DataReader buffer = DataReader.FromBuffer(message.Data);
            Debug.WriteLine("1: " + buffer.ReadByte());
            Debug.WriteLine("2: " + buffer.ReadByte());
            int payloadLength = buffer.ReadByte();
            Debug.WriteLine("5: " + buffer.ReadByte());
            byte[] payload = new byte[payloadLength];
            buffer.ReadBytes(payload);
            byte langLen = (byte)(payload[0] & 0x3f);
            int textLeng = payload.Length - 1 - langLen;
            byte[] textBuf = new byte[textLeng];
            System.Buffer.BlockCopy(payload, 1 + langLen, textBuf, 0, textLeng);
            return Encoding.UTF8.GetString(textBuf, 0, textBuf.Length);
        }

        public async Task<String> onLocationScan(String tagAddress, Boolean isConnected)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            String address = tagAddress;
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                double latitude = geoposition.Coordinate.Point.Position.Latitude;
                double longitude = geoposition.Coordinate.Point.Position.Longitude;
                Debug.WriteLine("longtitude " + longitude + " latitude: " + latitude);
                presentCoordinate = new GeoCoordinate(latitude, longitude);
                address = calcPosition(tagAddress, presentCoordinate, isConnected);

            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    Debug.WriteLine("location  is disabled in phone settings.");
                }
            }

            return address;
        }

        public String calcPosition(String tagAddress, GeoCoordinate presentCoordinate, Boolean isConnected)
        {
            double latitude = 0d;
            double longitude = 0d;
            String address = tagAddress;
            Boolean check = false;
            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                if (!isConnected)
                {
                    dBFacade.createLocalStorageNFCs(presentCoordinate.Latitude, presentCoordinate.Longitude, tagAddress);
                    return tagAddress;
                }
                cts.CancelAfter(10000);
                List<String> tag = dBFacade.getAddress(tagAddress);
                if (!cts.IsCancellationRequested)
                {
                    address = tag[0];
                    longitude = (Double)Convert.ToDecimal(tag[1]);
                    latitude = (Double)Convert.ToDecimal(tag[2]);
                    targetCoordinate = new GeoCoordinate(latitude, longitude);
                    check = getDistance(presentCoordinate, targetCoordinate, address);
                }
                else
                {
                    getDistance(presentCoordinate, presentCoordinate, tagAddress);
                }

            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("cancellation token");
            }
            return address;
        }

        public Boolean getDistance(GeoCoordinate presentCoordinate, GeoCoordinate targetCoordinates, String tagAddress)
        {
            Boolean check = false;
            if (!presentCoordinate.Latitude.Equals(targetCoordinates.Latitude))
            {
                double distance = targetCoordinates.GetDistanceTo(presentCoordinate);
                Boolean rangeCheck = false;
                if (distance > 500)
                {
                    rangeCheck = false;
                }
                else
                {
                    rangeCheck = true;
                }
                check = dBFacade.createNFC(new NFC(rangeCheck, tagAddress, DateTime.Now, dBFacade.getLocalStorageUser()));
            }
            else
            {
                dBFacade.createLocalStorageNFCs(presentCoordinate.Latitude, presentCoordinate.Longitude, tagAddress);
            }
            return check;


        }

        public bool checkNetworkConnection()
        {
            NetworkInterfaceType ni = NetworkInterface.NetworkInterfaceType;
            bool IsConnected = false;
            if ((ni == NetworkInterfaceType.Wireless80211) || (ni == NetworkInterfaceType.MobileBroadbandCdma) || (ni == NetworkInterfaceType.MobileBroadbandGsm))
                IsConnected = true;
            else if (ni == NetworkInterfaceType.None)
                IsConnected = false;
            return IsConnected;
        }

        public AlarmReport getLocalTempAlarmReport(long id)
        {
            return dBFacade.getLocalTempAlarmReport(id);
        }

        public Boolean removeLocalStorageTempSelectedAlarmReport(long id)
        {
            return dBFacade.removeLocalStorageTempSelectedAlarmReport(id);
        }

        public int getLocalStorageNFCs()
        {
            return dBFacade.getLocalStorageNumberOfNFCs();
        }

        public int getLocalStorageAlarmReports()
        {
            return dBFacade.getLocalStorageNumberOfAlarmReports();
        }

        public Boolean sendPendingNFCs()
        {
            List<List<String>> tags = dBFacade.getLocalStorageNFCs();
            check = false;
            foreach (List<String> tag in tags)
            {
                double presentLatitude = Convert.ToDouble(tag[0]);
                double presentLongitude = Convert.ToDouble(tag[1]);
                List<String> nfcs = dBFacade.getAddress(tag[2]);
                String tagAddress = nfcs[0];
                double targetLongtitude = Convert.ToDouble(nfcs[1]);
                double targetLatitude = Convert.ToDouble(nfcs[2]);
                presentCoordinate = new GeoCoordinate(presentLatitude, presentLongitude);
                targetCoordinate = new GeoCoordinate(targetLatitude, targetLongtitude);
                check = getDistance(presentCoordinate, targetCoordinate, tagAddress);
            }
            dBFacade.removeLocalStorageNFCs();
            return check;
        }

        public Boolean sendPendingAlarmReports()
        {
            Boolean alarmReportCheck = false;
            List<AlarmReport> alarmReports = dBFacade.getLocalStorageAlarmReports();
            alarmReportCheck = dBFacade.createAlarmReports(alarmReports);

            if (alarmReportCheck)
            {
                return dBFacade.removeLocalStorageAlarmReports();
            }

            return alarmReportCheck;
        }

        public async Task<User> getUser(long id)
        {
            return await dBFacade.getUser(id);
        }

        public async Task<Customer> getCustomer(long id)
        {
            return await dBFacade.getCustomer(id);
        }
    }
}