namespace DLExt.RestService
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PersonDto
    {
        public PersonDto(string displayName, string email)
        {
            this.DisplayName = displayName;
            this.Email = email;
        }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}