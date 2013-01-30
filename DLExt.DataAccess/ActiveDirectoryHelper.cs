using System.DirectoryServices;

namespace DLExt.DataAccess
{
    public static class ActiveDirectoryHelper
    {
        public static SearchResultCollection Read(string fullPath, string filter)
        {
            using (var entry = new DirectoryEntry(fullPath))
            {
                using (var searcher = new DirectorySearcher(entry, filter) { SearchScope = SearchScope.OneLevel })
                {
                    return searcher.FindAll();
                }
            }
        }
    }
}