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
        private const string PersonNameFieldKey = "displayName";
        private const string PersonMailFieldKey = "mail";
        private const string PersonLocarionFieldKey = "physicalDeliveryOfficeName";

        private readonly string rootPath;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Repository));

        public Repository(string controllerName)
        {
            rootPath = string.Format("LDAP://{0}/OU=Sites,OU=Company,DC=domain,DC=corp", controllerName);
        }

        public virtual IList<Person> GetPersons()
        {
            return GetItem(
                "(objectCategory=Person)",
                SearchScope.Subtree,
                (id, searchResult) => new Person(
                    id,
                    searchResult.Properties[PersonNameFieldKey][0].ToString(),
                    searchResult.Properties[PersonMailFieldKey][0].ToString(),
                    searchResult.Properties[PersonLocarionFieldKey][0].ToString()),
                result => (IsPropertyValid(result, PersonNameFieldKey) &&
                           IsPropertyValid(result, PersonMailFieldKey) &&
                           IsPropertyValid(result, PersonLocarionFieldKey)),
                person => person.Name);
        }

        public virtual IList<string> GetLocations()
        {
            return GetItem(
                "(objectClass=organizationalUnit)",
                SearchScope.OneLevel,
                (id, searchResult) => searchResult.Properties["name"][0].ToString(),
                searchResult => IsPropertyValid(searchResult, "name"),
                item => item);
        }

        private IList<T> GetItem<T>(
            string filter,
            SearchScope searchScope,
            Func<int, SearchResult, T> factoryMethod,
            Func<SearchResult, bool> validationFunction,
            Func<T, string> orderFieldSelector) where T : class
        {
            Logger.InfoFormat("Retrieving items of type {0}.", typeof(T));
            var result = new List<T>();
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format(rootPath)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry, filter)
                                                            {
                                                                SearchScope = searchScope
                                                            })
                    {
                        SearchResultCollection searchItems = searcher.FindAll();
                        if (searchItems.Count == 0)
                        {
                            return result;
                        }

                        int index = 1;
                        foreach (SearchResult searchItem in searchItems)
                        {
                            if (validationFunction(searchItem))
                            {
                                result.Add(factoryMethod(index, searchItem));
                            }

                            index++;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Error retrieving items", exception);
            }

            return result.OrderBy(orderFieldSelector).ToList();
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
