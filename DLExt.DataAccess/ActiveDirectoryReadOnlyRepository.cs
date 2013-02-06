using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;

namespace DLExt.DataAccess
{
    using System.Linq;

    using DLExt.Domain;

    public class ActiveDirectoryReadOnlyRepository : IReadOnlyRepository
    {
        private readonly string server;
        private readonly string rootPath;

        private readonly string locationFilter;
        private readonly string personsFilter;

        public ActiveDirectoryReadOnlyRepository()
        {
            this.server = "domain.corp";
            this.rootPath = "OU=Sites,OU=Company,DC=domain,DC=corp";
            locationFilter = "(objectClass=organizationalUnit)";
            personsFilter = "(objectCategory=Person)";
        }

        public ReadOnlyCollection<Location> AllLocations
        {
            get
            {
                var path = this.CreateADPathString(rootPath);
                var collection = ActiveDirectoryHelper.Read(path, locationFilter);
                var list = new List<Location>();
                foreach (SearchResult result in collection)
                {
                    Location location;
                    if (result.TryParseLocation(out location))
                    {
                        list.Add(location);
                    }
                }

                return new ReadOnlyCollection<Location>(list);
            }
        }

        public ReadOnlyCollection<Person> GetPersonsByLocations(IEnumerable<Location> selectedLocations)
        {
            var list = new List<Person>();
            foreach (var location in selectedLocations)
            {
                var path = CreateADPathString(string.Format("{0},{1}", "OU=Users", location.Path));
                var collection = ActiveDirectoryHelper.Read(path, personsFilter);
                foreach (SearchResult result in collection)
                {
                    Person person;
                    if (result.TryParsePerson(out person))
                    {
                        list.Add(person);
                    }
                }
            }

            return new ReadOnlyCollection<Person>(list.OrderBy(p => p.DisplayName).ToList());
        }

        public IDictionary<Location, IEnumerable<Person>> GetPersonsGroupedByLocation()
        {
            var dictionary = new Dictionary<Location, IEnumerable<Person>>();

            foreach (var location in AllLocations)
            {
                dictionary.Add(location, GetPersonsByLocations(new[] { location }));
            }

            return dictionary;
        }

        private string CreateADPathString(string relativePath)
        {
            return string.Format("LDAP://{0}/{1}", this.server, relativePath);
        }
    }
}