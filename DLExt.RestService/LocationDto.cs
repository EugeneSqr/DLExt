namespace DLExt.RestService
{
    using System.Runtime.Serialization;

    [DataContract]
    public class LocationDto
    {
        public LocationDto(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public bool IsSelected { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Path { get; set; }
    }
}