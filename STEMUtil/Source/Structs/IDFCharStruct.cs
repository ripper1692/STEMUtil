namespace STEMUtil.Source.Structs
{
    class IDFCharStruct
    {
        private int charID;

        private short leftTopX;
        private short leftTopY;

        private byte width;
        private byte height;

        private byte bearingX;
        private byte bearingY;

        private byte advanceX;
        private byte advanceY;

        public IDFCharStruct() { }

        public int CharID { get => charID; set => charID = value; }

        public short LeftTopX { get => leftTopX; set => leftTopX = value; }
        public short LeftTopY { get => leftTopY; set => leftTopY = value; }

        public byte Width { get => width; set => width = value; }
        public byte Height { get => height; set => height = value; }

        public byte BearingX { get => bearingX; set => bearingX = value; }
        public byte BearingY { get => bearingY; set => bearingY = value; }

        public byte AdvanceX { get => advanceX; set => advanceX = value; }
        public byte AdvanceY { get => advanceY; set => advanceY = value; }
    }
}
