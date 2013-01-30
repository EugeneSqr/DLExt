using System;
using System.Collections.Generic;
using System.Linq;

namespace DLExt.WebApplication
{
    using DLExt.Domain;
    using DLExt.RestService;

    internal class Controller
    {
        private readonly IList<Person> excludedPersons;

        private readonly string serviceUrl = @"http://dlext.ru/Service.svc";
        private IList<Location> locations;
        private IEnumerable<Person> persons;

        public Controller()
        {
            excludedPersons = new List<Person>();
            locations = new List<Location>();
        }

        public IEnumerable<Location> Locations
        {
            get { return this.locations; }
        }

        public IEnumerable<Person> Persons
        {
            get { return this.persons; }
        }

        public IEnumerable<Person> ExcludedPersons
        {
            get { return this.excludedPersons; }
        }

        public string GetEmailList(out int count)
        {
            var request = new FilterRequest
                {
                    ExcludedPersons = this.excludedPersons.Select(Mapper.ToDataTransferObject).ToList(),
                    Locations = this.locations.Select(Mapper.ToDataTransferObject).Where(l => l.IsSelected).ToList()
                };
            var data = Utils.FormatJson(request);
            var responce = Utils.CallRestPost(serviceUrl + RestUri.GetDistributionList, data);

            var emailList = responce.TrimStart('"').TrimEnd('"');
            count = emailList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Length;

            return emailList;
        }

        public void LoadAvailableLocations()
        {
            var locationsString = Utils.CallRestGet(serviceUrl + RestUri.GetLocations);
            this.locations = Utils.ParseJson<List<LocationDto>>(locationsString).Select(Mapper.ToBusinessObject).ToList();
        }

        public void LoadPersonsList()
        {
            string result;
            if (locations.Any(l => l.IsSelected))
            {
                var request = new FilterRequest
                    {
                        ExcludedPersons = new List<PersonDto>(),
                        Locations = locations.Where(l => l.IsSelected).Select(Mapper.ToDataTransferObject).ToList(),
                    };
                var data = Utils.FormatJson(request);
                result = Utils.CallRestPost(serviceUrl + RestUri.GetPersons, data);
            }
            else
            {
                result = Utils.CallRestGet(serviceUrl + RestUri.GetAllPersons);
            }

            this.persons = Utils.ParseJson<List<PersonDto>>(result).Select(Mapper.ToBusinessObject);
            persons = persons.OrderBy(p => p.DisplayName);
        }

        public void LoadExcludedPersonsFromCookies(string excludedPersonCookie)
        {
            if (string.IsNullOrEmpty(excludedPersonCookie))
            {
                return;
            }

            var rawPersons = excludedPersonCookie.Split(new[] { "##" }, StringSplitOptions.None);
            foreach (var person in rawPersons)
            {
                var nameEmail = person.Split(new[] { "@@" }, StringSplitOptions.None);
                this.excludedPersons.Add(new Person(nameEmail[0], nameEmail[1]));
            }
        }

        public string ConvertExcludedPersonsToCookieValue()
        {
            var cookie = string.Empty;
            foreach (var person in this.ExcludedPersons)
            {
                cookie += person.DisplayName + "@@" + person.Email + "##";
            }

            if (!string.IsNullOrEmpty(cookie))
            {
                cookie = cookie.TrimEnd('#');
            }

            return cookie;
        }

        public void AddExcludedPersonByEmail(string email)
        {
            if (this.excludedPersons.Any(p => p.Email.Equals(email)))
            {
                return;
            }

            this.excludedPersons.Add(this.persons.First(p => p.Email.Equals(email)));
        }

        public void ClearExcludedPersonsList()
        {
            this.excludedPersons.Clear();
        }

        public void SetLocationSelected(string locationName, bool selected)
        {
            foreach (var location in this.locations)
            {
                if (location.Name == locationName)
                {
                    location.IsSelected = selected;
                }
            }
        }

        public void DeleteExcludedPersonByEmail(string email)
        {
            var person = excludedPersons.FirstOrDefault(p => p.Email.Equals(email));
            if (person == null)
            {
                return;
            }

            excludedPersons.Remove(person);
        }
    }
}
