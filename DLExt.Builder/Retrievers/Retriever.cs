using System.Collections.Generic;
using System.DirectoryServices;

namespace DLExt.Builder.Retrievers
{
    public abstract class Retriever<T>
    {
        protected Retriever(string server)
        {
            Server = server;
        }

        public string Server { get; private set; }

        public abstract IList<T> Retrieve(string path);

        protected virtual bool IsPropertyValid(SearchResult item, string propertyName)
        {
            if (item != null && item.Properties != null && item.Properties.Contains(propertyName))
            {
                var propertyItem = item.Properties[propertyName];
                if (propertyItem != null && propertyItem.Count > 0)
                {
                    var propertyItemString = propertyItem[0] as string;
                    return !string.IsNullOrEmpty(propertyItemString) && propertyItemString.Trim().Length > 0;
                }
            }

            return false;
        }
    }
}
