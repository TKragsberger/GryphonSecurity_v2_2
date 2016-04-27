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
        private Boolean nfcCheck = false;

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
            //TODO fix så der bliver fortalt om der bliver gemt til localstorage eller i dbs
                if (checkNetworkConnection())
                {
                    return await dBFacade.createAlarmReport(alarmReport);
                }
                else
                {
                    return false;
                }

        }

        public Boolean createLocalStorageAlarmReport(AlarmReport alarmReport)
        {
            return dBFacade.createLocalStorageAlarmReport(alarmReport);
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
                    deviceProxomity = false;
                }
            return deviceProxomity;

        }

        public String readDataFromNFCTag(ProximityMessage message, Boolean isConnected)
        {
            DataReader buffer = DataReader.FromBuffer(message.Data);
            int typeLength = buffer.ReadByte();
            int payloadLength = buffer.ReadByte();
            byte[] type = new byte[typeLength];
            byte[] payload = new byte[payloadLength];
            buffer.ReadBytes(type);
            buffer.ReadBytes(payload);  
                if ((Encoding.UTF8.GetString(type, 0, typeLength) == "U"))
                {
                    return getUri(payload);
                } else if(Encoding.UTF8.GetString(type, 0, typeLength) == "T")
                {
                    return getText(payload);
                }
            return null;

        }
        private String getUri(byte[] payload)
        {
            String identifier = getUriIdentifier(payload[0]);
            String uri = Encoding.UTF8.GetString(payload, 1, payload.Length - 1);
            String[] split = new String[] { "hash=" };
            String[] parts = uri.Split(split, StringSplitOptions.None);
            String id = parts[1];
            return id;
        }

        private String getText(byte[] payload)
        {
            byte langLen = (byte)(payload[0] & 0x3f);
            int textleng = payload.Length - 1 - langLen;
            byte[] textbuf = new byte[textleng];
            System.Buffer.BlockCopy(payload, 1 + langLen, textbuf, 0, textleng);
            return Encoding.UTF8.GetString(textbuf, 0, textbuf.Length);
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
                try
                {
               
                    Geoposition geoposition = await geolocator.GetGeopositionAsync(
                        maximumAge: TimeSpan.FromMinutes(5),
                        timeout: TimeSpan.FromSeconds(10)
                        );
                    double latitude = geoposition.Coordinate.Point.Position.Latitude;
                    double longitude = geoposition.Coordinate.Point.Position.Longitude;
                    presentCoordinate = new GeoCoordinate(latitude, longitude);
                    address = await calcPosition(tagAddress, presentCoordinate, isConnected);      
                }
                catch (Exception ex)
                {
                    if ((uint)ex.HResult == 0x80004004)
                    {
                        // the application does not have the right capability or the location master switch is off
                        Debug.WriteLine("location  is disabled in phone settings.");
                    } else
                    {
                    Debug.WriteLine(ex.StackTrace);
                    }
                }
            return address;
        }

        public async Task<String> calcPosition(String tagAddress, GeoCoordinate presentCoordinate, Boolean isConnected)
        {
            String addressName = tagAddress;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
           
            try
            {
                    if (!isConnected)
                    {
                        dBFacade.createLocalStorageNFCs(presentCoordinate.Latitude, presentCoordinate.Longitude, tagAddress);
                        return tagAddress;
                    }
                cancellationTokenSource.CancelAfter(10000);
                
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        Address address = await dBFacade.getAddress(tagAddress);
                            if (address == null)
                            {
                                return null;
                            }
                        addressName = address.AddressName;
                        targetCoordinate = new GeoCoordinate(address.Latitude, address.Longtitude);
                        await getDistance(presentCoordinate, targetCoordinate, addressName);
                    }
                    else
                    {
                        await getDistance(presentCoordinate, presentCoordinate, tagAddress);
                    }

            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine("cancellation token");
                Debug.WriteLine(ex.StackTrace);
            }
            return addressName;
        }

        public async Task<Boolean> getDistance(GeoCoordinate presentCoordinate, GeoCoordinate targetCoordinates, String tagAddress)
        {
            Boolean result = false;
                if (!presentCoordinate.Latitude.Equals(targetCoordinates.Latitude))
                {
                    Boolean rangeCheck = getRangeCheck(presentCoordinate);
                    result = await dBFacade.createNFC(new NFC(rangeCheck, tagAddress, DateTime.Now, dBFacade.getLocalStorageUser().Id));
                        if (!result)
                        {
                            dBFacade.createLocalStorageNFCs(presentCoordinate.Latitude, presentCoordinate.Longitude, tagAddress);
                        }
                   
                }
                else
                {
                    dBFacade.createLocalStorageNFCs(presentCoordinate.Latitude, presentCoordinate.Longitude, tagAddress);
                }
            return result;


        }

        public NFC checkNFC(GeoCoordinate presentCoordinate, GeoCoordinate targetCoordinates, String tagAddress)
        {
            Boolean rangeCheck = getRangeCheck(presentCoordinate);
            return new NFC(rangeCheck, tagAddress, DateTime.Now, dBFacade.getLocalStorageUser().Id);
        }
        
        public Boolean getRangeCheck(GeoCoordinate presentCoordinate)
        {
            double distance = targetCoordinate.GetDistanceTo(presentCoordinate);
            Boolean result = false;
                if (distance > 500)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            return result;
        }

        public void createLocalStorageNFCsTest(double latitude,double longitude, String tagAdress)
        {
            dBFacade.createLocalStorageNFCs(latitude, longitude, tagAdress);
        }

        public bool checkNetworkConnection()
        {
            bool IsConnected = false;
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    IsConnected = true;
                }
                else
                {
                    IsConnected = false;
                }
                   
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

        public int getLocalStorageCustomers()
        {
            return dBFacade.getLocalStorageNumberOfCustomers();
        }

        public async Task<Boolean> sendPendingNFCs()
        {
            List<List<String>> localTags = dBFacade.getLocalStorageNFCs();
            List<NFC> nfcs = new List<NFC>();
            nfcCheck = false;
                foreach (List<String> tags in localTags)
                {
                    double presentLatitude = Convert.ToDouble(tags[0]);
                    double presentLongitude = Convert.ToDouble(tags[1]);
                    Address address = await dBFacade.getAddress(tags[2]);
                    double targetLatitude = address.Latitude;
                    double targetLongtitude = address.Longtitude;
                    presentCoordinate = new GeoCoordinate(presentLatitude, presentLongitude);
                    targetCoordinate = new GeoCoordinate(targetLatitude, targetLongtitude);
                    nfcs.Add(checkNFC(presentCoordinate, targetCoordinate, address.AddressName));
                }
            nfcCheck = await dBFacade.createNFCs(nfcs);
                if (nfcCheck)
                {
                    return dBFacade.removeLocalStorageNFCs();
                }
            return nfcCheck;
        }
     

        public async Task<Boolean> sendPendingAlarmReports()
        {
            Boolean alarmReportCheck = false;
            List<AlarmReport> alarmReports = dBFacade.getLocalStorageAlarmReports();
            alarmReportCheck = await dBFacade.createAlarmReports(alarmReports);
                if (alarmReportCheck)
                {
                    return dBFacade.removeLocalStorageAlarmReports();
                }

            return alarmReportCheck;
        }

        public async Task<Boolean> sendPendingCustomers()
        {
            Boolean customerCheck = false;
            List<Customer> customers = dBFacade.getLocalStorageCustomers();
                for (int i = customers.Count-1; i >= 0; i--)
                {
                    if(await dBFacade.getCustomer(customers[i].CustomerNumber) != null)
                    {
                        customers.RemoveAt(i);
                    }
                }
            customerCheck = await dBFacade.createCustomers(customers);

                if (customerCheck)
                {
                    return dBFacade.removeLocalStorageCustomers();
                }

            return customerCheck;
        }

        public async Task<User> getUser(long id)
        {
            return await dBFacade.getUser(id);
        }

        public async Task<Customer> getCustomer(long id)
        {
            return await dBFacade.getCustomer(id);
        }

        public async Task<Boolean> createCustomer(Customer customer)
        {
            return await dBFacade.createCustomer(customer);
        }

        public Boolean createLocalStorageCustomer(Customer customer)
        {
            return dBFacade.createLocalStorageCustomer(customer);
        }
    }
}