using System.Collections.Generic;
using System.Linq;
using DLExt.DataAccess;
using DLExt.Domain;

namespace DLExt.RestService
{
    public class AppDirectoryService : IAppDirectoryService
    {
        private readonly IReadOnlyRepository repository;

        public AppDirectoryService()
        {
            this.repository = new ActiveDirectoryReadOnlyRepository();
        }

        public string GetDistributionList(FilterRequest filterRequest)
        {
            var persons = this.repository.GetPersonsByLocations(filterRequest.Locations.Select(Mapper.ToBusinessObject));
            var resultPersons = persons.Except(filterRequest.ExcludedPersons.Select(Mapper.ToBusinessObject), new PersonEqualityComparer());

            var distributionListCreator = new DistributionListCreator(resultPersons);
            return distributionListCreator.Create();
        }

        public IEnumerable<LocationDto> GetLocations()
        {
            return this.repository.AllLocations.Select(Mapper.ToDataTransferObject);
        }

        public IEnumerable<PersonDto> GetAllPersons()
        {
            return this.repository.GetPersonsByLocations(this.repository.AllLocations).Select(Mapper.ToDataTransferObject);
        }

        public IEnumerable<PersonDto> GetPersons(FilterRequest filterRequest)
        {
            var persons = this.repository.GetPersonsByLocations(filterRequest.Locations.Select(Mapper.ToBusinessObject));
            var resultPersons = persons.Except(filterRequest.ExcludedPersons.Select(Mapper.ToBusinessObject), new PersonEqualityComparer());

            return resultPersons.Select(Mapper.ToDataTransferObject);
        }
    }


}
