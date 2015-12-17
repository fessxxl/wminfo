﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace wminfo.Lib
{
    static class Crawler
    {
        public static Computer GetInfo(string targetHost, string username, string password, string[] categories)
        {
            Computer result = new Computer();
            if (targetHost == null) return null;
            ConnectionOptions options = new ConnectionOptions();
            if(username != null && password != null)
            {
                options.Username = username;
                options.Password = password;
            }
            ManagementScope scope = new ManagementScope("\\\\" + targetHost + "\\root\\CIMV2",options);
            scope.Connect();

            if(categories.Contains("os")) result.OperatingSystems = Get_OperatingSystems(scope);
            if (categories.Contains("software")) result.SoftwareProducts = Get_SoftwareProducts(scope);
            if (categories.Contains("processor")) result.Processors = Get_Processors(scope);
            if (categories.Contains("cachememory")) result.CacheMemory = Get_CacheMemory(scope);
            if (categories.Contains("ram")) result.Memory = Get_Memory(scope);
            if (categories.Contains("video")) result.VideoControllers = Get_VideoControllers(scope);
            if (categories.Contains("mb")) result.ComputerSystem = Get_ComputerSystem(scope);
            if (categories.Contains("storage")) {
                result.HardDrives = Get_HardDrives(scope);
                result.CDDrives = Get_CDDrives(scope);
                result.LogicalVolumes = Get_LogicalVolumes(scope);
            }
            if (categories.Contains("devices")) result.PnPDevices = Get_PnPDevices(scope);
            if (categories.Contains("network")) result.NetworkAdapters = Get_NetworkAdapters(scope);
            if (categories.Contains("audio")) result.SoundDevices = Get_SoundDevices(scope);

            return result;
        }

        public static List<OperatingSystem> Get_OperatingSystems(ManagementScope scope)
        {
            List<OperatingSystem> result = new List<OperatingSystem>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var os = new OperatingSystem();
                if(queryObj["CSDVersion"] != null) os.ServicePack = queryObj["CSDVersion"].ToString().Trim(' ');
                os.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                os.Version = queryObj["Version"].ToString().Trim(' ');
                os.Architecture = queryObj["OSArchitecture"].ToString().Trim(' ');
                os.RegisteredUser = queryObj["RegisteredUser"].ToString().Trim(' ');
                os.Organization = queryObj["Organization"].ToString().Trim(' ');
                os.CodeSet = queryObj["CodeSet"].ToString().Trim(' ');
                os.CountryCode = queryObj["CountryCode"].ToString().Trim(' ');
                os.CurrentTimeZone = queryObj["CurrentTimeZone"].ToString().Trim(' ');
                os.EncryptionLevel = queryObj["EncryptionLevel"].ToString().Trim(' ');
                os.ForegroundApplicationBoost = queryObj["ForegroundApplicationBoost"].ToString().Trim(' ');
                os.InstallDate = queryObj["InstallDate"].ToString().Trim(' ');
                os.LastBootupTime = queryObj["LastBootUpTime"].ToString().Trim(' ');
                os.LocalDateTime = queryObj["LocalDateTime"].ToString().Trim(' ');
                os.Locale = queryObj["Locale"].ToString().Trim(' ');
                os.OSLanguage = queryObj["OSLanguage"].ToString().Trim(' ');
                os.OSType = queryObj["OSType"].ToString().Trim(' ');
                os.ProductType = Convert.ToInt32(queryObj["ProductType"].ToString().Trim(' '));
                os.ProductID = queryObj["SerialNumber"].ToString().Trim(' ');
                os.SystemDrive = queryObj["SystemDrive"].ToString().Trim(' ');
                os.WindowsDirectory = queryObj["WindowsDirectory"].ToString().Trim(' ');
                os.SKU = queryObj["OperatingSystemSKU"].ToString().Trim(' ');
                os.ProductName = queryObj["Caption"].ToString().Trim(' ');
                result.Add(os);
            }

            return result;
        }

        public static List<SoftwareProduct> Get_SoftwareProducts (ManagementScope scope)
        {
            List<SoftwareProduct> result = new List<SoftwareProduct>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_Product");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var pr = new SoftwareProduct();
                if (queryObj["Version"] != null) pr.Version = queryObj["Version"].ToString().Trim(' ');
                if (queryObj["Vendor"] != null) pr.Vendor = queryObj["Vendor"].ToString().Trim(' ');
                if (queryObj["InstallLocation"] != null) pr.InstallLocation = queryObj["InstallLocation"].ToString().Trim(' ');
                if (queryObj["InstallSource"] != null) pr.InstallSource = queryObj["InstallSource"].ToString().Trim(' ');
                if (queryObj["InstallDate"] != null) pr.InstallDate = queryObj["InstallDate"].ToString().Trim(' ');
                if (queryObj["Name"] != null) pr.Name = queryObj["Name"].ToString().Trim(' ');
                if (queryObj["URLInfoAbout"] != null) pr.URLInfoAbout = queryObj["URLInfoAbout"].ToString().Trim(' ');
                result.Add(pr);
            }

            return result;
        }

        public static List<Processor> Get_Processors(ManagementScope scope)
        {
            List<Processor> result = new List<Processor>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var pr = new Processor();
                if (queryObj["CurrentClockSpeed"] != null) pr.CurrentClockSpeed = Convert.ToInt32(queryObj["CurrentClockSpeed"].ToString().Trim(' '));
                if (queryObj["MaxClockSpeed"] != null) pr.MaxClockSpeed = Convert.ToInt32(queryObj["MaxClockSpeed"].ToString().Trim(' '));
                if (queryObj["ExtClock"] != null) pr.ExtClock = Convert.ToInt32(queryObj["ExtClock"].ToString().Trim(' '));
                pr.Multiplier = pr.MaxClockSpeed / pr.ExtClock;
                if (queryObj["Description"] != null) pr.Description = queryObj["Description"].ToString().Trim(' ');
                if (queryObj["Manufacturer"] != null) pr.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                if (queryObj["Status"] != null) pr.Status = queryObj["Status"].ToString().Trim(' ');
                if (queryObj["Name"] != null) pr.Name = queryObj["Name"].ToString().Trim(' ');
                if (queryObj["SocketDesignation"] != null) pr.SocketDesignation = queryObj["SocketDesignation"].ToString().Trim(' ');
                if (queryObj["L2CacheSize"] != null) pr.L2CacheSize = Convert.ToInt32(queryObj["L2CacheSize"].ToString().Trim(' '));
                if (queryObj["L3CacheSize"] != null) pr.L3CacheSize = Convert.ToInt32(queryObj["L3CacheSize"].ToString().Trim(' '));
                if (queryObj["CurrentVoltage"] != null) pr.CurrentVoltage = Convert.ToSingle(queryObj["CurrentVoltage"].ToString().Trim(' ')) / 10;
                if (queryObj["UpgradeMethod"] != null) pr.UpgradeMethod = Convert.ToInt32(queryObj["UpgradeMethod"].ToString().Trim(' '));
                if (queryObj["NumberOfCores"] != null) pr.NumberOfCores = Convert.ToInt32(queryObj["NumberOfCores"].ToString().Trim(' '));
                if (queryObj["NumberOfLogicalProcessors"] != null) pr.NumberOfLogicalProcessors = Convert.ToInt32(queryObj["NumberOfLogicalProcessors"].ToString().Trim(' '));
                if (pr.NumberOfCores < pr.NumberOfLogicalProcessors) pr.Multithreaded = true;
                result.Add(pr);
            }

            return result;
        }

        public static List<CacheMemory> Get_CacheMemory(ManagementScope scope)
        {
            List<CacheMemory> result = new List<CacheMemory>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_CacheMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var pr = new CacheMemory();
                if (queryObj["InstalledSize"] != null) pr.Size = Convert.ToInt32(queryObj["InstalledSize"].ToString().Trim(' '));
                if (queryObj["CacheType"] != null) pr.Type = queryObj["CacheType"].ToString().Trim(' ');
                if (queryObj["Level"] != null) pr.Level = queryObj["Level"].ToString().Trim(' ');
                if (queryObj["Associativity"] != null) pr.Associativity = queryObj["Associativity"].ToString().Trim(' ');
                result.Add(pr);
            }

            return result;
        }

        public static Memory Get_Memory(ManagementScope scope)
        {
            Memory result = new Memory();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_PageFileUsage");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.PagefileSizeActual = Convert.ToInt32(queryObj["CurrentUsage"].ToString().Trim(' '));
                result.PagefileSizeMaximum = Convert.ToInt32(queryObj["AllocatedBaseSize"].ToString().Trim(' '));
            }

            wmiquery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            searcher = new ManagementObjectSearcher(scope, wmiquery);
            coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.VirtualMemorySize = Convert.ToInt32(queryObj["TotalVirtualMemorySize"].ToString().Trim(' '));
                result.TotalVisibleMemorySize = Convert.ToInt32(queryObj["TotalVisibleMemorySize"].ToString().Trim(' '))/1024;
                result.FreeMemorySize = Convert.ToInt32(queryObj["FreePhysicalMemory"].ToString().Trim(' '))/1024;
            }

            wmiquery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            searcher = new ManagementObjectSearcher(scope, wmiquery);
            coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.PhysicalMemorySize = (int)(Convert.ToInt64(queryObj["TotalPhysicalMemory"].ToString().Trim(' '))/1024/1024); //Нужно переделать. Не точное число физической памяти.
            }

            result.MemoryLoadPercentage = (int)((float)((float)result.PhysicalMemorySize / (float)result.FreeMemorySize) * 10);
            
            wmiquery = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory");
            searcher = new ManagementObjectSearcher(scope, wmiquery);
            coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                MemoryModule mm = new MemoryModule();
                mm.Capacity = (int)(Convert.ToInt64(queryObj["Capacity"].ToString().Trim(' ')) / 1024 / 1024);
                mm.BankLabel = queryObj["BankLabel"].ToString().Trim(' ');
                mm.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                if (queryObj["InstallDate"] != null) mm.ManufactureDate = queryObj["InstallDate"].ToString().Trim(' ');
                mm.MemoryType = queryObj["MemoryType"].ToString().Trim(' ');
                mm.FormFactor = queryObj["FormFactor"].ToString().Trim(' ');
                mm.PartNumber = queryObj["PartNumber"].ToString().Trim(' ');
                mm.SerialNumber = queryObj["SerialNumber"].ToString().Trim(' ');
                mm.Speed = queryObj["Speed"].ToString().Trim(' ');
                result.MemoryModules.Add(mm);
            }


            return result;
        }

        public static List<VideoController> Get_VideoControllers(ManagementScope scope)
        {
            List<VideoController> result = new List<VideoController>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new VideoController();
                cp.Availability = queryObj["Availability"].ToString().Trim(' ');
                cp.InstalledVideoRAM = queryObj["AdapterRAM"].ToString().Trim(' ');
                cp.DACType = queryObj["AdapterDACType"].ToString().Trim(' ');
                cp.AdapterFamily = queryObj["AdapterCompatibility"].ToString().Trim(' ');
                cp.AdapterName = queryObj["Name"].ToString().Trim(' ');
                cp.ScanMode = queryObj["CurrentScanMode"].ToString().Trim(' ');
                cp.VideoArchitecture = queryObj["VideoArchitecture"].ToString().Trim(' ');
                cp.VideoMemoryType = queryObj["VideoMemoryType"].ToString().Trim(' ');
                cp.CurrentVideoMode = queryObj["CurrentHorizontalResolution"].ToString().Trim(' ') + " x " + queryObj["CurrentVerticalResolution"].ToString().Trim(' ') + " x " + queryObj["CurrentBitsPerPixel"].ToString().Trim(' ') + "bpp x " + queryObj["CurrentRefreshRate"].ToString().Trim(' ') + " Hz";
                cp.DisplayDrivers = queryObj["InstalledDisplayDrivers"].ToString().Trim(' ');
                cp.DriverVersion = queryObj["DriverVersion"].ToString().Trim(' ');
                cp.DriverDate = queryObj["DriverDate"].ToString().Trim(' ');
                result.Add(cp);
            }

            return result;
        }

        public static ComputerSystem Get_ComputerSystem(ManagementScope scope)
        {
            ComputerSystem result = new ComputerSystem();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_BIOS");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.BIOS.Name = queryObj["Name"].ToString().Trim(' ');
                result.BIOS.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                result.BIOS.ReleaseDate = queryObj["ReleaseDate"].ToString().Trim(' ');
                result.BIOS.SerialNumber = queryObj["SerialNumber"].ToString().Trim(' ');
                result.BIOS.SMBIOSVersion = queryObj["SMBIOSBIOSVersion"].ToString().Trim(' ');
            }

            wmiquery = new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct");
            searcher = new ManagementObjectSearcher(scope, wmiquery);
            coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.Model = queryObj["Version"].ToString().Trim(' ');
                result.Manufacturer = queryObj["Vendor"].ToString().Trim(' ');
                result.UUID = queryObj["UUID"].ToString().Trim(' ');
                result.ProductNumber = queryObj["Name"].ToString().Trim(' ');
            }

            wmiquery = new ObjectQuery("SELECT * FROM Win32_SystemEnclosure");
            searcher = new ManagementObjectSearcher(scope, wmiquery);
            coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.Chassis.CaseType = ((ushort[])queryObj["ChassisTypes"])[0].ToString().Trim(' ');
                result.Chassis.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                result.Chassis.SerialNumber = queryObj["SerialNumber"].ToString().Trim(' ');
                result.Chassis.AssetTag = queryObj["SMBIOSAssetTag"].ToString().Trim(' ');
            }

            wmiquery = new ObjectQuery("SELECT * FROM Win32_BaseBoard");
            searcher = new ManagementObjectSearcher(scope, wmiquery);
            coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                result.Motherboard.Name = queryObj["Product"].ToString().Trim(' ');
                result.Motherboard.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                result.Motherboard.SerialNumber = queryObj["SerialNumber"].ToString().Trim(' ');
                result.Motherboard.Version = queryObj["Version"].ToString().Trim(' ');
            }

            return result;
        }

        public static List<HardDrive> Get_HardDrives(ManagementScope scope)
        {
            List<HardDrive> result = new List<HardDrive>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new HardDrive();
                cp.Model = queryObj["Model"].ToString().Trim(' ');
                cp.InterfaceType = queryObj["InterfaceType"].ToString().Trim(' ');
                cp.MediaType = queryObj["MediaType"].ToString().Trim(' ');
                cp.Size = ((int)(Convert.ToInt64(queryObj["Size"].ToString().Trim(' ')) / 1000 / 1000 / 1000)).ToString();
                cp.SerialNumber = queryObj["SerialNumber"].ToString().Trim(' ');
                cp.FirmwareRevision = queryObj["FirmwareRevision"].ToString().Trim(' ');
                result.Add(cp);
            }

            return result;
        }

        public static List<CDDrive> Get_CDDrives(ManagementScope scope)
        {
            List<CDDrive> result = new List<CDDrive>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_CDROMDrive");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new CDDrive();
                cp.Model = queryObj["Name"].ToString().Trim(' ');
                cp.DiskLetter = queryObj["Drive"].ToString().Trim(' ');
                cp.DriveType = queryObj["MediaType"].ToString().Trim(' ');
                if (queryObj["SerialNumber"] != null) cp.SerialNumber = queryObj["SerialNumber"].ToString().Trim(' ');
                result.Add(cp);
            }

            return result;
        }

        public static List<LogicalVolume> Get_LogicalVolumes(ManagementScope scope)
        {
            List<LogicalVolume> result = new List<LogicalVolume>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new LogicalVolume();
                cp.Description = queryObj["Description"].ToString().Trim(' ');
                cp.MediaType = queryObj["MediaType"].ToString().Trim(' ');
                cp.DriveType = queryObj["DriveType"].ToString().Trim(' ');
                cp.Name = queryObj["Name"].ToString().Trim(' ');
                if (queryObj["FileSystem"] != null) cp.FileSystem = queryObj["FileSystem"].ToString().Trim(' ');
                if (queryObj["Size"] != null) cp.Size = (int)(Convert.ToInt64(queryObj["Size"].ToString().Trim(' ')) / 1024 / 1024 / 1024);
                if (queryObj["FreeSpace"] != null) cp.Free = (int)(Convert.ToInt64(queryObj["FreeSpace"].ToString().Trim(' ')) / 1024 / 1024 / 1024);
                cp.Used = cp.Size - cp.Free;
                if (queryObj["VolumeName"] != null) cp.VolumeLabel = queryObj["VolumeName"].ToString().Trim(' ');
                if (queryObj["VolumeSerialNumber"] != null) cp.SerialNumber = queryObj["VolumeSerialNumber"].ToString().Trim(' ');
                if (queryObj["MaximumComponentLength"] != null) cp.MaxComponentLength = queryObj["MaximumComponentLength"].ToString().Trim(' ');
                if (queryObj["SupportsDiskQuotas"] != null) cp.SuportDiskQuotas = queryObj["SupportsDiskQuotas"].ToString().Trim(' ');
                if (queryObj["SupportsFileBasedCompression"] != null) cp.SupportFilebasedCompression = queryObj["SupportsFileBasedCompression"].ToString().Trim(' ');
                if (queryObj["Compressed"] != null) cp.Compressed = queryObj["Compressed"].ToString().Trim(' ');
                if (queryObj["ProviderName"] != null) cp.SerialNumber = queryObj["ProviderName"].ToString().Trim(' ');
                result.Add(cp);
            }

            return result;
        }

        public static List<PnPDevice> Get_PnPDevices(ManagementScope scope)
        {
            List<PnPDevice> result = new List<PnPDevice>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_PnPEntity");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new PnPDevice();
                if (queryObj["ClassGuid"] != null) cp.ClassGUID = queryObj["ClassGuid"].ToString().Trim(' ');
                if (queryObj["Manufacturer"] != null) cp.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                if (queryObj["PnPDeviceID"] != null) cp.PnPDeviceID = queryObj["PnPDeviceID"].ToString().Trim(' ');
                if (queryObj["Name"] != null) cp.Name = queryObj["Name"].ToString().Trim(' ');
                if (queryObj["ConfigManagerErrorCode"] != null) cp.ErrorCode = queryObj["ConfigManagerErrorCode"].ToString().Trim(' ');
                result.Add(cp);
            }

            return result;
        }

        public static List<NetworkAdapter> Get_NetworkAdapters(ManagementScope scope)
        {
            List<NetworkAdapter> result = new List<NetworkAdapter>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter = True");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new NetworkAdapter();
                if (queryObj["Name"] != null) cp.Name = queryObj["Name"].ToString().Trim(' ');
                if (queryObj["Manufacturer"] != null) cp.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                if (queryObj["ConfigManagerErrorCode"] != null) cp.ErrorCode = queryObj["ConfigManagerErrorCode"].ToString().Trim(' ');
                if (queryObj["MACAddress"] != null) cp.MACAddress = queryObj["MACAddress"].ToString().Trim(' ');
                if (queryObj["AdapterType"] != null) cp.AdapterType = queryObj["AdapterType"].ToString().Trim(' ');
                if (queryObj["NetConnectionID"] != null) cp.NetConnectionID = queryObj["NetConnectionID"].ToString().Trim(' ');
                if (queryObj["NetConnectionStatus"] != null) cp.NetConnectionStatus = queryObj["NetConnectionStatus"].ToString().Trim(' ');
                if (queryObj["Speed"] != null) cp.Speed = queryObj["Speed"].ToString().Trim(' ');
                if (queryObj["Availability"] != null) cp.Availability = queryObj["Availability"].ToString().Trim(' ');
                var index = queryObj["DeviceID"];
                ObjectQuery wmiquery2 = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = " + index);
                ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(scope, wmiquery2);
                ManagementObjectCollection coll2 = searcher2.Get();
                foreach (ManagementObject queryObj2 in coll2)
                {
                    if (queryObj2["IPAddress"] != null) cp.IPAddress = string.Join(", ", ((string[])queryObj2["IPAddress"]));
                    if (queryObj2["IPSubnet"] != null) cp.IPSubnet = string.Join(", ", ((string[])queryObj2["IPSubnet"]));
                    if (queryObj2["DefaultIPGateway"] != null) cp.DefaultIPGateway = string.Join(", ", ((string[])queryObj2["DefaultIPGateway"]));
                    if (queryObj2["DNSServerSearchOrder"] != null) cp.DNSServerOrder = string.Join(", ",((string[])queryObj2["DNSServerSearchOrder"]));
                    if (queryObj2["DHCPEnabled"] != null) cp.DHCPEnabled = queryObj2["DHCPEnabled"].ToString().Trim(' ');
                    if (queryObj2["DHCPServer"] != null) cp.DHCPServerName = queryObj2["DHCPServer"].ToString().Trim(' ');
                    if (queryObj2["DNSDomain"] != null) cp.DNSDomain = queryObj2["DNSDomain"].ToString().Trim(' ');
                    if (queryObj2["DNSHostName"] != null) cp.DNSHostName = queryObj2["DNSHostName"].ToString().Trim(' ');
                    if (queryObj2["TcpipNetbiosOptions"] != null) cp.NetBIOSParameters = queryObj2["TcpipNetbiosOptions"].ToString().Trim(' ');
                    if (queryObj2["IPFilterSecurityEnabled"] != null) cp.IPFilterEnabled = queryObj2["IPFilterSecurityEnabled"].ToString().Trim(' ');
                }
                result.Add(cp);
            }

            return result;
        }

        public static List<SoundDevice> Get_SoundDevices(ManagementScope scope)
        {
            List<SoundDevice> result = new List<SoundDevice>();

            ObjectQuery wmiquery = new ObjectQuery("SELECT * FROM Win32_SoundDevice");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, wmiquery);
            ManagementObjectCollection coll = searcher.Get();
            foreach (ManagementObject queryObj in coll)
            {
                var cp = new SoundDevice();
                if (queryObj["Name"] != null) cp.Name = queryObj["Name"].ToString().Trim(' ');
                if (queryObj["Manufacturer"] != null) cp.Manufacturer = queryObj["Manufacturer"].ToString().Trim(' ');
                if (queryObj["ConfigManagerErrorCode"] != null) cp.ErrorCode = queryObj["ConfigManagerErrorCode"].ToString().Trim(' ');
                result.Add(cp);
            }

            return result;
        }
    }
}
