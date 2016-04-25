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
        private Boolean isAlarmReportEmpty = true;

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

        public Boolean checkIfAlarmReportIsEmpty()
        {
            Boolean check = false;
            if (!textBoxCustomerName.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxCustomerNumber.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxStreetAndHouseNumber.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxZipCode.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxCity.Text.Equals(""))
            {
                return false;
            }

            if (!textBoxPhonenumber.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxZone.Text.Equals(""))
            {
                return false;
            }

            if ((Boolean)checkBoxBurglaryVandalism.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxWindowDoorClosed.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxApprehendedPerson.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxStaffError.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxNothingToReport.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxTechnicalError.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxUnknownReason.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxOther.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxCancelsDuringEmergency.IsChecked)
            {
                return false;
            }
            if ((Boolean)checkBoxCoverMade.IsChecked)
            {
                return false;
            }
            if (!textBoxCoverMadeBy.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxRemark.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxInstaller.Text.Equals(""))
            {
                return false;
            }
            if (!textBoxControlCenter.Text.Equals(""))
            {
                return false;
            }
            return true;
        }


        public async Task<Boolean> checkAlarmReport()
        {
            Boolean check = true;
            Boolean isConnected = controller.checkNetworkConnection();
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
            DateTime date = (DateTime)textBoxDate.Value;
            String dateTB = date.ToString("yyyy-MM-dd");
            DateTime time = (DateTime)textBoxTime.Value;
            String timeTB = time.ToString("H:mm:ss");
            String zoneTB = textBoxZone.Text;

            Boolean burglaryVandalismCB = (Boolean)checkBoxBurglaryVandalism.IsChecked;
            Boolean windowDoorClosedCB = (Boolean)checkBoxWindowDoorClosed.IsChecked;
            Boolean apprehendedPersonCB = (Boolean)checkBoxApprehendedPerson.IsChecked;
            Boolean staffErrorCB = (Boolean)checkBoxStaffError.IsChecked;
            Boolean nothingToReportCB = (Boolean)checkBoxNothingToReport.IsChecked;
            Boolean technicalErrorCB = (Boolean)checkBoxTechnicalError.IsChecked;
            Boolean unknownReasonCB = (Boolean)checkBoxUnknownReason.IsChecked;
            Boolean otherCB = (Boolean)checkBoxOther.IsChecked;
            String reasonCodeId = "000";
            Boolean cancelDuringEmergencyCB = (Boolean)checkBoxCancelsDuringEmergency.IsChecked;
            String cancelDuringEmergencyTimeTP = null;
            if (cancelDuringEmergencyCB)
            {
                if (timeBoxCanceledDuringEmergencyTime.Value.Equals(null))
                {
                    check = false;
                } else
                {
                    DateTime cancelDuringEmergencyTime = (DateTime)timeBoxCanceledDuringEmergencyTime.Value;
                    cancelDuringEmergencyTimeTP = cancelDuringEmergencyTime.ToString("H:mm:ss");
                }
            }
            Boolean coverMadeCB = (Boolean)checkBoxCoverMade.IsChecked;
            String coverMadeByTB = textBoxCoverMadeBy.Text;
            if (coverMadeCB)
            {
                if (coverMadeByTB.Equals(""))
                {
                    check = false;
                }
            }

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
            DateTime guardRadioedDate = (DateTime)textBoxGuardRadioedDate.Value;
            String guardRadioedDateTB = guardRadioedDate.ToString("yyyy-MM-dd");
            DateTime guardRadioedFrom = (DateTime)textBoxGuardRadioedFrom.Value;
            String guardRadioedFromTB = guardRadioedFrom.ToString("H:mm:ss");
            DateTime guardRadioedTo = (DateTime)textBoxGuardRadioedTo.Value;
            String guardRadioedToTB = guardRadioedTo.ToString("H:mm:ss");
            DateTime arrivedAt = (DateTime)textBoxArrivedAt.Value;
            String arrivedAtTB = arrivedAt.ToString("H:mm:ss");
            DateTime done = (DateTime)textBoxDone.Value;
            String doneTB = done.ToString("H:mm:ss");
            if (check)
            {
                AlarmReport alarmReport = new AlarmReport(customerNameTB, customerNumberTB, streetAndHouseNumberTB, zipCodeTB, cityTB, phonenumberTB, dateTB, timeTB, zoneTB, burglaryVandalismCB,
                                            windowDoorClosedCB, apprehendedPersonCB, staffErrorCB, nothingToReportCB, technicalErrorCB, unknownReasonCB, otherCB, reasonCodeId, cancelDuringEmergencyCB, cancelDuringEmergencyTimeTP,
                                            coverMadeCB, coverMadeByTB, remarkTB, nameTB, installerTB, controlCenterTB, guardRadioedDateTB, guardRadioedFromTB, guardRadioedToTB, arrivedAtTB, doneTB, controller.getUser().Id);
                if (isConnected)
                {
                    if (await controller.createAlarmReport(alarmReport))
                    {
                        emptyAlarmReport();
                        isAlarmReportEmpty = true;
                        MessageBox.Show(AppResources.ReportAlarmReportSuccess);
                    }
                    else
                    {
                        isAlarmReportEmpty = false;
                        if (controller.createLocalStorageAlarmReport(alarmReport))
                        {
                            emptyAlarmReport();
                            isAlarmReportEmpty = true;
                            MessageBox.Show(AppResources.ErrorInBackend + "\r\n" + AppResources.ReportAlarmReportLocalStorageSuccess);
                        }
                        else
                        {
                            isAlarmReportEmpty = false;
                            MessageBox.Show(AppResources.ReportAlarmReportLocalStorageFailed);
                        }
                    }
                }
                else
                {
                    if (controller.createLocalStorageAlarmReport(alarmReport))
                    {
                        emptyAlarmReport();
                        isAlarmReportEmpty = true;
                        MessageBox.Show(AppResources.ReportAlarmReportLocalStorageSuccess);
                    }
                    else
                    {
                        isAlarmReportEmpty = false;
                        MessageBox.Show(AppResources.ReportAlarmReportLocalStorageFailed);
                    }
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
            Boolean check = await checkAlarmReport();
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
            Debug.WriteLine("isConnected " + isConnected);
            String address = await controller.onLocationScan(tagAddress, isConnected);
            if(address == null)
            {
                textBlockNFCScanInformation.Text = AppResources.ErrorNFCTagAddress;
            } else
            {
                long number;
                if(!Int64.TryParse(address, out number))
                {
                    textBlockNFCScanInformation.Text = AppResources.ScanNFCTagAddress + ": " + address + "\r\n" + AppResources.ScanNFCScanTime + ": " + DateTime.Now;
                } else
                {
                    textBlockNFCScanInformation.Text = AppResources.ScanNFCTagAddress + ": " + address + "\r\n" + AppResources.ScanNFCScanTime + ": " + DateTime.Now + "\r\n" + AppResources.ScanNFCTempStorage;
                }
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    if (checkIfAlarmReportIsEmpty())
                    {
                        updateAlarmReportDateTime();
                    }
                } else if(pivot.SelectedIndex == 3)
                {
                    device.StopSubscribingForMessage(deviceId);
                    int nfcs = controller.getLocalStorageNFCs();
                    int alarmReports = controller.getLocalStorageAlarmReports();
                    int customers = controller.getLocalStorageCustomers();
                    textBlockPendingNFCScans.Text = AppResources.PendingNFC + " " + nfcs;
                    textBlockPendingAlarmReports.Text = AppResources.PendingAlarmReports + " " + alarmReports;
                    textBlockPendingCustomers.Text = AppResources.PendingCustomers + " " + customers;
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
            textBoxDate.Value = Convert.ToDateTime(alarmReport.Date);
            textBoxTime.Value = Convert.ToDateTime(alarmReport.Time);
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
            {
                checkBoxCancelsDuringEmergency.IsChecked =true;
                timeBoxCanceledDuringEmergencyTime.Value = Convert.ToDateTime(alarmReport.CancelDuringEmergencyTime);
            }
            else
            {
                timeBoxCanceledDuringEmergencyTime.Value = null;
            }
            if (alarmReport.CoverMade)
            {
                checkBoxCoverMade.IsChecked=true;
                textBoxCoverMadeBy.Text = alarmReport.CoverMadeBy;
            } else
            {
                textBoxCoverMadeBy.Text = alarmReport.CoverMadeBy;
            }
            textBoxRemark.Text = alarmReport.Remark;
            textBoxName.Text = alarmReport.Name;
            textBoxInstaller.Text = alarmReport.Installer;
            textBoxControlCenter.Text = alarmReport.ControlCenter;
            textBoxGuardRadioedDate.Value = Convert.ToDateTime(alarmReport.GuardRadioedDate);
            textBoxGuardRadioedFrom.Value = Convert.ToDateTime(alarmReport.GuardRadioedFrom);
            textBoxGuardRadioedTo.Value = Convert.ToDateTime(alarmReport.GuardRadioedTo);
            textBoxArrivedAt.Value = Convert.ToDateTime(alarmReport.ArrivedAt);
            textBoxDone.Value = Convert.ToDateTime(alarmReport.Done);
        }

        private async void sendPendingButton_Click(object sender, RoutedEventArgs e)
        {
            isConnected = controller.checkNetworkConnection();
            if (isConnected)
            {
                if (await controller.sendPendingNFCs())
                {
                    textBlockPendingNFCScans.Text = AppResources.PendingNFC + " " + 0;
                }

                if (await controller.sendPendingAlarmReports())
                {
                    textBlockPendingAlarmReports.Text = AppResources.PendingAlarmReports + " " + 0;
                }

                if (await controller.sendPendingCustomers())
                {
                    textBlockPendingCustomers.Text = AppResources.PendingCustomers + " " + 0;
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
            DateTime date = (DateTime)textBoxDate.Value;
            String dateTB = date.ToString("yyyy-MM-dd");
            DateTime time = (DateTime)textBoxTime.Value;
            String timeTB = time.ToString("H:mm:ss");
            String zoneTB = textBoxZone.Text;
            Boolean burglaryVandalismCB = (Boolean)checkBoxBurglaryVandalism.IsChecked;
            Boolean windowDoorClosedCB = (Boolean)checkBoxWindowDoorClosed.IsChecked;
            Boolean apprehendedPersonCB = (Boolean)checkBoxApprehendedPerson.IsChecked;
            Boolean staffErrorCB = (Boolean)checkBoxStaffError.IsChecked;
            Boolean nothingToReportCB = (Boolean)checkBoxNothingToReport.IsChecked;
            Boolean technicalErrorCB = (Boolean)checkBoxTechnicalError.IsChecked;
            Boolean unknownReasonCB = (Boolean)checkBoxUnknownReason.IsChecked;
            Boolean otherCB = (Boolean)checkBoxOther.IsChecked;
            String reasonCodeId = "000";
            Boolean cancelDuringEmergencyCB = (Boolean)checkBoxCancelsDuringEmergency.IsChecked;
            String cancelDuringEmergencyTimeTP = null;
            if (cancelDuringEmergencyCB)
            {
                DateTime cancelDuringEmergencyTime = (DateTime)timeBoxCanceledDuringEmergencyTime.Value;
                cancelDuringEmergencyTimeTP = cancelDuringEmergencyTime.ToString("h:mm:ss");
            } 
            Boolean coverMadeCB = (Boolean)checkBoxCoverMade.IsChecked;
            String coverMadeByTB = textBoxCoverMadeBy.Text;
            String remarkTB = textBoxRemark.Text;
            String nameTB = textBoxName.Text;
            String installerTB = textBoxInstaller.Text;
            String controlCenterTB = textBoxControlCenter.Text;
            DateTime guardRadioedDate = (DateTime)textBoxGuardRadioedDate.Value;
            String guardRadioedDateTB = guardRadioedDate.ToString("yyyy-MM-dd");
            DateTime guardRadioedFrom = (DateTime)textBoxGuardRadioedFrom.Value;
            String guardRadioedFromTB = guardRadioedFrom.ToString("H:mm:ss");
            DateTime guardRadioedTo = (DateTime)textBoxGuardRadioedTo.Value;
            String guardRadioedToTB = guardRadioedTo.ToString("H:mm:ss");
            DateTime arrivedAt = (DateTime)textBoxArrivedAt.Value;
            String arrivedAtTB = arrivedAt.ToString("H:mm:ss");
            DateTime done = (DateTime)textBoxDone.Value;
            String doneTB = done.ToString("H:mm:ss");
            if (controller.createTempAlarmReport(new AlarmReport(customerNameTB, customerNumberTB, streetAndHouseNumberTB, zipCodeTB, cityTB, 
                                        phonenumberTB, dateTB, timeTB, zoneTB, burglaryVandalismCB, windowDoorClosedCB, apprehendedPersonCB, 
                                        staffErrorCB, nothingToReportCB, technicalErrorCB, unknownReasonCB, otherCB, reasonCodeId, cancelDuringEmergencyCB,
                                        cancelDuringEmergencyTimeTP, coverMadeCB, coverMadeByTB, remarkTB, nameTB, installerTB, controlCenterTB, 
                                        guardRadioedDateTB, guardRadioedFromTB, guardRadioedToTB, arrivedAtTB, doneTB, controller.getUser().Id)))
            {
                MessageBox.Show(AppResources.ReportAlarmReportLocalStorageSuccess);
                emptyAlarmReport();
                isAlarmReportEmpty = true;
            }
            else
            {
                isAlarmReportEmpty = false;
                MessageBox.Show(AppResources.ReportAlarmReportLocalStorageFailed);
            }

        }

        private async void searchForCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (controller.checkNetworkConnection())
            { 
                long customerNumber = Convert.ToInt64(textBoxCustomerNumber.Text);
                Customer customer = await controller.getCustomer(customerNumber);
                if (!object.ReferenceEquals(customer, null))
                {
                    textBoxCustomerName.Text = customer.CustomerName;
                    textBoxStreetAndHouseNumber.Text = customer.StreetAndHouseNumber;
                    textBoxZipCode.Text = customer.ZipCode + "";
                    textBoxCity.Text = customer.City;
                    textBoxPhonenumber.Text = customer.Phonenumber + "";
                } else
                {
                    MessageBox.Show(AppResources.ReportCustomerNotFound);
                }
            } else
            {
                MessageBox.Show(AppResources.NoNetWorkConnection);
            }
        }

        private async void createCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            Boolean isConnected = controller.checkNetworkConnection();
            if (isConnected)
            {
                customer = await controller.getCustomer(Convert.ToInt64(textBoxCreateCustomerNumber.Text));
            }

            if(customer == null)
            {
                Boolean check = await checkCreateCustomer(isConnected);
                if (!check)
                {
                    MessageBox.Show(AppResources.CreateCustomerFill);
                }
            } else
            {
                MessageBox.Show(AppResources.ErrorCreateCustomerExist);
            }
        }

        public async Task<Boolean> checkCreateCustomer(Boolean isConnected)
        {
            Boolean check = true;
            if (textBoxCreateCustomerName.Text.Equals(""))
            {
                check = false;
            }
            String customerNameTB = textBoxCreateCustomerName.Text;
            long customerNumberTB = 0;
            if (!textBoxCreateCustomerNumber.Text.Equals(""))
            {
                customerNumberTB = Convert.ToInt64(textBoxCreateCustomerNumber.Text);
            }
            else
            {
                check = false;
            }
            if (textBoxCreateCustomerStreetAndHouseNumber.Text.Equals(""))
            {
                check = false;
            }
            String streetAndHouseNumberTB = textBoxCreateCustomerStreetAndHouseNumber.Text;
            int zipCodeTB = 0;
            if (!textBoxCreateCustomerZipCode.Text.Equals(""))
            {
                zipCodeTB = Convert.ToInt32(textBoxCreateCustomerZipCode.Text);
            }
            else
            {
                check = false;
            }
            if (textBoxCreateCustomerCity.Text.Equals(""))
            {
                check = false;
            }
            String cityTB = textBoxCreateCustomerCity.Text;
            long phonenumberTB = 0;
            if (!textBoxCreateCustomerPhonenumber.Text.Equals(""))
            {
                phonenumberTB = Convert.ToInt64(textBoxCreateCustomerPhonenumber.Text);
            }
            if (check)
            {
                Customer customer = new Customer(customerNameTB, customerNumberTB, streetAndHouseNumberTB, zipCodeTB, cityTB, phonenumberTB);
                if (isConnected) { 
                    if (await controller.createCustomer(customer))
                    {
                        emptyCreateCustomer();
                        MessageBox.Show(AppResources.CreateCustomerSuccess);
                    }
                    else
                    {
                        if (controller.createLocalStorageCustomer(customer))
                        {
                            emptyCreateCustomer();
                            MessageBox.Show(AppResources.NoNetWorkConnection + "\r\n" + AppResources.CreateCustomerLocalSuccess);
                        }
                        else
                        {
                            MessageBox.Show(AppResources.CreateCustomerError);
                        }
                    }
                } else
                {
                    if(controller.createLocalStorageCustomer(customer))
                    {
                        emptyCreateCustomer();
                        MessageBox.Show(AppResources.CreateCustomerLocalSuccess);
                    } else
                    {
                        MessageBox.Show(AppResources.CreateCustomerError);
                    }
                }
            }

            return check;
        }

        public void emptyCreateCustomer()
        {
            textBoxCreateCustomerName.Text = "";
            textBoxCreateCustomerNumber.Text = "";
            textBoxCreateCustomerStreetAndHouseNumber.Text = "";
            textBoxCreateCustomerZipCode.Text = "";
            textBoxCreateCustomerCity.Text = "";
            textBoxCreateCustomerPhonenumber.Text = "";
        }

        private void checkBoxCancelsDuringEmergency_Click(object sender, RoutedEventArgs e)
        {
            Boolean check = (Boolean) checkBoxCancelsDuringEmergency.IsChecked;
            if (check)
            {
                timeBoxCanceledDuringEmergencyTime.Value = null;
                timeBoxCanceledDuringEmergencyTime.Visibility = Visibility.Visible;
            } else
            {
                timeBoxCanceledDuringEmergencyTime.Visibility = Visibility.Collapsed;
            }
        }

        private void checkBoxCoverMade_Click(object sender, RoutedEventArgs e)
        {
            Boolean check = (Boolean)checkBoxCoverMade.IsChecked;
            if (check)
            {
                textBoxCoverMadeBy.Visibility = Visibility.Visible;
            } else
            {
                textBoxCoverMadeBy.Visibility = Visibility.Collapsed;
            }
        }
    }
}