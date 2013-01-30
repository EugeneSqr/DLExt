namespace DLExt.Domain
{
    public class Person
    {
        public Person(string displayName, string email)
        {
            this.DisplayName = displayName;
            this.Email = email;
        }

        public string DisplayName { get; private set; }

        public string Email { get; private set; }
    }
}
