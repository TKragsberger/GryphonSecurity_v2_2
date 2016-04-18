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

        public async Task<Boolean> createAlarmReport(AlarmReport alarmReport)
        {
            if (checkNetworkConnection())
            {
                Debug.WriteLine("DB");
                return await dBFacade.createAlarmReport(alarmReport);
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
            Debug.WriteLine(buffer.ReadByte());
            Debug.WriteLine(buffer.ReadByte());
            int payloadLength = buffer.ReadByte();
            Debug.WriteLine(buffer.ReadByte());
            byte[] payload = new byte[payloadLength];

            buffer.ReadBytes(payload);
            String fullUri = getUri(payload);
            return fullUri;
            //byte langLen = (byte)(payload[0] & 0x3f);
            //int textLeng = payload.Length - 1 - langLen;
            //byte[] textBuf = new byte[textLeng];
            //System.Buffer.BlockCopy(payload, 1 + langLen, textBuf, 0, textLeng);
            //return Encoding.UTF8.GetString(textBuf, 0, textBuf.Length);
        }
        private String getUri(byte[] payload)
        {
            String identifier = getUriIdentifier(payload[0]);
            String uri = Encoding.UTF8.GetString(payload, 1, payload.Length - 1);
            String fullUri = identifier + uri;
            return fullUri;
        }

        private string getUriIdentifier(byte abbrByte)
        {
            var identifier = "";

            switch (abbrByte)
            {
                case 0x00:
                    identifier = "";
                    break;
                case 0x01:
                    identifier = "http://www.";
                    break;
                case 0x02:
                    identifier = "https://www.";
                    break;
                case 0x03:
                    identifier = "http://";
                    break;
                case 0x04:
                    identifier = "https://";
                    break;
                case 0x05:
                    identifier = "tel:";
                    break;
                case 0x06:
                    identifier = "mailto:";
                    break;
                case 0x07:
                    identifier = "ftp://anonymous:anonymous@";
                    break;
                case 0x08:
                    identifier = "ftp://ftp.";
                    break;
                case 0x09:
                    identifier = "ftps://";
                    break;
                case 0x0A:
                    identifier = "sftp://";
                    break;
                case 0x0B:
                    identifier = "smb://";
                    break;
                case 0x0c:
                    identifier = "nfs://";
                    break;
                case 0x0d:
                    identifier = "ftp://";
                    break;
                case 0x0e:
                    identifier = "dav://";
                    break;
                case 0x0f:
                    identifier = "news:";
                    break;
                case 0x10:
                    identifier = "telnet://";
                    break;
                case 0x11:
                    identifier = "imap:";
                    break;
                case 0x12:
                    identifier = "rtsp://";
                    break;
                case 0x13:
                    identifier = "urn:";
                    break;
                case 0x14:
                    identifier = "pop:";
                    break;
                case 0x15:
                    identifier = "sip:";
                    break;
                case 0x16:
                    identifier = "sips:";
                    break;
                case 0x17:
                    identifier = "tftp:";
                    break;
                case 0x18:
                    identifier = "btspp://";
                    break;
                case 0x19:
                    identifier = "btl2cap://";
                    break;
                case 0x1a:
                    identifier = "btgoep://";
                    break;
                case 0x1b:
                    identifier = "tepobex://";
                    break;
                case 0x1c:
                    identifier = "irdaobex://";
                    break;
                case 0x1d:
                    identifier = "file://";
                    break;
                case 0x1e:
                    identifier = "urn:epc:id:";
                    break;
                case 0x1f:
                    identifier = "urn:epc:tag:";
                    break;
                case 0x20:
                    identifier = "urn:epc:pat:";
                    break;
                case 0x21:
                    identifier = "urn:epc:raw:";
                    break;
                case 0x22:
                    identifier = "urn:epc:";
                    break;
                case 0x23:
                    identifier = "urn:nfc:";
                    break;
                default:
                    identifier = "RFU";
                    break;
            }
            return identifier;
        }

        public async Task<String> onLocationScan(String tagAddress, Boolean isConnected)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            String address = tagAddress;
            Debug.WriteLine("1: " + tagAddress);
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
            Debug.WriteLine("return " + address);
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
                    Debug.WriteLine("1: " + tagAddress);
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
                    Debug.WriteLine("2: " + address);
                }
                else
                {
                    getDistance(presentCoordinate, presentCoordinate, tagAddress);
                    Debug.WriteLine("kommer i else mhmhm");
                }

            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("cancellation token");
            }
            Debug.WriteLine("return: " + address);
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
        public void createLocalStorageNFCsTest(double latitude,double longitude, String tagAdress)
        {
            dBFacade.createLocalStorageNFCs(latitude, longitude, tagAdress);
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