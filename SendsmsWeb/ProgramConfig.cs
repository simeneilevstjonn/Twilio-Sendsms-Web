using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SendsmsWeb
{
    public class ProgramConfig
    {
        /// <summary>
        /// Parses the config file at /etc/sendsms/webconfig.json or C:\\Program Files\\sendsms\\webconfig.json, and sets its values according to the parsed data. Throws exceptions if config file is invalid.
        /// </summary>
        public ProgramConfig()
        {
            string ConfigFile = "";
            bool IsWindows = false;

            try
            {
                // Get the file for the current OS
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    IsWindows = false;
                    ConfigFile = File.ReadAllText("/etc/sendsms/webconfig.json");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    IsWindows = true;
                    ConfigFile = File.ReadAllText("C:\\Program Files\\sendsms\\webconfig.json");
                }
                else
                {
                    throw new Exception("Unsupported operating system");
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Unsupported operating system") throw;

                throw new Exception("Could not find config file at " + IsWindows switch
                {
                    false => "/etc/sendsms/webconfig.json",
                    true => "C:\\Program Files\\sendsms\\webconfig.json"
                });
            }

            JObject ParsedConfig = JObject.Parse(ConfigFile);

            try
            {
                // Get the file for the current OS
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    IsWindows = false;
                    ConfigFile = File.ReadAllText("/etc/sendsms/config.json");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    IsWindows = true;
                    ConfigFile = File.ReadAllText("C:\\Program Files\\sendsms\\config.json");
                }
                else
                {
                    throw new Exception("Unsupported operating system");
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Unsupported operating system") throw;

                throw new Exception("Could not find config file at " + IsWindows switch
                {
                    false => "/etc/sendsms/config.json",
                    true => "C:\\Program Files\\sendsms\\config.json"
                });
            }

            JObject TwilioConfig = JObject.Parse(ConfigFile);

            // Set the properties
            ConnectionString = (string)ParsedConfig["ConnectionString"] ?? throw new Exception("ConnectionString is missing or invalid");
            AuthToken = (string)TwilioConfig["AuthToken"] ?? throw new Exception("AuthToken is missing or invalid");
            AccountSID = (string)TwilioConfig["AccountSID"] ?? throw new Exception("AccountSID is missing or invalid");
            ServiceSID = (string)TwilioConfig["ServiceSID"] ?? throw new Exception("ServiceSID is missing or invalid");
            EnableScheduling = (bool)ParsedConfig["EnableScheduling"];

            if (EnableScheduling)
            {
                SendsmsLocation = (string)ParsedConfig["SendsmsLocation"] ?? throw new Exception("SendsmsLocation is missing or invalid");
            }

            EnableContacts = (bool)ParsedConfig["EnableContacts"];
            EnableSenderDictionary = (bool)ParsedConfig["EnableSenderDictionary"];
            Enable1881 = (bool)ParsedConfig["Enable1881"];

            if (Enable1881)
            {
                ApiKey1881 = (string)ParsedConfig["ApiKey1881"] ?? throw new Exception("ApiKey1881 is missing or invalid");
            }
        }
        
        // Properties
        public string ConnectionString { get; private set; }
        public string SendsmsLocation { get; private set; }
        public bool EnableScheduling { get; private set; }
        public bool EnableContacts { get; private set; }
        public bool EnableSenderDictionary { get; private set; }
        public bool Enable1881 { get; private set; }
        public string ApiKey1881 { get; private set; }
        public string AuthToken { get; private set; }
        public string AccountSID { get; private set; }
        public string ServiceSID { get; private set; }

    }
}
