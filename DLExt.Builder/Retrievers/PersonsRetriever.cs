using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using DLExt.Builder.Model;

namespace DLExt.Builder.Retrievers
{
    public class PersonsRetriever : Retriever<Person>
    {
        public PersonsRetriever(string server) : base(server)
        {
        }

        public override IList<Person> Retrieve(string path)
        {
            var result = new List<Person>();
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", Server, path)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry, "(objectCategory=Person)"))
                    {
                        SearchResultCollection persons = searcher.FindAll();
                        if (persons.Count == 0)
                        {
                            return result;
                        }

                        result.AddRange(from SearchResult person in persons
                                        where IsPropertyValid(person, "displayName") && IsPropertyValid(person, "mail")
                                        orderby person.Properties["displayName"].ToString()
                                        select new Person(
                                            person.Properties["displayName"][0].ToString(), 
                                            person.Properties["mail"][0].ToString()));
                    }
                }
            }
            catch
            {
            }

            return result.OrderBy(person => person.DisplayName).ToList();
        }
    }
}
