using STEMUtil.Source.Structs;
using System;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace STEMUtil.Source
{
    class Extractor : PKTRGeneral
    {
        private string input;
        private string output;
        private string pkrFile;

        public Extractor(String input, String pkrFile, String output)
        {
            this.input = input;
            this.pkrFile = pkrFile;
            this.output = output;
        }

        public delegate void ProgressUpdate(int currentData, int maxData, string textStatus);
        public event ProgressUpdate OnProgressUpdate;

        override
        public void Run()
        {
            output += "\\";
            String header = output + "HEADER.data";

            FileInfo file = new FileInfo(output);
            Directory.CreateDirectory(file.DirectoryName);

            using (FileStream fs = new FileStream(input, FileMode.Open, FileAccess.Read))
            using (BinaryReader data = new BinaryReader(fs))
            {

                this.StatusText = "Getting PTR structure...";
                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                CPTRStruct cptr = new CPTRStruct(data);

                byte[] compessedData = data.ReadBytes((int)(data.BaseStream.Length - 0x10));
                byte[] decompressedPTRData = this.DecompressData(ref compessedData);

                using (BufferedStream stream = new BufferedStream(new FileStream(header, FileMode.Create, FileAccess.Write)))
                {
                    stream.Write(decompressedPTRData, 0, decompressedPTRData.Length);
                }

                using (BinaryReader ptrData = new BinaryReader(new FileStream(header, FileMode.Open, FileAccess.Read)))
                {
                    DPTRStruct ptr = new DPTRStruct(ptrData);
                    PKRStruct[] table = ptr.Files;

                    this.TotalFiles = (int)ptr.FilesCount;
                    this.CurrentProgress = 0;
                    OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                    using (FileStream pkrFS = new FileStream(pkrFile, FileMode.Open, FileAccess.Read))
                    using (BinaryReader pkr = new BinaryReader(pkrFS))
                    {
                        byte[] pkrData;
                        string outputFile;
                        FileInfo extractedFile;

                        for (int i = 0; i < ptr.FilesCount; ++i)
                        {
                            this.CurrentProgress = i;
                            OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                            pkr.BaseStream.Seek(table[i].Offset, SeekOrigin.Begin);

                            if (ptr.Filenames.ElementAtOrDefault(table[i].NameID) != null)
                            {
                                this.StatusText = "Extracting: " + ptr.Filenames[table[i].NameID];
                                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                                outputFile = output + ptr.Filenames[table[i].NameID];
                                pkrData = pkr.ReadBytes(table[i].CompressedSize);

                                if (table[i].CompressedSize != table[i].DecompressedSize)
                                {
                                    pkrData = this.DecompressData(ref pkrData);
                                }

                                extractedFile = new FileInfo(outputFile);

                                this.SaveData(ref pkrData, outputFile);

                                pkrData = null;

                                // File extension is lanb -> convert it.
                                if (extractedFile.Extension.Equals(".lanb"))
                                {
                                    this.StatusText = "Converting text file: " + ptr.Filenames[table[i].NameID];

                                    using (FileStream lanbFS = new FileStream(outputFile, FileMode.Open, FileAccess.Read))
                                    using (BinaryReader lanbReader = new BinaryReader(lanbFS))
                                    {
                                        LABNStruct lanbData = new LABNStruct(lanbReader);

                                        using (StreamWriter lanbWriter = new StreamWriter(outputFile + ".txt"))
                                        {
                                            for (int j = 0; j < lanbData.StringCount; ++j)
                                            {
                                                lanbWriter.WriteLine(lanbData.RowsName[j]);
                                                lanbWriter.WriteLine(lanbData.RowsText[j]);
                                            }
                                        }
                                    }
                                }
                                // File name is 48.dat -> convert to xml map. (some games 64_df.dat)
                                else if (extractedFile.Name.Equals("48.dat"))
                                {
                                    this.StatusText = "Converting font map: " + ptr.Filenames[table[i].NameID];

                                    using (FileStream idfFS = new FileStream(outputFile, FileMode.Open, FileAccess.Read))
                                    using (BinaryReader idfReader = new BinaryReader(idfFS))
                                    {
                                        IDFStruct idfData = new IDFStruct(idfReader);
                                        IDFCharStruct[] charsData = idfData.CharsData;

                                        using (StreamWriter lanbWriter = new StreamWriter(outputFile + ".xml"))
                                        {
                                            lanbWriter.WriteLine("<GeneralData " +
                                                "chars=\""+idfData.CharsCount+ 
                                                "\" param1=\"" + idfData.Unknown1 + 
                                                "\" param2=\"" + idfData.Unknown2 +
                                                "\" param3=\"" + idfData.Unknown3 + "\" />"
                                            );

                                            for (int j = 0; j < charsData.Length; ++j)
                                            {
                                                lanbWriter.WriteLine("<Char " +
                                                    "id=\""+ charsData[j].CharID + 
                                                    "\" x=\"" + charsData[j].LeftTopX +
                                                    "\" y=\"" + charsData[j].LeftTopY +
                                                    "\" width=\"" + charsData[j].Width +
                                                    "\" height=\"" + charsData[j].Height +
                                                    "\" bearingX=\"" + charsData[j].BearingX +
                                                    "\" bearingY=\"" + charsData[j].BearingY +
                                                    "\" advanceX=\"" + charsData[j].AdvanceX +
                                                    "\" advanceY=\"" + charsData[j].AdvanceY + "\" />"
                                                );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            this.StatusText = "Done";
            this.CurrentProgress = this.TotalFiles;
            OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);
        }

        private byte[] DecompressData(ref byte[] data)
        {
            MemoryStream decompressedStream = new MemoryStream();
            using (MemoryStream compressedMem = new MemoryStream(data))
            using (DeflateStream deflated = new DeflateStream(compressedMem, CompressionMode.Decompress))
            {
                 deflated.CopyTo(decompressedStream);
                 deflated.Close();

                 decompressedStream.Position = 0;

                 return decompressedStream.ToArray();
            }
        }
    }
}
