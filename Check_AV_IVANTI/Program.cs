using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;

namespace Chec_kAV_IVANTI
{
    class Program
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        static string ReadSubKeyValue(string subKey, string key)
        {
            string str = string.Empty;
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(subKey))
            {
                if (registryKey != null)

                {
                    try
                    {
                        str = registryKey.GetValue(key).ToString();
                        registryKey.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception error with get regkey :" + ex.ToString());
                    }

                }
            }
            return str;
        }
        public static String GetWindowsServiceStatus(String SERVICENAME)
        {
            ServiceController sc = new ServiceController(SERVICENAME);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Status Changing";
            }
        }
        static void Main(string[] args)
        {
            String AVstatus = "ntrtscan";
            String IVANTI = "EMSS Agent";
            String regtry_text = "";
            String ComName = Environment.MachineName.ToString();

            if (Environment.Is64BitOperatingSystem)
            {
                regtry_text = @"SOFTWARE\WOW6432Node\TrendMicro\PC-cillinNTCorp\CurrentVersion";
            }
            else
            {
                regtry_text = @"SOFTWARE\TrendMicro\PC-cillinNTCorp\CurrentVersion";
            }


            String ipaddress = GetLocalIPAddress();
            String NetID = ipaddress.Substring(0, ipaddress.LastIndexOf("."));
            String ServerAV= NetID.Substring(0, NetID.LastIndexOf("."));
            String MAC = ReadSubKeyValue(regtry_text, "MAC");
            String GUID = ReadSubKeyValue(regtry_text, "GUID");
            String InstallDate = ReadSubKeyValue(regtry_text, "InstDate");
            String domain = ReadSubKeyValue(regtry_text, "Domain");
            String NtVer = ReadSubKeyValue(regtry_text, "NtVer");
            String server = ReadSubKeyValue(regtry_text, "Server");
            Console.WriteLine("Computer name  : {0}", GetLocalIPAddress()); 


            Console.WriteLine("IP Address : {0}", ipaddress );
            Console.WriteLine("Network ID : {0}",NetID );
            Console.WriteLine("Server For install  : {0}.1.5", ServerAV);
            Console.WriteLine("MAC Address : {0}",MAC );
            Console.WriteLine("GUID : {0}", GUID);
            Console.WriteLine("Install Date : {0}", InstallDate);
            Console.WriteLine("Domain : {0}", domain);
            Console.WriteLine("Nt Version : {0}", NtVer);
            Console.WriteLine("Server : {0}", server);
            
            Console.WriteLine("Service Name is :{0} : service is {1}", AVstatus, GetWindowsServiceStatus(AVstatus));
            Console.WriteLine("Service Name is :{0} : service is {1}", IVANTI, GetWindowsServiceStatus(IVANTI));

            Console.ReadKey();
        }
    }
}
