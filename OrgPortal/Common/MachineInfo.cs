using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace OrgPortal.Common
{
    public class MachineInfo
    {
        public MachineInfo()
        {

        }

        public static async Task<Dictionary<string, string>> GatherMachineInfo()
        {
            var windowsVersion = await WindowsStoreSystemInformation.GetWindowsVersionAsync();
            var processor = await WindowsStoreSystemInformation.GetProcessorArchitectureAsync();
            var deviceCategory = await WindowsStoreSystemInformation.GetDeviceCategoryAsync();
            var deviceManufacturer = await WindowsStoreSystemInformation.GetDeviceManufacturerAsync();
            var deviceModel = await WindowsStoreSystemInformation.GetDeviceModelAsync();
            var osInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            string machineName = osInfo.FriendlyName;
            string operatingSystem = osInfo.OperatingSystem;
            string systemManufacturer = osInfo.SystemManufacturer;
            string systemProductName = osInfo.SystemProductName;
            string systemSku = osInfo.SystemSku;
            string systemId = osInfo.Id.ToString();

            var _hwId = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null).Id;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(_hwId);
            byte[] bytes = new byte[_hwId.Length];
            dataReader.ReadBytes(bytes);
            var HardwareId = BitConverter.ToString(bytes);

            Dictionary<string, string> retValue = new Dictionary<string,string>();
            retValue.Add("machineName",        osInfo.FriendlyName);
            retValue.Add("operatingSystem",    osInfo.OperatingSystem);
            retValue.Add("systemManufacturer", osInfo.SystemManufacturer);
            retValue.Add("systemProductName",  osInfo.SystemProductName);
            retValue.Add("systemSku",          osInfo.SystemSku);
            retValue.Add("systemId",           osInfo.Id.ToString("D"));
            retValue.Add("windowsVersion",     windowsVersion);
            retValue.Add("deviceCategory",     deviceCategory);
            retValue.Add("deviceManufacturer", deviceManufacturer);
            retValue.Add("deviceModel",        deviceModel);
            retValue.Add("HardwareId",         HardwareId);

            return retValue;
          

        }

        public static async Task<string> GatherMachineInfoLoop()
        {
            string retValue = string.Empty;
            foreach (var item in await GatherMachineInfo())
            {
                retValue += item.Key
                         +  System.Environment.NewLine
                         +  item.Value;
            }
            return retValue;
        }


        public static async Task<string[]> GatherMachineInfoArray()
        {
            string[] retValue = new string[0];
            foreach (var item in await GatherMachineInfo())
            {
            Array.Resize(ref retValue, retValue.Length + 1);
            retValue[retValue.Length - 1] = item.Key;

            Array.Resize(ref retValue, retValue.Length + 1);
            retValue[retValue.Length - 1] = item.Value;
            }
            return retValue;
        }

        public static async Task<string> GatherMachineInfoDisplay()
        {
            #region gather machine info

            var _hwId = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null).Id;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(_hwId);
            byte[] bytes = new byte[_hwId.Length];
            dataReader.ReadBytes(bytes);
            var HardwareId = BitConverter.ToString(bytes);


            var windowsVersion = await WindowsStoreSystemInformation.GetWindowsVersionAsync();
            var processor = await WindowsStoreSystemInformation.GetProcessorArchitectureAsync();
            var deviceCategory = await WindowsStoreSystemInformation.GetDeviceCategoryAsync();
            var deviceManufacturer = await WindowsStoreSystemInformation.GetDeviceManufacturerAsync();
            var deviceModel = await WindowsStoreSystemInformation.GetDeviceModelAsync();
            var osInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();
            string machineName = osInfo.FriendlyName;
            string operatingSystem = osInfo.OperatingSystem;
            string systemManufacturer = osInfo.SystemManufacturer;
            string systemProductName = osInfo.SystemProductName;
            string systemSku = osInfo.SystemSku;
            string systemId = osInfo.Id.ToString();
            string deviceInfo =
                string.Format(
@"
[Hardware Info]
HwId: {7}
Data/Hora: {0}
operatingSystem: {8}
systemManufacturer: {9}
systemProductName: {10}
systemSku: {11}
systemId: {12}
windowsVersion: {1}
processor: {2}
deviceCategory: {3}
deviceManufacturer: {4}
deviceModel: {5}
machineName: {6}

",
                    DateTime.Now.ToString(), // 00
                    windowsVersion,  // 01
                    processor, // 02
                    deviceCategory, // 03 
                    deviceManufacturer,  // 04
                    deviceModel, // 05
                    machineName, // 06
                    HardwareId, // 07
                    operatingSystem, // 08
                    systemManufacturer, // 09
                    systemProductName, // 10
                    systemSku, // 11
                    systemId // 12
                );
            //ExceptionLogger.
            #endregion
            Debug.WriteLine("deviceInfo = " + deviceInfo);

            return deviceInfo;
        }

        public string HardwareId
        {
            get
            {
                var _hwId = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null).Id;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(_hwId);

                byte[] bytes = new byte[_hwId.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes);
            }
        }

        private string HardwareSignature
        {
            get
            {
                var _hwSignature = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null).Signature;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(_hwSignature);

                byte[] bytes = new byte[_hwSignature.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes);
            }
        }

        private string HardwareCertificate
        {
            get
            {
                var _hwCertificate = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null).Certificate;
                var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(_hwCertificate);

                byte[] bytes = new byte[_hwCertificate.Length];
                dataReader.ReadBytes(bytes);

                return BitConverter.ToString(bytes);
            }
        }

        public bool Internet
        {
            get
            {
                var _internet = NetworkInformation.GetInternetConnectionProfile();
                return _internet == null ? false : _internet.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
        }

    }
}
