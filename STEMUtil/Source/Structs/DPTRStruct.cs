using System.IO;
using System.Text;

namespace STEMUtil.Source.Structs
{
    class DPTRStruct
    {
        private int filesCount;
        private int unknown2;
        private int unknown3;
        private int unknown4;
        private int unknown5;

        private int[] unknownDataset1;
        private int[] unknownDataset2;

        private int namesLength;
        private int namesCount;

        private string[] filenames;

        private PKRStruct[] files;

        private byte[] someshit;

        public DPTRStruct(BinaryReader data)
        {
            this.FilesCount = data.ReadInt32();
            this.Unknown2 = data.ReadInt32();
            this.Unknown3 = data.ReadInt32();
            this.Unknown4 = data.ReadInt32();
            this.Unknown5 = data.ReadInt32();

            this.UnknownDataset1 = new int[this.Unknown4];
            for (int i = 0; i < this.Unknown4; ++i)
            {
                this.UnknownDataset1[i] = data.ReadInt32();
            }

            this.UnknownDataset2 = new int[this.Unknown5];
            for (int i = 0; i < this.Unknown5; ++i)
            {
                this.UnknownDataset2[i] = data.ReadInt32();
            }

            this.NamesLength = data.ReadInt32();
            this.NamesCount = data.ReadInt32();

            byte[] namesContainer = data.ReadBytes(this.NamesLength);
            this.Filenames = Encoding.UTF8.GetString(namesContainer).Split('\0');

            Files = new PKRStruct[this.FilesCount];
            for (int i = 0; i < this.FilesCount; ++i)
            {
                this.Files[i] = new PKRStruct(data);
            }

            Someshit = data.ReadBytes((int)(data.BaseStream.Length - data.BaseStream.Position));
        }

        public int Unknown2 { get => unknown2; set => unknown2 = value; }
        public int Unknown3 { get => unknown3; set => unknown3 = value; }
        public int Unknown4 { get => unknown4; set => unknown4 = value; }
        public int Unknown5 { get => unknown5; set => unknown5 = value; }
        public int[] UnknownDataset1 { get => unknownDataset1; set => unknownDataset1 = value; }
        public int[] UnknownDataset2 { get => unknownDataset2; set => unknownDataset2 = value; }
        public int NamesLength { get => namesLength; set => namesLength = value; }
        public string[] Filenames { get => filenames; set => filenames = value; }
        public int NamesCount { get => namesCount; set => namesCount = value; }
        public int FilesCount { get => filesCount; set => filesCount = value; }
        public byte[] Someshit { get => someshit; set => someshit = value; }
        internal PKRStruct[] Files { get => files; set => files = value; }
    }
}
