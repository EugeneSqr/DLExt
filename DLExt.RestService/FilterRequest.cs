namespace DLExt.RestService
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class FilterRequest
    {
        [DataMember]
        public List<LocationDto> Locations { get; set; }

        [DataMember]
        public List<PersonDto> ExcludedPersons { get; set; }
    }
}