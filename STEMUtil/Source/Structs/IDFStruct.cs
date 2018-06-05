using System;
using System.IO;

namespace STEMUtil.Source.Structs
{
    class IDFStruct
    {
        private byte[] magic = new byte[4];
        private short unknown1;
        private short unknown2;
        private short unknown3;

        private short charsCount;

        IDFCharStruct[] charsData;

        public IDFStruct(BinaryReader data)
        {
            this.Magic = data.ReadBytes(this.Magic.Length);

            this.Unknown1 = data.ReadInt16();
            this.Unknown2 = data.ReadInt16();
            this.Unknown3 = data.ReadInt16();

            byte[] tempChars = data.ReadBytes(2);
            Array.Reverse(tempChars);

            this.CharsCount = BitConverter.ToInt16(tempChars, 0);

            this.CharsData = new IDFCharStruct[this.CharsCount];
            for (int i = 0; i < this.CharsCount; ++i)
            {
                this.CharsData[i] = new IDFCharStruct();

                this.CharsData[i].Width = data.ReadByte();
                this.CharsData[i].Height = data.ReadByte();

                this.CharsData[i].BearingY = data.ReadByte();
                this.CharsData[i].AdvanceY = data.ReadByte();

                this.CharsData[i].BearingX = data.ReadByte();
                this.CharsData[i].AdvanceX = data.ReadByte();

                this.CharsData[i].LeftTopX = data.ReadInt16();
                this.CharsData[i].LeftTopY = data.ReadInt16();
            }

            for (int i = 0; i < this.CharsCount; ++i)
            {
                this.CharsData[i].CharID = data.ReadInt32();
            }
        }

        public byte[] Magic { get => magic; set => magic = value; }
        public short Unknown1 { get => unknown1; set => unknown1 = value; }
        public short Unknown2 { get => unknown2; set => unknown2 = value; }
        public short Unknown3 { get => unknown3; set => unknown3 = value; }
        public short CharsCount { get => charsCount; set => charsCount = value; }
        internal IDFCharStruct[] CharsData { get => charsData; set => charsData = value; }
    }
}
