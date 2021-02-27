using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendsmsWeb.Contacts
{
    public static class ContactsDbInterface
    {
        /// <summary>
        /// Gets all contacts from a specific SQL query
        /// </summary>
        /// <param name="Query">The SQL query given</param>
        /// <returns>A list of contact objects</returns>
        static async Task<List<Contact>> GetContactsFromSql(string Query)
        {
            // Exceute SQL query and get rows
            List<List<string>> Rows = await Program.DatabaseConnection.GetRowsAsync(Query);

            // Create contact objects for each row
            List<Contact> Contacts = new List<Contact>();

            foreach (List<string> Row in Rows)
            {
                // Assuming this order: PhoneNumber, FullName, IndexName, Company, JobTitle, Comment, Tags
                // Create a new contact object and add to list
                Contacts.Add(new Contact
                {
                    PhoneNumber = Row[0],
                    FullName = Row[1],
                    IndexName = Row[2],
                    Company = Row[3],
                    JobTitle = Row[4],
                    Comment = Row[5],
                    TagsJson = Row[6]
                });
            }

            // Return the list
            return Contacts;
        }

        /// <summary>
        /// Gets all contacts in the database.
        /// </summary>
        /// <returns>A list of contact objects</returns>
        public static async Task<List<Contact>> GetAllContacts() => await GetContactsFromSql("SELECT [PhoneNumber], [FullName], [IndexName], [Company], [JobTitle], [Comment], [Tags] FROM [dbo].[Contacts] ORDER BY [IndexName] ASC;");

        /// <summary>
        /// Gets all contacts in the database, matching the specified tags.
        /// </summary>
        /// <param name="Tags">A list of tags to look for.</param>
        /// <param name="RequireAllPresent">Wether all tags must be present, or wether to look for contact who have at least one.</param>
        /// <returns>A list of contact objects</returns>
        public static async Task<List<Contact>> GetContactsFromTags(List<string> Tags, bool RequireAllPresent)
        {
            // Check that Tags list has members
            if (Tags.Count == 0)
            {
                throw new ArgumentException("Argument Tags must contain members.");
            }

            string Conditions = "";

            bool IsZeroeth = true;
            // Iterate through all tags
            foreach (string Tag in Tags)
            {
                // Add logical operator if not is zeroeth
                if (!IsZeroeth)
                {
                    Conditions += " " + RequireAllPresent switch
                    {
                        true => "AND",
                        false => "OR"
                    } + " ";
                }
                // Add condition
                Conditions += $"[Tags] LIKE '%{DatabaseConnection.EscapeSql(Tag).Replace("\"", "\\\"")}%'";
            }

            // Return contact getter with conditions
            return await GetContactsFromSql($"SELECT [PhoneNumber], [FullName], [IndexName], [Company], [JobTitle], [Comment], [Tags] FROM [dbo].[Contacts] WHERE {Conditions} ORDER BY [IndexName] ASC;");
        }

        /// <summary>
        /// Gets all tags in the database.
        /// </summary>
        /// <returns>A list of all tags in the database.</returns>
        public static async Task<List<string>> GetAllTags()
        {
            // Get all unique arrays
            List<List<string>> UniqueArrays = await Program.DatabaseConnection.GetRowsAsync("SELECT DISTINCT [Tags] FROM [dbo].[Contacts];");

            // Create a list to add tags to
            List<string> Tags = new List<string>();

            // Iterate through all rows
            foreach (List<string> Row in UniqueArrays)
            {
                // Iterate through each value in a parsed json array
                foreach (string Tag in JArray.Parse(Row[0]))
                {
                    // Add to list if not already present
                    if (Tags.IndexOf(Tag) < 0)
                    {
                        Tags.Add(Tag);
                    }
                }
            }

            // Return the tag list
            return Tags;
        }
    }
}