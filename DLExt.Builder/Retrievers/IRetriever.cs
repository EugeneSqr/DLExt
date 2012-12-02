using System.Collections.Generic;

namespace DLExt.Builder.Retrievers
{
    public interface IRetriever<T>
    {
        string Server { get; }

        IList<T> Retrieve(string path);
    }
}
