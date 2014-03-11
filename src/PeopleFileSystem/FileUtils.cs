using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PeopleFileSystem
{
    internal static class FileUtils
    {
        internal static IEnumerable<string> ReadAllLinesWithNoLock(string filePath)
        {
            var lines = new List<string>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);
            }

            return lines;
        }
    }
}