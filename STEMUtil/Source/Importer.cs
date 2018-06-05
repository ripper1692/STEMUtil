using STEMUtil.Source.Structs;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Security.Cryptography;

namespace STEMUtil.Source
{
    class Importer : PKTRGeneral
    {
        private string input;
        private string outputPkr;
        private string outputPtr;

        public Importer(String input, String outputPkr, String outputPtr)
        {
            this.input = input;
            this.outputPkr = outputPkr;
            this.outputPtr = outputPtr;
        }

        public delegate void ProgressUpdate(int currentData, int maxData, string textStatus);
        public event ProgressUpdate OnProgressUpdate;

        override
        public void Run()
        {
            input += "\\";
            String header = input + "HEADER.data";

            FileInfo fileInfo;
            String searchedName;
            using (FileStream fs = new FileStream(header, FileMode.Open, FileAccess.Read))
            using (BinaryReader headerStream = new BinaryReader(fs))
            {
                this.StatusText = "Reading structure...";
                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                CPTRStruct cptrData = new CPTRStruct();
                DPTRStruct ptrData = new DPTRStruct(headerStream);
                PKRStruct[] table = ptrData.Files;

                this.TotalFiles = (int)table.Length;
                this.CurrentProgress = 0;

                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                int pkrOffset = 16;
                byte[] compressedContainer;
                byte[] ending = {0, 0, 255, 255};

                byte[] bytes;

                bytes = PKRStruct.Magic;
                this.SaveData(ref bytes, outputPkr);

                bytes = new byte[12];
                this.SaveData(ref bytes, outputPkr);

                bytes = null;

                for (int i = 0; i < table.Length; ++i)
                {
                    this.CurrentProgress = i;
                    OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);
                    if (ptrData.Filenames.ElementAtOrDefault(table[i].NameID) != null)
                    {
                        this.StatusText = "Importing: " + ptrData.Filenames[table[i].NameID];
                        OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);

                        searchedName = input + ptrData.Filenames[table[i].NameID];
                        fileInfo = new FileInfo(searchedName);

                        if (fileInfo.Extension.Equals(".lanb") && new FileInfo(searchedName + ".txt").Exists)
                        {
                            this.StatusText = "Converting text data from: " + searchedName + ".txt";
                            this.ImportLanb(fileInfo.FullName, searchedName + ".txt");
                        }
                        else if (fileInfo.Name.Equals("48.dat") && new FileInfo(searchedName + ".xml").Exists)
                        {
                            this.StatusText = "Converting font map from: " + searchedName + ".txt";
                            this.ImportFontMap(fileInfo.FullName, searchedName + ".xml");
                        }

                        compressedContainer = this.GetFileData(searchedName);

                        table[i].Offset = pkrOffset;
                        table[i].CompressedSize = compressedContainer.Length;
                        table[i].DecompressedSize = (int)fileInfo.Length;

                        this.SaveData(ref compressedContainer, outputPkr);

                        if (table[i].CompressedSize != table[i].DecompressedSize)
                        {
                            this.SaveData(ref ending, outputPkr);
                        }
                        pkrOffset += table[i].CompressedSize;

                        compressedContainer = null;
                    }
                }

                // Create new ptr
                this.StatusText = "Creating new PTR file";
                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);
                using (FileStream ptrStream = new FileStream(outputPtr, FileMode.Append, FileAccess.Write))
                using (BinaryWriter ptrWriter = new BinaryWriter(ptrStream))
                {
                    ptrWriter.Write(ptrData.FilesCount);
                    ptrWriter.Write(ptrData.Unknown2);
                    ptrWriter.Write(ptrData.Unknown3);
                    ptrWriter.Write(ptrData.Unknown4);
                    ptrWriter.Write(ptrData.Unknown5);

                    for (int i = 0; i < ptrData.UnknownDataset1.Length; ++i)
                    {
                        ptrWriter.Write(ptrData.UnknownDataset1[i]);
                    }

                    for (int i = 0; i < ptrData.UnknownDataset2.Length; ++i)
                    {
                        ptrWriter.Write(ptrData.UnknownDataset2[i]);
                    }

                    ptrWriter.Write(ptrData.NamesLength);
                    ptrWriter.Write(ptrData.NamesCount);

                    ptrWriter.Write(Encoding.UTF8.GetBytes(String.Join("\0", ptrData.Filenames)));

                    for (int i = 0; i < table.Length; ++i)
                    {
                        ptrWriter.Write(table[i].NameID);

                        ptrWriter.Write(table[i].Offset);

                        ptrWriter.Write(table[i].CompressedSize);
                        ptrWriter.Write(table[i].DecompressedSize);
                    }

                    ptrWriter.Write(ptrData.Someshit);
                }

                // new PKR hash
                this.StatusText = "Calculate new PKR hash";
                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);
                byte[] PkrHashData = new byte[0x8000];
                using (FileStream pkrfs = new FileStream(outputPkr, FileMode.Open, FileAccess.Read))
                using (BinaryReader pkrbr = new BinaryReader(pkrfs))
                {
                    pkrbr.BaseStream.Seek(16, SeekOrigin.Begin);
                    PkrHashData = pkrbr.ReadBytes(PkrHashData.Length);
                }

                byte[] MD5PkrHash = this.MD5hash(PkrHashData);
                int MD5FinalPkr = 0;
                for (int i = 0; i < MD5PkrHash.Length; i += 4)
                {
                    MD5FinalPkr = MD5FinalPkr ^ BitConverter.ToInt32(MD5PkrHash.Skip(i).Take(4).ToArray(), 0);
                }

                // new PTR hash
                this.StatusText = "Calculate new PTR hash";
                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);
                byte[] MD5PtrHash = this.MD5hash(File.ReadAllBytes(outputPtr));
                int MD5FinalPtr = 0;
                for (int i = 0; i < MD5PtrHash.Length; i += 4)
                {
                    MD5FinalPtr = MD5FinalPtr ^ BitConverter.ToInt32(MD5PtrHash.Skip(i).Take(4).ToArray(), 0);
                }

                cptrData.DecompressedSize = cptrData.CryptSize((int)(new FileInfo(outputPtr).Length));
                cptrData.PkrChecksum = MD5FinalPkr;
                cptrData.PtrChecksum = MD5FinalPtr;

                byte[] compressedPTR = this.Compress(outputPtr);

                using (FileStream compressedFS = new FileStream(outputPtr, FileMode.Create, FileAccess.Write))
                using (BinaryWriter cPTR = new BinaryWriter(compressedFS))
                {
                    cPTR.Write(cptrData.DecompressedSize);
                    cPTR.Write(cptrData.Version);
                    cPTR.Write(cptrData.PkrChecksum);
                    cPTR.Write(cptrData.PtrChecksum);

                    cPTR.Write(compressedPTR);
                    cPTR.Write(ending);
                }

                compressedPTR = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                this.StatusText = "Done";
                this.CurrentProgress = this.TotalFiles;
                OnProgressUpdate?.Invoke(this.CurrentProgress, this.TotalFiles, this.StatusText);
            }
        }

        private void ImportLanb(String sourceFile, String textFile)
        {
            LABNStruct lanbData;

            using (FileStream lanbFS = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (BinaryReader lanbReader = new BinaryReader(lanbFS))
            {
                lanbData = new LABNStruct(lanbReader);

                using (StreamReader txtReader = new StreamReader(textFile))
                {
                    String bufferLine;
                    String textContainer = "";

                    int rowIndex;
                    int prevIndex = -1;

                    while (!txtReader.EndOfStream)
                    {
                        bufferLine = txtReader.ReadLine();

                        if (bufferLine.IndexOf("#") == 0)
                        {
                            rowIndex = Array.IndexOf(lanbData.RowsName, bufferLine);
                            if (textContainer.Length > 0 && prevIndex != -1)
                            {
                                lanbData.RowsText[prevIndex] = textContainer.Trim();
                                textContainer = "";
                            }

                            prevIndex = rowIndex;
                        }
                        else
                        {
                            textContainer += "\n" + bufferLine;
                        }

                        if (textContainer.Length > 0 && prevIndex != -1)
                        {
                            textContainer = textContainer.Trim();
                            lanbData.RowsText[prevIndex] = textContainer;
                        }
                    }
                }
            }

            // Create new lanb
            using (FileStream lanboutFS = new FileStream(sourceFile, FileMode.Create, FileAccess.Write))
            using (BinaryWriter lanbWriter = new BinaryWriter(lanboutFS))
            {
                lanbWriter.Write(lanbData.Magic);
                lanbWriter.Write(lanbData.Unknown1);
                lanbWriter.Write(lanbData.StringCount);

                for (int i = 0; i < lanbData.StringCount; ++i)
                {
                    lanbWriter.Write(lanbData.RowsID[i]);

                    lanbWriter.Write(Encoding.UTF8.GetByteCount(lanbData.RowsName[i]));
                    lanbWriter.Write(Encoding.UTF8.GetBytes(lanbData.RowsName[i]));

                    lanbWriter.Write(Encoding.UTF8.GetByteCount(lanbData.RowsText[i]));
                    lanbWriter.Write(Encoding.UTF8.GetBytes(lanbData.RowsText[i]));
                }
            }
        }

        private void ImportFontMap(String sourceFile, String xmlMap)
        {
            IDFStruct idfData;
            IDFCharStruct[] charsData;

            using (FileStream idfFS = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (BinaryReader idfReader = new BinaryReader(idfFS))
            {
                idfData = new IDFStruct(idfReader);

                String xmlLine;
                using (StreamReader xmlReader = new StreamReader(xmlMap))
                {
                    xmlLine = xmlReader.ReadLine();
                    idfData.CharsCount = Convert.ToInt16(this.GetParam("chars", xmlLine));

                    idfData.Unknown1 = Convert.ToInt16(this.GetParam("param1", xmlLine));
                    idfData.Unknown2 = Convert.ToInt16(this.GetParam("param2", xmlLine));
                    idfData.Unknown3 = Convert.ToInt16(this.GetParam("param3", xmlLine));

                    charsData = new IDFCharStruct[idfData.CharsCount];
                    for (int i = 0; i < idfData.CharsCount; ++i)
                    {
                        xmlLine = xmlReader.ReadLine();

                        charsData[i] = new IDFCharStruct();

                        charsData[i].CharID = Convert.ToInt32(this.GetParam("id", xmlLine));

                        charsData[i].LeftTopX = Convert.ToInt16(this.GetParam("x", xmlLine));
                        charsData[i].LeftTopY = Convert.ToInt16(this.GetParam("y", xmlLine));

                        charsData[i].Width = Convert.ToByte(this.GetParam("width", xmlLine));
                        charsData[i].Height = Convert.ToByte(this.GetParam("height", xmlLine));

                        charsData[i].BearingX = Convert.ToByte(this.GetParam("bearingX", xmlLine));
                        charsData[i].BearingY = Convert.ToByte(this.GetParam("bearingY", xmlLine));

                        charsData[i].AdvanceX = Convert.ToByte(this.GetParam("advanceX", xmlLine));
                        charsData[i].AdvanceY = Convert.ToByte(this.GetParam("advanceY", xmlLine));
                    }
                }
            }

            // Create new 48.dat
            using (FileStream xmlFS = new FileStream(sourceFile, FileMode.Create, FileAccess.Write))
            using (BinaryWriter xmlmapWriter = new BinaryWriter(xmlFS))
            {
                xmlmapWriter.Write(idfData.Magic);
                xmlmapWriter.Write(idfData.Unknown1);
                xmlmapWriter.Write(idfData.Unknown2);
                xmlmapWriter.Write(idfData.Unknown3);

                byte[] tempChars = BitConverter.GetBytes(idfData.CharsCount);
                Array.Reverse(tempChars);

                xmlmapWriter.Write(tempChars);

                for (int i = 0; i < idfData.CharsCount; ++i)
                {
                    xmlmapWriter.Write(charsData[i].Width);
                    xmlmapWriter.Write(charsData[i].Height);

                    xmlmapWriter.Write(charsData[i].BearingY);
                    xmlmapWriter.Write(charsData[i].AdvanceY);

                    xmlmapWriter.Write(charsData[i].BearingX);
                    xmlmapWriter.Write(charsData[i].AdvanceX);
                    
                    xmlmapWriter.Write(charsData[i].LeftTopX);
                    xmlmapWriter.Write(charsData[i].LeftTopY);
                }

                for (int i = 0; i < idfData.CharsCount; ++i)
                {
                    xmlmapWriter.Write(charsData[i].CharID);
                }
            }
        }

        private String GetParam(String paramName, String text)
        {
            int startIndex = text.IndexOf(paramName + "=\"");
            startIndex = startIndex != -1 ? startIndex + paramName.Length + 2 : -1;

            int endIndex = startIndex != -1 ? text.IndexOf("\"", startIndex) : -1;

            String paramValue = startIndex != -1 && endIndex != -1 ? text.Substring(startIndex, endIndex - startIndex) : "";

            return paramValue;
        }

        private byte[] Compress(String file)
        {
            MemoryStream compressedData = new MemoryStream();

            using (FileStream compressedFileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (DeflateStream compressionStream = new DeflateStream(compressedData, CompressionMode.Compress))
            {
                compressedFileStream.CopyTo(compressionStream);
                compressionStream.Close();

                return compressedData.ToArray();
            }
        }

        private byte[] GetFileData(String file)
        {
            byte[] content;

            using (FileStream compressedFileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                content = new byte[compressedFileStream.Length];
                compressedFileStream.Read(content, 0, (int)compressedFileStream.Length);

                return content;
            }
        }

        private byte[] MD5hash(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] result = md5.ComputeHash(data);

            return result;
        }
    }
}
