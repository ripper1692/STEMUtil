using System.IO;

namespace STEMUtil.Source.Structs
{
    class PKRStruct
    {
        private static byte[] magic = { 0x83, 0x11, 0xBA, 0xFC };

        private int nameID;

        private int offset;

        private int compressedSize;
        private int decompressedSize;

        public PKRStruct(BinaryReader data)
        {
            this.NameID = data.ReadInt32();

            this.Offset = data.ReadInt32();

            this.CompressedSize = data.ReadInt32();
            this.DecompressedSize = data.ReadInt32();
        }

        public static byte[] Magic { get => magic; set => magic = value; }
        public int NameID { get => nameID; set => nameID = value; }
        public int Offset { get => offset; set => offset = value; }
        public int CompressedSize { get => compressedSize; set => compressedSize = value; }
        public int DecompressedSize { get => decompressedSize; set => decompressedSize = value; }
    }
}
