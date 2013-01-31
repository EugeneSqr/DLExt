using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using DLExt.Builder.Model;
using log4net;

namespace DLExt.Builder.Retrievers
{
    public class PersonsRetriever : Retriever<Person>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PersonsRetriever));

        public PersonsRetriever(string server) : base(server)
        {
        }

        public override IList<Person> Retrieve(string path)
        {
            Logger.Info("PersonsRetriever: Retrieve method has been invoked.");
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
            catch(Exception exception)
            {
                Logger.Error("PersonsRetriever: error retrieving persons", exception);
            }

            return result.OrderBy(person => person.DisplayName).ToList();
        }
    }
}
