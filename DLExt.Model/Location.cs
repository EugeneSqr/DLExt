using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DLExt.Model
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "persons")]
        public IList<Person> Persons { get; set; }
    }
}
