using System;
using System.IO;

namespace api.test
{
    public class TempFile : IDisposable
    {
        public FileInfo File { get; }
        public string FileName { get; }

        public TempFile(string data)
        {
            FileName = Path.GetTempFileName();
            File = new FileInfo(FileName);
            System.IO.File.WriteAllText(File.FullName, data);
        }
        public void Dispose()
        {
            if (File.Exists)
                File.Delete();
        }


    }
}
