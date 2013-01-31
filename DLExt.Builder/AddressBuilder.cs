using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLExt.Builder.Model;
using DLExt.Builder.Retrievers;
using log4net;

namespace DLExt.Builder
{
    public class AddressBuilder
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AddressBuilder));
        
        private readonly IList<string> containersToSearch;
        private readonly IList<Location> locationsToSearch;
        private readonly List<Person> extractedPersons;
        private readonly IList<Person> personsToExclude;
        private readonly List<Person> filteredPersons;

        public string Server { get; private set; }

        public string ResultAddress { get; private set; }

        public AddressBuilder(string server, IEnumerable<Location> locations, IEnumerable<Person> personsToExclude)
        {
            Server = server;
            locationsToSearch = locations.ToList();
            this.personsToExclude = personsToExclude.ToList();

            containersToSearch = new List<string>();
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
            catch(Exception exception)
            {
                Logger.Error("AddressBuilder: error during address string building", exception);
            }
            finally
            {
                FinalizeBuilder();
            }
        }

        public IList<Person> ExtractPersons()
        {
            LocateContainersToSearch(locationsToSearch);
            GetPersons();
            return extractedPersons;
        }

        protected virtual void LocateContainersToSearch(IList<Location> locations)
        {
            Logger.Info("AddressBuilder: locateContainersToSearch has been invoked.");
            foreach (Location location in locations)
            {
                containersToSearch.Add(string.Format("{0},{1}", "OU=Users", location.Path));
            }
        }

        protected virtual void GetPersons()
        {
            Logger.Info("AddressBuilder: getPersons has been invoked.");
            var personsRetriever = new PersonsRetriever(Server);
            foreach (string containerPath in containersToSearch)
            {
                extractedPersons.AddRange(personsRetriever.Retrieve(containerPath));
            }
        }

        protected virtual void ExcludePersons()
        {
            Logger.Info("AddressBuilder: excludePersons has been invoked.");
            filteredPersons.AddRange(extractedPersons.Except(personsToExclude, new PersonEqualityComparer()));
        }

        protected virtual void BuildDistributionList()
        {
            Logger.Info("AddressBuilder: build distribution list has been invoked.");
            var builder = new StringBuilder();
            foreach (Person person in filteredPersons)
            {
                builder.Append(person.Email).Append(';');
            }

            ResultAddress = builder.ToString();
        }

        protected virtual void FinalizeBuilder()
        {
        }
    }
}
