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
            String SrvAV= NetID.Substring(0, NetID.LastIndexOf("."));
            String findip = SrvAV.Substring(3);
            //String ServerAV = "10.20.2.7";
            //if (int.Parse(findip) > 79)
            //{
            //    ServerAV = SrvAV + ".1.5";
            //}
            String MAC = ReadSubKeyValue(regtry_text, "MAC");
            String GUID = ReadSubKeyValue(regtry_text, "GUID");
            String InstallDate = ReadSubKeyValue(regtry_text, "InstDate");
            String domain = ReadSubKeyValue(regtry_text, "Domain");
            String NtVer = ReadSubKeyValue(regtry_text, "NtVer");
            String server = ReadSubKeyValue(regtry_text, "Server");

            
            string[] ipvalues = ipaddress.Split('.');
            //Console.WriteLine(Int32.Parse(ipvalues[0]));
            //Console.WriteLine(Int32.Parse(ipvalues[1]));
            //Console.WriteLine(Int32.Parse(ipvalues[2]));
            //Console.WriteLine(Int32.Parse(ipvalues[3]));

            
            Console.WriteLine("Computer name  : {0}", GetLocalIPAddress()); 


            Console.WriteLine("IP Address : {0}", ipaddress );
            Console.WriteLine("Network ID : {0}",NetID );
            Console.WriteLine("IP Address for search :  {0}", findip);
            //Console.WriteLine("Server For install  : {0}", ServerAV);
            Console.WriteLine("MAC Address : {0}",MAC );
            Console.WriteLine("GUID : {0}", GUID);
            Console.WriteLine("Install Date : {0}", InstallDate);
            Console.WriteLine("Domain : {0}", domain);
            Console.WriteLine("Nt Version : {0}", NtVer);
            Console.WriteLine("Server : {0}", server);
            
            Console.WriteLine("Service Name is :{0} : service is {1}", AVstatus, GetWindowsServiceStatus(AVstatus));
            Console.WriteLine("Service Name is :{0} : service is {1}", IVANTI, GetWindowsServiceStatus(IVANTI));

            
            string instserver = "";
            if (ipvalues[0] == "10")
            {
                if (Int32.Parse(ipvalues[1]) <= 33)
                {
                    instserver = "10.20.2.7";
                }
                else if ((Int32.Parse(ipvalues[1]) >= 34 && Int32.Parse(ipvalues[1]) <= 69) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";  //PAK1-3
                }
                else if ((Int32.Parse(ipvalues[1]) >= 34 && Int32.Parse(ipvalues[1]) <= 69) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.20.2.9";  //PAK1-3 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 80 && Int32.Parse(ipvalues[1]) <= 95) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK4
                }
                else if ((Int32.Parse(ipvalues[1]) >= 80 && Int32.Parse(ipvalues[1]) <= 95) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.80.7.7";   //PAK4 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 96 && Int32.Parse(ipvalues[1]) <= 111) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK5
                }
                else if ((Int32.Parse(ipvalues[1]) >= 96 && Int32.Parse(ipvalues[1]) <= 111) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.96.7.7";   //PAK5 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 112 && Int32.Parse(ipvalues[1]) <= 127) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK6
                }
                else if ((Int32.Parse(ipvalues[1]) >= 112 && Int32.Parse(ipvalues[1]) <= 127) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.122.7.7";   //PAK6 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 128 && Int32.Parse(ipvalues[1]) <= 143) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK7
                }
                else if ((Int32.Parse(ipvalues[1]) >= 128 && Int32.Parse(ipvalues[1]) <= 143) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.128.7.7";   //PAK7 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 144 && Int32.Parse(ipvalues[1]) <= 159) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK8
                }
                else if ((Int32.Parse(ipvalues[1]) >= 144 && Int32.Parse(ipvalues[1]) <= 159) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.144.7.7";   //PAK8 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 160 && Int32.Parse(ipvalues[1]) <= 175) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK9
                }
                else if ((Int32.Parse(ipvalues[1]) >= 160 && Int32.Parse(ipvalues[1]) <= 175) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.160.7.7";   //PAK9 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 176 && Int32.Parse(ipvalues[1]) <= 191) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK10
                }
                else if ((Int32.Parse(ipvalues[1]) >= 176 && Int32.Parse(ipvalues[1]) <= 191) && (Int32.Parse(ipvalues[2]) >= 16))
                {
                    instserver = "10.176.7.7";   //PAK10 SS
                }
                else if ((Int32.Parse(ipvalues[1]) >= 192 && Int32.Parse(ipvalues[1]) <= 207) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK11
                }
                else if ((Int32.Parse(ipvalues[1]) >= 192 && Int32.Parse(ipvalues[1]) <= 207) && (Int32.Parse(ipvalues[2]) > 16))
                {
                    instserver = "10.192.7.7";   //PAK11 SS

                }
                else if ((Int32.Parse(ipvalues[1]) >= 208 && Int32.Parse(ipvalues[1]) <= 216) && (Int32.Parse(ipvalues[2]) < 16))
                {
                    instserver = ipvalues[0] + ipvalues[1] + ".1.5";    //PAK12
                }
                else if ((Int32.Parse(ipvalues[1]) >= 208 && Int32.Parse(ipvalues[1]) <= 216) && (Int32.Parse(ipvalues[2]) > 16))
                {
                    instserver = "10.208.7.7";   //PAK12 SS

                }
                else
                {
                    instserver = "10.20.2.9";
                }
            }
            else
            {
                instserver = "10.20.2.7";
            }

            Console.WriteLine("Server For install AV is : {0}", instserver);
            Console.ReadKey();
        }
    }
}
