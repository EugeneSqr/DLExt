using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using DLExt.Builder.Model;

namespace DLExt.Builder.Retrievers
{
    public class PersonsRetriever : IRetriever<Person>
    {
        public string Server { get; private set; }

        public PersonsRetriever(string server)
        {
            Server = server;
        }

        public IList<Person> Retrieve(string path)
        {
            var result = new List<Person>();
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", Server, path)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry, "(objectCategory=Person)"))
                    {
                        SearchResultCollection locations = searcher.FindAll();
                        if (locations.Count == 0)
                        {
                            return result;
                        }

                        result.AddRange(from SearchResult location in locations
                                        orderby location.Properties["displayName"][0].ToString()
                                        select new Person(
                                            location.Properties["displayName"][0].ToString(),
                                            location.Properties["mail"][0].ToString()));
                    }
                }
            }
            catch
            {
            }

            return result;
        }
    }
}
