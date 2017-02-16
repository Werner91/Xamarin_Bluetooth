namespace BluetoothCheck
{
   using System.Collections.ObjectModel;

   public interface IBth
   {
      void Start(string name, int sleeptime, bool readAsCharArray);

      void Cancel();

      ObservableCollection<string> PairedDevices();
   }
}