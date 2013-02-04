using System.Runtime.Serialization;

namespace DLExt.Model
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Location { get; set; }

        public Person(int id, string name, string email, string location)
        {
            Id = id;
            Name = name;
            Email = email;
            Location = location;
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Person)) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
