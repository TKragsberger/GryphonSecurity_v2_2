using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GryphonSecurity_v2_2.Resources;
using GryphonSecurity_v2_2.Domain;
using GryphonSecurity_v2_2.Domain.Entity;
using Windows.Networking.Proximity;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GryphonSecurity_v2_2
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Controller controller = Controller.Instance;
        private Windows.Networking.Proximity.ProximityDevice device;
        private long deviceId;
        private Boolean isConnected = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            initializeProximitySample();
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;            
            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        public async Task<Boolean> checkAlarmReport()
        {
            Boolean check = true;
            if (textBoxCustomerName.Text.Equals(""))
            {
                check = false;
            }
            String customerNameTB = textBoxCustomerName.Text;
            long customerNumberTB = 0;
            if (!textBoxCustomerNumber.Text.Equals(""))
            {
                customerNumberTB = Convert.ToInt64(textBoxCustomerNumber.Text);
            }
            else
            {
                check = false;
            }
            if (textBoxStreetAndHouseNumber.Text.Equals(""))
            {
                check = false;
            }
            String streetAndHouseNumberTB = textBoxStreetAndHouseNumber.Text;
            int zipCodeTB = 0;
            if (!textBoxZipCode.Text.Equals(""))
            {
                zipCodeTB = Convert.ToInt32(textBoxZipCode.Text);
            }
            else
            {
                check = false;
            }
            if (textBoxCity.Text.Equals(""))
            {
                check = false;
            }
            String cityTB = textBoxCity.Text;
            long phonenumberTB = 0;
            if (!textBoxPhonenumber.Text.Equals(""))
            {
                phonenumberTB = Convert.ToInt64(textBoxPhonenumber.Text);
            }
            DateTime dateTB = (DateTime)textBoxDate.Value;
            DateTime timeTB = (DateTime)textBoxTime.Value;
            String zoneTB = textBoxZone.Text;

            Boolean burglaryVandalismCB = (Boolean)checkBoxBurglaryVandalism.IsChecked;
            Boolean windowDoorClosedCB = (Boolean)checkBoxWindowDoorClosed.IsChecked;
            Boolean apprehendedPersonCB = (Boolean)checkBoxApprehendedPerson.IsChecked;
            Boolean staffErrorCB = (Boolean)checkBoxStaffError.IsChecked;
            Boolean nothingToReportCB = (Boolean)checkBoxNothingToReport.IsChecked;
            Boolean technicalErrorCB = (Boolean)checkBoxTechnicalError.IsChecked;
            Boolean unknownReasonCB = (Boolean)checkBoxUnknownReason.IsChecked;
            Boolean otherCB = (Boolean)checkBoxOther.IsChecked;
            Boolean cancelDuringEmergencyCB = (Boolean)checkBoxCancelsDuringEmergency.IsChecked;
            Boolean coverMadeCB = (Boolean)checkBoxCoverMade.IsChecked;

            String remarkTB = textBoxRemark.Text;
            if (textBoxName.Text.Equals(""))
            {
                check = false;
            }
            String nameTB = textBoxName.Text;
            if (textBoxInstaller.Text.Equals(""))
            {
                check = false;
            }
            String installerTB = textBoxInstaller.Text;
            if (textBoxControlCenter.Text.Equals(""))
            {
                check = false;
            }
            String controlCenterTB = textBoxControlCenter.Text;
            DateTime guardRadioedDateTB = (DateTime)textBoxGuardRadioedDate.Value;
            DateTime guardRadioedFromTB = (DateTime)textBoxGuardRadioedFrom.Value;
            DateTime guardRadioedToTB = (DateTime)textBoxGuardRadioedTo.Value;
            DateTime arrivedAtTB = (DateTime)textBoxArrivedAt.Value;
            DateTime doneTB = (DateTime)textBoxDone.Value;
            if (check)
            {
                if (await controller.createAlarmReport(new AlarmReport(customerNameTB, customerNumberTB, streetAndHouseNumberTB, zipCodeTB, cityTB, phonenumberTB, dateTB, timeTB, zoneTB, burglaryVandalismCB,
                                            windowDoorClosedCB, apprehendedPersonCB, staffErrorCB, nothingToReportCB, technicalErrorCB, unknownReasonCB, otherCB, cancelDuringEmergencyCB, coverMadeCB,
                                            remarkTB, nameTB, installerTB, controlCenterTB, guardRadioedDateTB, guardRadioedFromTB, guardRadioedToTB, arrivedAtTB, doneTB, controller.getUser())))
                {
                    emptyAlarmReport();
                    MessageBox.Show(AppResources.ReportAlarmReportSuccess);
                }
                else
                {
                    MessageBox.Show(AppResources.ReportAlarmReportFailed);
                }
            }
            else
            {
                return check;
            }
            return check;
        }

        public void emptyAlarmReport()
        {
            
            textBoxCustomerName.Text = "";
            textBoxCustomerNumber.Text = "";
            textBoxStreetAndHouseNumber.Text = "";
            textBoxZipCode.Text = "";
            textBoxCity.Text = "";
            textBoxPhonenumber.Text = "";
            textBoxZone.Text = "";
            checkBoxBurglaryVandalism.IsChecked = false;
            checkBoxWindowDoorClosed.IsChecked = false;
            checkBoxApprehendedPerson.IsChecked = false;
            checkBoxStaffError.IsChecked = false;
            checkBoxNothingToReport.IsChecked = false;
            checkBoxTechnicalError.IsChecked = false;
            checkBoxUnknownReason.IsChecked = false;
            checkBoxOther.IsChecked = false;
            checkBoxCancelsDuringEmergency.IsChecked = false;
            checkBoxCoverMade.IsChecked = false;
            textBoxRemark.Text = "";
            textBoxInstaller.Text = "";
            textBoxControlCenter.Text = "";
        }

        private async void sendReport_Click(object sender, RoutedEventArgs e)
        {
            Boolean check =await checkAlarmReport();
            if (!check)
            {
                MessageBox.Show(AppResources.ReportFillSpaces);
            }
        }  

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            if (object.ReferenceEquals(controller.getUser(), null))
            {
                NavigationService.Navigate(new Uri("/RegisterLayout.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton registerBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/pencil.png", UriKind.Relative));
            registerBarButton.Text = AppResources.AppBarButtonText;
            ApplicationBar.Buttons.Add(registerBarButton);
            registerBarButton.Click += RegisterBarButton_Click;

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem aboutMe = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            ApplicationBar.MenuItems.Add(aboutMe);
            aboutMe.Click += AboutMe_Click;
        }

        private void messageReceived(ProximityDevice sender, ProximityMessage message)
        {
            isConnected = controller.checkNetworkConnection();
            String tagAddress = controller.readDataFromNFCTag(message, isConnected);
            //Debug.WriteLine("Tagaddress" + tagAddress);
            Dispatcher.BeginInvoke(() =>
                {
                    gps(tagAddress, isConnected);
                });         
        }

        private async void gps(String tagAddress, Boolean isConnected)
        {
            String address = await controller.onLocationScan(tagAddress, isConnected);
            long number;
            if(!Int64.TryParse(address, out number))
            {
                textBlockNFCScanInformation.Text = AppResources.ScanNFCTagAddress + ": " + address + "\r\n" + AppResources.ScanNFCScanTime + ": " + DateTime.Now;
            } else
            {
                textBlockNFCScanInformation.Text = AppResources.ScanNFCTagAddress + ": " + address + "\r\n" + AppResources.ScanNFCScanTime + ": " + DateTime.Now + "\r\n" + AppResources.ScanNFCTempStorage;
            }
        }

        private void initializeProximitySample()
        {
            device = Windows.Networking.Proximity.ProximityDevice.GetDefault();
            if (device == null)
                Debug.WriteLine("Failed to initialized proximity device.\n" +
                                 "Your device may not have proximity hardware.");
        }

        private void AboutMe_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RegisterBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegisterLayout.xaml", UriKind.RelativeOrAbsolute));
        }

        public void updateAlarmReportDateTime()
        {
            textBoxDate.Value = DateTime.Now;
            textBoxTime.Value = DateTime.Now;
            textBoxGuardRadioedDate.Value = DateTime.Now;
            textBoxGuardRadioedFrom.Value = DateTime.Now;
            textBoxGuardRadioedTo.Value = DateTime.Now;
            textBoxArrivedAt.Value = DateTime.Now;
            textBoxDone.Value = DateTime.Now;
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (pivot.SelectedIndex == 0)
                {
                    deviceId = device.SubscribeForMessage("NDEF", messageReceived);
                } else if(pivot.SelectedIndex == 1)
                {
                    User user = controller.getUser();
                    if (user != null)
                    {
                        textBoxName.Text = user.Firstname + " " + user.Lastname;
                    }
                    device.StopSubscribingForMessage(deviceId);
                    if (! await checkAlarmReport())
                    {
                        updateAlarmReportDateTime();
                    }
                } else if(pivot.SelectedIndex == 2)
                {
                    device.StopSubscribingForMessage(deviceId);
                    int nfcs = controller.getLocalStorageNFCs();
                    int alarmReports = controller.getLocalStorageAlarmReports();
                    textBlockPendingNFCScans.Text = "Pending NFCs: " + nfcs;
                    textBlockPendingAlarmReports.Text = "Pending Alarm Reports: " + alarmReports;
                    tempAlarmReportScroll.Children.Clear();
                    List<AlarmReport> tempAlarmReports = controller.getLocalStorageTempAlarmReports();
                    for (int i = 0; i < tempAlarmReports.Count; i++){
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = tempAlarmReports[i].CustomerName + "\r\n" + tempAlarmReports[i].CustomerNumber + "\r\n" + tempAlarmReports[i].Time;
                        textBlock.Tap += myTextBlock_Tap;
                        textBlock.FontSize = 25;
                    
                        textBlock.Name =""+ (i + 1);
                        tempAlarmReportScroll.Children.Add(textBlock);
                    }
                }
            }
            catch(NullReferenceException)
            {
                MessageBox.Show(AppResources.ReportNFCDisabled);
            }

        }
        public void myTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs args)
        {
            TextBlock textBlock = (TextBlock) args.OriginalSource;
            long id = Int64.Parse(textBlock.Name);

            AlarmReport currentAlarmReport = controller.getLocalTempAlarmReport(id);
            
            if (controller.removeLocalStorageTempSelectedAlarmReport(id))
            {
                fillAlarmReport(currentAlarmReport);
                pivot.SelectedItem = alarmReport;
            } else
            {
                MessageBox.Show(AppResources.ReportError);
            }
        }
        public void fillAlarmReport(AlarmReport alarmReport)
        {
            textBoxCustomerName.Text = alarmReport.CustomerName;
            if (alarmReport.CustomerNumber == 0)
                textBoxCustomerNumber.Text = "";
            else 
            textBoxCustomerNumber.Text =""+ alarmReport.CustomerNumber;

            textBoxStreetAndHouseNumber.Text = alarmReport.StreetAndHouseNumber;
            if (alarmReport.ZipCode == 0)
                textBoxZipCode.Text = "";
            else
                textBoxZipCode.Text = "" + alarmReport.ZipCode;

            textBoxCity.Text = alarmReport.City;
            if (alarmReport.Phonenumber == 0)
                textBoxPhonenumber.Text = "";
            else
                textBoxPhonenumber.Text =""+ alarmReport.Phonenumber;
            textBoxDate.Value = alarmReport.Date;
            textBoxTime.Value = alarmReport.Time;
            textBoxZone.Text = alarmReport.Zone +"";
            if (alarmReport.BurglaryVandalism)
                checkBoxBurglaryVandalism.IsChecked = true;
            if (alarmReport.WindowDoorClosed)
                checkBoxWindowDoorClosed.IsChecked = true;
            if (alarmReport.ApprehendedPerson)
            checkBoxApprehendedPerson.IsChecked = true;
            if (alarmReport.StaffError)
            checkBoxStaffError.IsChecked = true;
            if (alarmReport.NothingToReport)
            checkBoxNothingToReport.IsChecked = true;
            if (alarmReport.TechnicalError)
            checkBoxTechnicalError.IsChecked = true;
            if (alarmReport.UnknownReason)
            checkBoxUnknownReason.IsChecked = true;
            if (alarmReport.Other)
            checkBoxOther.IsChecked = true;
            if (alarmReport.CancelDuringEmergency)
            checkBoxCancelsDuringEmergency.IsChecked =true;
            if (alarmReport.CoverMade)
            checkBoxCoverMade.IsChecked=true;
            textBoxRemark.Text = alarmReport.Remark;
            textBoxName.Text = alarmReport.Name;
            textBoxInstaller.Text = alarmReport.Installer;
            textBoxControlCenter.Text = alarmReport.ControlCenter;
            textBoxGuardRadioedDate.Value = alarmReport.GuardRadioedDate;
            textBoxGuardRadioedFrom.Value = alarmReport.GuardRadioedFrom;
            textBoxGuardRadioedTo.Value = alarmReport.GuardRadioedTo;
            textBoxArrivedAt.Value = alarmReport.ArrivedAt;
            textBoxDone.Value = alarmReport.Done;
        }

        private void sendPendingButton_Click(object sender, RoutedEventArgs e)
        {
            isConnected = controller.checkNetworkConnection();
            if (isConnected)
            {
                if (controller.sendPendingNFCs())
                {
                    textBlockPendingNFCScans.Text = "Pending NFCs: " + 0;
                }

                if (controller.sendPendingAlarmReports())
                {
                    textBlockPendingAlarmReports.Text = "Pending Alarm Reports: " + 0;
                }
            }
        }

        private void buttonGemReport_Click(object sender, RoutedEventArgs e)
        {
            String check = "";
            String customerNameTB = textBoxCustomerName.Text;
            long customerNumberTB = 0;
            check = textBoxCustomerNumber.Text;
            if (!check.Equals(""))
             customerNumberTB = Convert.ToInt64(textBoxCustomerNumber.Text);
            String streetAndHouseNumberTB = textBoxStreetAndHouseNumber.Text;
            int zipCodeTB = 0;
            check = textBoxZipCode.Text;
            if (!check.Equals(""))
                zipCodeTB = Convert.ToInt32(textBoxZipCode.Text);
            String cityTB = textBoxCity.Text;
            long phonenumberTB =0;
            check = textBoxPhonenumber.Text;
            if (!check.Equals(""))
                phonenumberTB = Convert.ToInt64(textBoxPhonenumber.Text);
            DateTime dateTB = (DateTime)textBoxDate.Value;
            DateTime timeTB = (DateTime)textBoxTime.Value;
            String zoneTB = textBoxZone.Text;
            Boolean burglaryVandalismCB = (Boolean)checkBoxBurglaryVandalism.IsChecked;
            Boolean windowDoorClosedCB = (Boolean)checkBoxWindowDoorClosed.IsChecked;
            Boolean apprehendedPersonCB = (Boolean)checkBoxApprehendedPerson.IsChecked;
            Boolean staffErrorCB = (Boolean)checkBoxStaffError.IsChecked;
            Boolean nothingToReportCB = (Boolean)checkBoxNothingToReport.IsChecked;
            Boolean technicalErrorCB = (Boolean)checkBoxTechnicalError.IsChecked;
            Boolean unknownReasonCB = (Boolean)checkBoxUnknownReason.IsChecked;
            Boolean otherCB = (Boolean)checkBoxOther.IsChecked;
            Boolean cancelDuringEmergencyCB = (Boolean)checkBoxCancelsDuringEmergency.IsChecked;
            Boolean coverMadeCB = (Boolean)checkBoxCoverMade.IsChecked;
            String remarkTB = textBoxRemark.Text;
            String nameTB = textBoxName.Text;
            String installerTB = textBoxInstaller.Text;
            String controlCenterTB = textBoxControlCenter.Text;
            DateTime guardRadioedDateTB = (DateTime)textBoxGuardRadioedDate.Value;
            DateTime guardRadioedFromTB = (DateTime)textBoxGuardRadioedFrom.Value;
            DateTime guardRadioedToTB = (DateTime)textBoxGuardRadioedTo.Value;
            DateTime arrivedAtTB = (DateTime)textBoxArrivedAt.Value;
            DateTime doneTB = (DateTime)textBoxDone.Value;
            if (controller.createTempAlarmReport(new AlarmReport(customerNameTB, customerNumberTB, streetAndHouseNumberTB, zipCodeTB, cityTB, 
                                        phonenumberTB, dateTB, timeTB, zoneTB, burglaryVandalismCB, windowDoorClosedCB, apprehendedPersonCB, 
                                        staffErrorCB, nothingToReportCB, technicalErrorCB, unknownReasonCB, otherCB, cancelDuringEmergencyCB, 
                                        coverMadeCB, remarkTB, nameTB, installerTB, controlCenterTB, guardRadioedDateTB, guardRadioedFromTB, 
                                        guardRadioedToTB, arrivedAtTB, doneTB, controller.getUser())))
            {
                MessageBox.Show(AppResources.ReportAlarmReportLocalStorageSuccess);
                emptyAlarmReport();
            }
            else
            {
                MessageBox.Show(AppResources.ReportAlarmReportLocalStorageFailed);
            }

        }

        private async void searchForCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            long customerNumber = Convert.ToInt64(textBoxCustomerNumber.Text);
            Customer customer = await controller.getCustomer(customerNumber);
            if (!object.ReferenceEquals(customer, null))
            {
                textBoxCustomerName.Text = customer.CustomerName;
                textBoxStreetAndHouseNumber.Text = customer.StreetHouseNumber;
                textBoxZipCode.Text = customer.ZipCode + "";
                textBoxCity.Text = customer.City;
                textBoxPhonenumber.Text = customer.Phonenumber + "";
            } else
            {
                MessageBox.Show(AppResources.ReportCustomerNotFound);
            }
        }
    }
}