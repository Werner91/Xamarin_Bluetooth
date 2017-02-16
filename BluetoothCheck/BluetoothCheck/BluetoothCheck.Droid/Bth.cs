namespace BluetoothCheck.Droid
{
   using System.Collections.ObjectModel;
   using System.Threading;
   using System.Threading.Tasks;

   using Android.Bluetooth;

   class Bth : IBth
   {
      private const int RequestResolveError = 1000;

      public Bth()
      {
      }

      private CancellationTokenSource _ct { get; set; }

      // start the "reading" loop
      public void Start(string name, int sleeptime = 200, bool readAsCharArray = false)
      {
         Task.Run(async () => this.loop(name, sleeptime, readAsCharArray));
      }

      // cancel the raeding loop
      public void Cancel()
      {
         if (this._ct != null)
         {
            System.Diagnostics.Debug.WriteLine("Send a cancel to task");
            this._ct.Cancel();
         }
      }

      public ObservableCollection<string> PairedDevices()
      {
         BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
         ObservableCollection<string> devices = new ObservableCollection<string>();

         foreach (var bd in adapter.BondedDevices)
         {
            devices.Add(bd.Name);
         }

         return devices;
      }

      private async Task loop(string name, int sleepTime, bool readAsCharArray)
      {
         BluetoothDevice device = null;
         BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
         BluetoothSocket BthSocket = null;

         this._ct = new CancellationTokenSource();
         while (this._ct.IsCancellationRequested == false)
         {
            try
            {
               Thread.Sleep(sleepTime);
               adapter = BluetoothAdapter.DefaultAdapter;
               if (adapter == null)
               {
                  System.Diagnostics.Debug.WriteLine("No Bluetooth adapter found.");
               }
               else
               {
                  System.Diagnostics.Debug.WriteLine("Adapter found!");
               }

               if (!adapter.IsEnabled)
               {
                  System.Diagnostics.Debug.WriteLine("Bluetooth adapter is not enabled.");
               }
               else
               {
                  System.Diagnostics.Debug.WriteLine("Adapter Enabled");
               }

               System.Diagnostics.Debug.WriteLine("Try to connect to " + name);

               foreach (var bd in adapter.BondedDevices)
               {
                  System.Diagnostics.Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
                  if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                  {
                     System.Diagnostics.Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
                     device = bd;
                     break;
                  }
               }

               if (device == null)
               {
                  System.Diagnostics.Debug.WriteLine("Named device not found");
               }
               else
               {
                  System.Diagnostics.Debug.WriteLine("Here happens the barcode scanning magic.");
               }
            }
            catch
            {
            }
            finally
            {
               if (BthSocket != null)
               {
                  BthSocket.Close();
               }

               device = null;
               adapter = null;
            }
         }

         System.Diagnostics.Debug.WriteLine("Exit the external loop");
      }
   }
}