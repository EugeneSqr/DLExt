using System.Collections.Generic;
using System.Linq;
using DLExt.DataAccess;
using DLExt.Domain;

namespace DLExt.RestService
{
    using System;
    using System.Web;

    using log4net;

    public class AppDirectoryService : IAppDirectoryService
    {
        private readonly IReadOnlyRepository repository;

        private static ILog logger = LogManager.GetLogger(typeof(AppDirectoryService));

        public AppDirectoryService()
        {
            this.repository = new ActiveDirectoryReadOnlyRepository();
        }

        public IEnumerable<LocationDto> GetLocations()
        {
            return this.repository.AllLocations.Select(Mapper.ToDataTransferObject);
        }

        public IEnumerable<PersonDto> GetAllPersons()
        {
            return this.repository.GetPersonsByLocations(this.repository.AllLocations).Select(Mapper.ToDataTransferObject);
        }

        public string GetDistributionList(string filterRequest)
        {
            //filterRequest = HttpUtility.UrlDecode(filterRequest);
            var request = Utils.ParseJson<FilterRequest>(filterRequest);
            return this.GetDistributionListPost(request);
        }

        public string GetDistributionListPost(FilterRequest filterRequest)
        {
            var persons = this.repository.GetPersonsByLocations(filterRequest.Locations.Select(Mapper.ToBusinessObject));
            var resultPersons = persons.Except(filterRequest.ExcludedPersons.Select(Mapper.ToBusinessObject), new PersonEqualityComparer());

            var distributionListCreator = new DistributionListCreator(resultPersons);
            return distributionListCreator.Create();
        }

        public IEnumerable<PersonDto> GetPersons(string filterRequest)
        {
            logger.DebugFormat("raw filterRequest = '{0}'", filterRequest);
            var request = Utils.ParseJson<FilterRequest>(filterRequest);
            return this.GetPersonsPost(request);
        }

        public IEnumerable<PersonDto> GetPersonsPost(FilterRequest filterRequest)
        {
            try
            {
                var persons =
                    this.repository.GetPersonsByLocations(filterRequest.Locations.Select(Mapper.ToBusinessObject));
                var resultPersons = persons.Except(
                    filterRequest.ExcludedPersons.Select(Mapper.ToBusinessObject), new PersonEqualityComparer());

                return resultPersons.Select(Mapper.ToDataTransferObject);
            }
            catch (Exception exception)
            {
                logger.ErrorFormat("Unexpected exception occured: {0}", exception.Message);
                throw;
            }
        }

        public IDictionary<LocationDto, IEnumerable<PersonDto>> GetPersonsGroupedByLocations()
        {
            var groupedPersons =  repository.GetPersonsGroupedByLocation();
            var result = new Dictionary<LocationDto, IEnumerable<PersonDto>>();
            foreach (var key in groupedPersons.Keys)
            {
                var location = Mapper.ToDataTransferObject(key);
                result.Add(location, groupedPersons[key].Select(Mapper.ToDataTransferObject));
            }
            return result;
        }
    }
}
