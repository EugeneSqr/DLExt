using System.Collections.Generic;
using System.ServiceModel.Web;
using DLExt.Infrastructure;
using DLExt.Model;

namespace DLExt.Service
{
    public class DistributionListService : IDistributionListService
    {
        private readonly Repository repository;
        
        public DistributionListService()
        {
            repository = new Repository("domain.corp");
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IList<Person> GetPersons()
        {
            return repository.GetPersons();
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IList<string> GetLocations()
        {
            return repository.GetLocations();
        }
    }
}
