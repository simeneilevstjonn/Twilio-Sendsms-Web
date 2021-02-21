using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendsmsWeb.Contacts
{
    public class Contact
    {
        public string FullName { get; set; }
        public string IndexName { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string Comment { get; set; }
        public string[] Tags { get; set; }
        public string TagsJson
        {
            get => JsonConvert.SerializeObject(Tags);
            set => Tags = JArray.Parse(value).ToObject<string[]>();
        }
        public string JobTitleAndCompany
        {
            get
            {
                // Return string
                string Ret = "";

                // Add job title if present
                if (!string.IsNullOrWhiteSpace(JobTitle))
                {
                    Ret += JobTitle;

                    // If comapny is present, add separator
                    if (!string.IsNullOrWhiteSpace(Company)) Ret += ", ";
                }

                // Add company if present
                if (!string.IsNullOrWhiteSpace(Company)) Ret += Company;

                // Return the string
                return Ret;
            }
        }
        public string TagsAsLinks
        {
            get
            {
                // Return string
                string Ret = "";

                bool IsZeroeth = true;

                // Iterate through each tag
                foreach (string Tag in Tags)
                {
                    // Add separator if not zeroeth
                    if (!IsZeroeth) Ret += ", ";
                    else IsZeroeth = false;
                    Ret += $"<a href=\"?tag={Tag}\">{Tag}</a>";
                }

                // Return the string
                return Ret;
            }
        }
    }
}
