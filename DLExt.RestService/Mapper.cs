namespace DLExt.RestService
{
    using DLExt.Domain;

    public static class Mapper
    {
        public static Location ToBusinessObject(LocationDto locationDto)
        {
            return new Location(locationDto.Name, locationDto.Path) { IsSelected = locationDto.IsSelected };
        }

        public static Person ToBusinessObject(PersonDto personDto)
        {
            return new Person(personDto.DisplayName, personDto.Email);
        }

        public static PersonDto ToDataTransferObject(Person person)
        {
            return new PersonDto(person.DisplayName, person.Email);
        }

        public static LocationDto ToDataTransferObject(Location location)
        {
            return new LocationDto(location.Name, location.Path) { IsSelected = location.IsSelected };
        }
    }
}