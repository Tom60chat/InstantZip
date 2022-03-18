using System.IO.Compression;

namespace InstantZip
{
    internal static class AppOptions
    {
        internal static List<string> Sources = new();
        internal static string Destination = Directory.GetCurrentDirectory();
        internal static CompressionLevel CompressionLevel = CompressionLevel.Fastest;

        public static void Apply(string[] args)
        {
            string lastArg = string.Empty;

            foreach (var arg in args)
            {
                switch (lastArg)
                {
                    case "-d":
                    case "--destination":
                        Sources.RemoveAt(Sources.Count - 1);
                        Destination = arg;
                        break;

                    case "-c":
                    case "--compression-level":
                        Sources.RemoveAt(Sources.Count - 1);
                        CompressionLevel = arg.ToLower() switch
                        {
                            "nocompression" => CompressionLevel.NoCompression,
                            "n" => CompressionLevel.NoCompression,

                            "fastest" => CompressionLevel.Fastest,
                            "f" => CompressionLevel.Fastest,

                            "optimal" => CompressionLevel.Optimal,
                            "o" => CompressionLevel.Optimal,

                            "smallestSize" => CompressionLevel.SmallestSize,
                            "s" => CompressionLevel.SmallestSize,

                            _ => CompressionLevel
                        };
                        break;

                    default:
                        Sources.Add(arg);
                        break;
                }

                lastArg = arg;
            }
        }
    }
}
