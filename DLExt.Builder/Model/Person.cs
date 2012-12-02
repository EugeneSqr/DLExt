namespace DLExt.Builder.Model
{
    public class Person
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public Person(string displayName, string email)
        {
            DisplayName = displayName;
            Email = email;
        }
    }
}
