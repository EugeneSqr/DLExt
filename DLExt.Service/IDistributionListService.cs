using System.Collections.Generic;
using System.ServiceModel;
using DLExt.Model;

namespace DLExt.Service
{
    [ServiceContract]
    public interface IDistributionListService
    {
        [OperationContract]
        IList<Person> GetPersons();

        [OperationContract]
        IList<string> GetLocations();
    }
}
