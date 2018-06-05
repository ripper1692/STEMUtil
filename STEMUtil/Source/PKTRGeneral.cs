using System;
using System.IO;

namespace STEMUtil.Source
{
    abstract class PKTRGeneral
    {
        private int totalFiles = 100;
        private int currentProgress = 0;
        private string statusText = "";

        public int TotalFiles { get => totalFiles; set => totalFiles = value; }
        public string StatusText { get => statusText; set => statusText = value; }
        public int CurrentProgress { get => currentProgress; set => currentProgress = value; }

        protected void SaveData(ref byte[] content, String outputName)
        {
            FileInfo file = new FileInfo(outputName);
            Directory.CreateDirectory(file.DirectoryName);

            using (FileStream fileStream = new FileStream(outputName, FileMode.Append, FileAccess.Write, FileShare.None))
            using (BinaryWriter data = new BinaryWriter(fileStream))
            {
                data.Write(content);
            }
        }

        abstract public void Run();
    }
}
