namespace InstantZip
{
    public static class DirectoryInfoExtensions
    {
        #region Methods
        /// <summary>
        /// Get the parent or root of the directory
        /// </summary>
        /// <param name="directory">Directory</param>
        /// <returns>The parent or root of the directory</returns>
        public static DirectoryInfo GetParentOrRoot(this DirectoryInfo directory) =>
            directory.Parent ?? directory.Root;
        #endregion
    }
}
