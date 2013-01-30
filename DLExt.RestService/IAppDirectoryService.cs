namespace DLExt.RestService
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IAppDirectoryService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetDistributionList", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetDistributionList(FilterRequest filterRequest);

        [OperationContract]
        [WebGet(UriTemplate = "/GetLocations", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<LocationDto> GetLocations();

        [OperationContract]
        [WebGet(UriTemplate = "/GetAllPersons", ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<PersonDto> GetAllPersons();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetPersons", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<PersonDto> GetPersons(FilterRequest filterRequest);
    }
}