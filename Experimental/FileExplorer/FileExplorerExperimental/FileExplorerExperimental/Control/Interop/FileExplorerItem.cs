namespace FileExplorerExperimental.Control.Interop
{
    public class FileExplorerItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFolder { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
