namespace BluetoothCheck
{
   using System;
   using System.Collections.ObjectModel;
   using System.Windows.Input;

   using Xamarin.Forms;

   public partial class MainPage : ContentPage
   {
      public MainPage()
      {
         this.InitializeComponent();

         this.ConnectCommand = new Command(
                                  () =>
                                     {
                                        // Try to connect to a bth device
                                        DependencyService.Get<IBth>().Start(this.SelectedBthDevice, this._sleepTime, true);
                                        this._isConnected = true;
                                     });

         this.DisconnecteCommand = new Command(
                                      () =>
                                         {
                                            // Disconnect from bth device
                                            DependencyService.Get<IBth>().Cancel();
                                            this._isConnected = false;
                                         });

         // load all paired devices
         try
         {
            this.ListOfDevices = DependencyService.Get<IBth>().PairedDevices();
         }
         catch (Exception ex)
         {
            Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "ok");
         }
      }

      public ObservableCollection<string> ListOfDevices { get; set; } = new ObservableCollection<string>();

      public string SelectedBthDevice { get; set; } = string.Empty;

      public ICommand ConnectCommand { get; protected set; }

      public ICommand DisconnecteCommand { get; protected set; }

      private int _sleepTime { get; set; } = 250;

      private bool _isConnected { get; set; } = false;

      private void Cancle_OnClicked(object sender, EventArgs e)
      {
         this.ActivityIndicator.IsRunning = false;
         this.ActivityIndicator.IsVisible = false;
      }

      private void Connect_OnClicked(object sender, EventArgs e)
      {
         this.ActivityIndicator.IsVisible = true;
         this.ActivityIndicator.IsRunning = true;
      }

      private void GetDevices_OnClicked(object sender, EventArgs e)
      {
         foreach (string element in this.ListOfDevices)
         {
            this.pickerBluetoothDeives.Items.Add(element);
         }
      }
   }
}