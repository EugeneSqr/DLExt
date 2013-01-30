namespace DLExt.Domain
{
    public class Location
    {
        public Location(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public string Name { get; private set; }

        public bool IsSelected { get; set; }

        public string Path { get; private set; }
    }
}
