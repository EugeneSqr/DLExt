using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using DLExt.Builder.Model;

namespace DLExt.Builder
{
    public class AddressBuilder
    {
        private readonly IList<DirectoryEntry> containersToSearch;
        private readonly IList<Location> locationsToSearch;
        private readonly List<Person> extractedPersons;
        private readonly IList<Person> personsToExclude;
        private readonly List<Person> filteredPersons;

        public string Server { get; private set; }

        public string ResultAddress { get; private set; }

        public IList<string> PersonsToExclude { get; set; }

        public AddressBuilder(string server, IEnumerable<Location> locations, IEnumerable<Person> personsToExclude)
        {
            Server = server;
            locationsToSearch = locations.ToList();
            this.personsToExclude = personsToExclude.ToList();

            containersToSearch = new List<DirectoryEntry>();
            extractedPersons = new List<Person>();
            filteredPersons = new List<Person>();
            ResultAddress = string.Empty;
        }

        public void Build()
        {
            try
            {
                LocateContainersToSearch(locationsToSearch);
                GetPersons();
                ExcludePersons();
                BuildDistributionList();
            }
            finally
            {
                FinalizeBuilder();
            }
        }

        protected virtual void LocateContainersToSearch(IList<Location> locations)
        {
            foreach (Location location in locations)
            {
                containersToSearch.Add(new DirectoryEntry(string.Format("LDAP://{0}/{1},{2}", Server, "OU=Users", location.Path)));
            }
        }

        protected virtual void GetPersons()
        {
            foreach (DirectoryEntry container in containersToSearch)
            {
                using (var directorySearcher = new DirectorySearcher(container, "(objectCategory=Person)"))
                {
                    SearchResultCollection results = directorySearcher.FindAll();
                    if (results.Count == 0)
                    {
                        return;
                    }

                    extractedPersons.AddRange((from SearchResult result in results
                                select new Person(
                                    result.Properties["displayName"][0].ToString(), 
                                    result.Properties["mail"][0].ToString())).ToList());
                }
            }
        }

        protected virtual void ExcludePersons()
        {
            filteredPersons.AddRange(extractedPersons.Except(personsToExclude, new PersonEqualityComparer()));
        }

        protected virtual void BuildDistributionList()
        {
            var builder = new StringBuilder();
            foreach (Person person in filteredPersons)
            {
                builder.Append(person.Email).Append(';');
            }

            ResultAddress = builder.ToString();
        }

        protected virtual void FinalizeBuilder()
        {
            foreach (DirectoryEntry container in containersToSearch)
            {
                container.Dispose();
            }
        }
    }
}
