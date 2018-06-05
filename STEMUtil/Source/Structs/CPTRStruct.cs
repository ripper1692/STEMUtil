using System;
using System.IO;

namespace STEMUtil.Source.Structs
{
    class CPTRStruct
    {
        private int decompressedSize;
        private int version = 1;
        private int pkrChecksum;
        private int ptrChecksum;

        public CPTRStruct() { }

        public CPTRStruct(BinaryReader data)
        {
            this.DecompressedSize = data.ReadInt32();

            this.Version = data.ReadInt32();

            this.PkrChecksum = data.ReadInt32();
            this.PtrChecksum = data.ReadInt32();
        }

        public int CryptSize(int size)
        {
            byte[] cryptVal = { 0x88, 0x46, 0xDC, 0xFA };

            return size ^ BitConverter.ToInt32(cryptVal, 0);
        }

        public int DecompressedSize { get => decompressedSize; set => decompressedSize = value; }
        public int Version { get => version; set => version = value; }
        public int PkrChecksum { get => pkrChecksum; set => pkrChecksum = value; }
        public int PtrChecksum { get => ptrChecksum; set => ptrChecksum = value; }
    }
}
