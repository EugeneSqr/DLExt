using System.Collections.Generic;
using System.Text;

namespace DLExt.Domain
{
    public class DistributionListCreator
    {
        private IEnumerable<Person> persons;

        public DistributionListCreator(IEnumerable<Person> persons)
        {
            this.persons = persons;
        }

        public string Create()
        {
            var builder = new StringBuilder();
            foreach (Person person in persons)
            {
                builder.Append(person.Email).Append(';');
            }

            return builder.ToString();
        }
    }
}
