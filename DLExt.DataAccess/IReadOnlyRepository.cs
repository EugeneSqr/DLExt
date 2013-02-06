using System.Collections.Generic;
using System.Collections.ObjectModel;
using DLExt.Domain;

namespace DLExt.DataAccess
{
    public interface IReadOnlyRepository    {
        ReadOnlyCollection<Location> AllLocations { get; }

        ReadOnlyCollection<Person> GetPersonsByLocations(IEnumerable<Location> selectedLocations);

        IDictionary<Location, IEnumerable<Person>> GetPersonsGroupedByLocation();
    }
}
