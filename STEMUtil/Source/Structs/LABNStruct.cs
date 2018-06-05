using System.IO;
using System.Text;

namespace STEMUtil.Source.Structs
{
    class LABNStruct
    {
        private byte[] magic = new byte[4];
        private int unknown1;
        private int stringCount;

        private int[] rowsID;
        private string[] rowsName;
        private string[] rowsText;

        public LABNStruct(BinaryReader data)
        {
            this.Magic = data.ReadBytes(this.Magic.Length);
            this.Unknown1 = data.ReadInt32();
            this.StringCount = data.ReadInt32();

            this.RowsID = new int[this.StringCount];
            this.RowsName = new string[this.StringCount];
            this.RowsText = new string[this.StringCount];
            int stringLength;

            for (int i = 0; i < this.StringCount; ++i)
            {
                this.RowsID[i] = data.ReadInt32();

                stringLength = data.ReadInt32();
                this.RowsName[i] = Encoding.UTF8.GetString(data.ReadBytes(stringLength));

                stringLength = data.ReadInt32();
                this.RowsText[i] = Encoding.UTF8.GetString(data.ReadBytes(stringLength));
            }
        }

        public byte[] Magic { get => magic; set => magic = value; }
        public int Unknown1 { get => unknown1; set => unknown1 = value; }
        public int StringCount { get => stringCount; set => stringCount = value; }
        public int[] RowsID { get => rowsID; set => rowsID = value; }
        public string[] RowsName { get => rowsName; set => rowsName = value; }
        public string[] RowsText { get => rowsText; set => rowsText = value; }
    }
}
