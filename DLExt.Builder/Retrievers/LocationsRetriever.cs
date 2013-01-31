using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using DLExt.Builder.Model;
using log4net;

namespace DLExt.Builder.Retrievers
{
    public class LocationsRetriever : Retriever<Location>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LocationsRetriever));

        public LocationsRetriever(string server) : base(server)
        {
        }

        public override IList<Location> Retrieve(string path)
        {
            Logger.Info("LocationsRetriever: Retrieve has been invoked.");
            var result = new List<Location>();
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", Server, path)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry, "(objectClass=organizationalUnit)") { SearchScope = SearchScope.OneLevel })
                    {
                        SearchResultCollection locations = searcher.FindAll();
                        if (locations.Count == 0)
                        {
                            return result;
                        }

                        result.AddRange(from SearchResult location in locations
                                        where IsPropertyValid(location, "name") && IsPropertyValid(location, "distinguishedName")
                                        select new Location(
                                            location.Properties["name"][0].ToString(), 
                                            location.Properties["distinguishedName"][0].ToString()));
                    }
                }
            }
            catch(Exception exception)
            {
                Logger.Error("LocationsRetriever: error retrieving locations: ", exception);
            }

            return result;
        }
    }
}
