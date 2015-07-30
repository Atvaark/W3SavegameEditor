using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using W3SavegameEditor.ChunkedLz4;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.VariableParsers;

namespace W3SavegameEditor.Savegame
{

    public class SavegameFile
    {
        private class RbEntry
        {
            public short Size { get; set; }
            public int Offset { get; set; }
        }

        /// <summary>
        /// Flags that determine the size of certain values.
        /// </summary>
        private const int Flags = 0x1008002;

        public int TypeCode1 { get; set; }
        public int TypeCode2 { get; set; }
        public int TypeCode3 { get; set; }

        public long HeaderStartOffset { get; set; }
        public int VariableTableOffset { get; set; }
        public int StringTableFooterOffset { get; set; }
        public int StringTableOffset { get; set; }
        public int RbSectionOffset { get; set; }
        public int NmSectionOffset { get; set; }

        public VariableTableEntry[] VariableTableEntries { get; set; }
        public string[] Strings { get; set; }

        public static SavegameFile Read(Stream compressedInputStream)
        {
            using (var inputStream = ChunkedLz4File.Decompress(compressedInputStream))
            using (var reader = new BinaryReader(inputStream, Encoding.ASCII, true))
            {
                var savegameFile = new SavegameFile();
                savegameFile.ReadHeader(reader);
                savegameFile.ReadFooter(reader);
                savegameFile.ReadStringTable(reader);
                savegameFile.ReadVariableTable(reader);
                savegameFile.ReadVariables(reader);
                return savegameFile;
            }
        }

        private void ReadVariables(BinaryReader reader)
        {
            var parsers = new List<VariableParserBase>
            {
                new BrVariableParser(),
                new BsVariableParser(),
                new ManuVariableParser(),
                new OpVariableParser(),
                new SsVariableParser(),
                new VlVariableParser()
            };
            var parser = new VariableParser(parsers);

            foreach (var tableEntry in VariableTableEntries)
            {
                reader.BaseStream.Position = tableEntry.Offset;
                try
                {
                    parser.Parse(reader, tableEntry.Size);
                }
                catch (ParseVariableException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        private void ReadVariableTable(BinaryReader reader)
        {
            reader.BaseStream.Seek(VariableTableOffset, SeekOrigin.Begin);
            int entryCount = reader.ReadInt32();
            VariableTableEntry[] entires = new VariableTableEntry[entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                entires[i] = new VariableTableEntry
                {
                    Offset = reader.ReadInt32(),
                    Size = reader.ReadInt32()
                };
            }
            VariableTableEntries = entires;
        }

        private void ReadFooter(BinaryReader reader)
        {
            reader.BaseStream.Seek(-6, SeekOrigin.End);
            VariableTableOffset = reader.ReadInt32();
            StringTableFooterOffset = VariableTableOffset - 10;
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "SE")
            {
                throw new InvalidOperationException();
            }
        }

        private void ReadStringTable(BinaryReader reader)
        {
            reader.BaseStream.Position = StringTableFooterOffset;
            NmSectionOffset = reader.ReadInt32();
            RbSectionOffset = reader.ReadInt32();
            ReadNmVariable(reader);
            ReadRbVariable(reader);

            reader.BaseStream.Position = StringTableOffset;
            string magicNumber = reader.ReadString(4);
            if (magicNumber != "MANU")
            {
                throw new InvalidOperationException();
            }

            int stringCount = reader.ReadInt32();
            reader.Skip(4);

            var strings = new string[stringCount];
            for (int i = 0; i < stringCount; i++)
            {
                byte size = reader.ReadByte();
                strings[i] = reader.ReadString(size);
            }
            Strings = strings;

            reader.Skip(4);
            string doneMagicNumber = reader.ReadString(4);
            if (doneMagicNumber != "ENOD")
            {
                throw new InvalidOperationException();
            }
        }

        private void ReadNmVariable(BinaryReader reader)
        {
            reader.BaseStream.Position = NmSectionOffset;
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "NM")
            {
                throw new InvalidOperationException();
            }
            StringTableOffset = (int) reader.BaseStream.Position;
        }

        private void ReadRbVariable(BinaryReader reader)
        {
            reader.BaseStream.Position = RbSectionOffset;
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "RB")
            {
                throw new InvalidOperationException();
            }
            int count = reader.ReadInt32();
            RbEntry[] rbEntries = new RbEntry[count];
            for (int i = 0; i < count; i++)
            {
                rbEntries[i] = new RbEntry
                {
                    Size = reader.ReadInt16(),
                    Offset = reader.ReadInt32()
                };
            }
        }
        
        private void ReadHeader(BinaryReader reader)
        {
            HeaderStartOffset = reader.BaseStream.Position;
            string magicNumber = reader.ReadString(4);
            if (magicNumber != "SAV3")
            {
                throw new InvalidOperationException();
            }

            TypeCode1 = reader.ReadInt32();
            if (TypeCode1 != 53)
            {
                throw new InvalidOperationException();
            }

            TypeCode2 = reader.ReadInt32();
            if (TypeCode2 != 9)
            {
                throw new InvalidOperationException();
            }

            TypeCode3 = reader.ReadInt32();
            if (TypeCode3 != 162)
            {
                throw new InvalidOperationException();
            }
        }

        //private byte[] ReadValue(BinaryReader reader)
        //{
        ////byte[] value = new byte[0];
        ////if ((Flags & 1) > 0)
        ////{
        ////    value = reader.ReadBytes(2);
        ////}
        ////else
        ////{
        ////    if ((Flags & 2) > 0)
        ////    {
        ////        if (TypeCode1 >= 27)
        ////        {
        ////            value = reader.ReadBytes(2);
        ////        }
        ////        else
        ////        {
        ////            if ((Flags & 0x100000) > 0)
        ////            {
        ////                value = reader.ReadBytes(4);
        ////                if ((Flags & 0x800000) > 0)
        ////                {
        ////                    // Lookup default value
        ////                }
        ////                // Read unicode string value
        ////            }
        ////        }
        ////    }
        ////}
        ////return value;
        //}
    }
}
