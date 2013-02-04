using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using DLExt.Model;
using log4net;

namespace DLExt.Infrastructure
{
    public class Repository
    {
        private const string RootPath = "OU=Sites,OU=Company,DC=domain,DC=corp";
        private const string PersonNameFieldKey = "displayName";
        private const string PersonMailFieldKey = "mail";
        private const string PersonLocarionFieldKey = "physicalDeliveryOfficeName";
        
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Repository));
        
        public string ControllerName { get; private set; }
        
        public Repository(string controllerName)
        {
            ControllerName = controllerName;
        }

        public virtual IList<Person> GetPersons()
        {
            Logger.Info("Retrieving persons.");
            var result = new List<Person>();
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", ControllerName, RootPath)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry, "(objectCategory=Person)"))
                    {
                        SearchResultCollection persons = searcher.FindAll();
                        if (persons.Count == 0)
                        {
                            return result;
                        }

                        int index = 1;
                        foreach (SearchResult person in persons)
                        {
                            if (IsPropertyValid(person, PersonNameFieldKey) && IsPropertyValid(person, PersonMailFieldKey) && IsPropertyValid(person, PersonLocarionFieldKey))
                            {
                                result.Add(new Person(
                                    index,
                                    person.Properties[PersonNameFieldKey][0].ToString(),
                                    person.Properties[PersonMailFieldKey][0].ToString(),
                                    person.Properties[PersonLocarionFieldKey][0].ToString()));
                            }
                            else
                            {
                                Logger.WarnFormat("Some of the required fields are empty. Person is skipped");
                            }

                            index++;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Error retrieving persons", exception);
            }

            return result.OrderBy(person => person.Name).ToList();
        }

        public virtual IList<string> GetLocations()
        {
            Logger.Info("RetrievingLocations");
            var result = new List<string>();
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", ControllerName, RootPath)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry, "(objectClass=organizationalUnit)") { SearchScope = SearchScope.OneLevel })
                    {
                        SearchResultCollection locations = searcher.FindAll();
                        if (locations.Count == 0)
                        {
                            return result;
                        }

                        result.AddRange(from SearchResult location in locations
                                        where IsPropertyValid(location, "name")
                                        select location.Properties["name"][0].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Error retrieving locations: ", exception);
            }

            return result;
        }

        protected virtual bool IsPropertyValid(SearchResult item, string propertyName)
        {
            if (item != null && item.Properties != null && item.Properties.Contains(propertyName))
            {
                var propertyItem = item.Properties[propertyName];
                if (propertyItem != null && propertyItem.Count > 0)
                {
                    var propertyItemString = propertyItem[0] as string;
                    return !string.IsNullOrEmpty(propertyItemString) && propertyItemString.Trim().Length > 0;
                }
            }

            return false;
        }
    }
}
