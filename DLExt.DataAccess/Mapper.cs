using System.DirectoryServices;
using DLExt.Domain;

namespace DLExt.DataAccess
{
    using System;

    internal static class Mapper
    {
        public static bool TryParseLocation(this SearchResult searchResult, out Location location)
        {
            location = null;
            string name, path;
            if (!searchResult.TryGetFirstValue("name", out name))
            {
                return false;
            }

            if (!searchResult.TryGetFirstValue("distinguishedName", out path))
            {
                return false;
            }

            location = new Location(name, path);
            return true;
        }

        public static bool TryParsePerson(this SearchResult searchResult, out Person person)
        {
            person = null;
            string name, email;
            if (!searchResult.TryGetFirstValue("displayName", out name))
            {
                return false;
            }

            if (!searchResult.TryGetFirstValue("mail", out email))
            {
                return false;
            }

            person = new Person(name, email);
            return true;
        }

        private static bool TryGetFirstValue(this SearchResult searchResult, string name, out string value)
        {
            value = null;
            if (searchResult == null)
            {
                throw new InvalidOperationException();
            }

            if (searchResult.Properties.Count == 0 ||
                searchResult.Properties[name] == null ||
                searchResult.Properties[name].Count == 0)
            {
                return false;
            }

            value = searchResult.Properties[name][0].ToString();
            return true;
        }
    }
}