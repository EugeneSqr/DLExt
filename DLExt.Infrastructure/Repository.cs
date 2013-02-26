using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using DLExt.Model;
using log4net;

namespace DLExt.Infrastructure
{
    public class Repository
    {
        private const string DistinguishedNameKey = "distinguishedName";
        private const string NameKey = "name";
        private const string PersonNameFieldKey = "displayName";
        private const string PersonMailFieldKey = "mail";

        private readonly string root;
        private readonly string rootPath;

        private int index;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Repository));

        public Repository(string controllerName)
        {
            root = string.Format("LDAP://{0}/", controllerName);
            rootPath = string.Concat(root, "OU=Sites,OU=Company,DC=domain,DC=corp");
        }

        public virtual IList<Location> GetData()
        {
            var result = new List<Location>();
            SearchResultCollection locationsSearchResult = GetLocationsSearchResult();
            index = 1;
            foreach (SearchResult locationSearchResult in locationsSearchResult)
            {
                if (IsPropertyValid(locationSearchResult, DistinguishedNameKey) && IsPropertyValid(locationSearchResult, NameKey))
                {
                    var distinguishedName = locationSearchResult.Properties[DistinguishedNameKey][0].ToString();
                    var name = locationSearchResult.Properties[NameKey][0].ToString();
                    using (var container = new DirectoryEntry(string.Concat(root, distinguishedName)))
                    {
                        IList<Person> persons = GetItem(
                            GetSearchResult("(objectCategory=Person)", SearchScope.Subtree, container),
                            (id, searchResult) => new Person(
                                                      id,
                                                      searchResult.Properties[PersonNameFieldKey][0].ToString(),
                                                      searchResult.Properties[PersonMailFieldKey][0].ToString(),
                                                      name),
                            searchResult => (IsPropertyValid(searchResult, PersonNameFieldKey) &&
                                             IsPropertyValid(searchResult, PersonMailFieldKey)),
                            null);
                        if (persons.Count > 0)
                        {
                            result.Add(new Location {Name = name, Persons = persons});
                        }
                    }
                }
            }
            
            return result;
        }

        private SearchResultCollection GetLocationsSearchResult()
        {
            return GetSearchResult("(objectClass=organizationalUnit)", SearchScope.OneLevel);
        }

        private SearchResultCollection GetSearchResult(string filter, SearchScope searchScope)
        {
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format(rootPath)))
                {
                    return GetSearchResult(filter, searchScope, entry);
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Error creating container entry", exception);
            }

            return null;
        }

        private static SearchResultCollection GetSearchResult(
            string filter, 
            SearchScope searchScope, 
            DirectoryEntry container)
        {
            try
            {
                using (DirectorySearcher searcher = new DirectorySearcher(container, filter)
                                                        {
                                                            SearchScope = searchScope
                                                        })
                {
                    return searcher.FindAll();
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Error retrieving items", exception);
            }

            return null;
        }

        private IList<T> GetItem<T>(
            ICollection searchResult,
            Func<int, SearchResult, T> factoryMethod,
            Func<SearchResult, bool> validationFunction,
            Func<T, string> orderFieldSelector) where T : class
        {
            Logger.InfoFormat("Retrieving items of type {0}.", typeof(T));
            var result = new List<T>();
            if (searchResult == null || searchResult.Count == 0)
            {
                return result;
            }

            foreach (SearchResult searchItem in searchResult)
            {
                if (validationFunction(searchItem))
                {
                    result.Add(factoryMethod(index, searchItem));
                }

                index++;
            }

            return (orderFieldSelector == null) 
                ? result 
                : result.OrderBy(orderFieldSelector).ToList();
        }

        private static bool IsPropertyValid(SearchResult item, string propertyName)
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
