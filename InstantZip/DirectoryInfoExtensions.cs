namespace InstantZip
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetParentOrRoot(this DirectoryInfo directory) => directory.Parent ?? directory.Root;
    }
}
