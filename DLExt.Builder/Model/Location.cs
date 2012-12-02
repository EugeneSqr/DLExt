namespace DLExt.Builder.Model
{
    public class Location
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsSelected { get; set; }

        public Location(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
