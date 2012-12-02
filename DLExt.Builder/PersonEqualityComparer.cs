using System.Collections.Generic;
using DLExt.Builder.Model;

namespace DLExt.Builder
{
    public class PersonEqualityComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            return x.DisplayName.ToLower() == y.DisplayName.ToLower();
        }

        public int GetHashCode(Person obj)
        {
            return obj.DisplayName.GetHashCode();
        }
    }
}
