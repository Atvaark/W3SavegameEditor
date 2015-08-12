using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using W3SavegameEditor.Core.ChunkedLz4;
using W3SavegameEditor.Core.Common;
using W3SavegameEditor.Core.Exceptions;
using W3SavegameEditor.Core.Savegame.VariableParsers;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame
{

    public class SavegameFile
    {
        private class RbEntry
        {
            public short Size { get; set; }
            public int Offset { get; set; }
        }

        [Flags]
        private enum SizeFlag
        {
            Unknown1 = 0x1,
            Unknown2 = 0x2,
            Unknown3 = 0x8000,
            Unknown4 = 0x1000000,
        }

        /// <summary>
        /// Flags that determine the size of certain values.
        /// </summary>
        private const SizeFlag DefaultFlags = SizeFlag.Unknown2 | SizeFlag.Unknown3 | SizeFlag.Unknown4;

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
        public string[] VariableNames { get; set; }
        public Variable[] Variables { get; set; }

        public static Task<SavegameFile> ReadAsync(
            string path,
            IReadSavegameProgress progress = null)
        {
            return Task.Run(() => Read(path, progress));
        }

        public static SavegameFile Read(
            string path,
            IReadSavegameProgress progress = null)
        {

            if (progress != null) progress.Report(true, true, 0, 0);
            using (var compressedInputStream = File.OpenRead(path))
            using (var inputStream = ChunkedLz4File.Decompress(compressedInputStream))
            using (var reader = new BinaryReader(inputStream, Encoding.ASCII, true))
            {
                var savegameFile = new SavegameFile();
                savegameFile.ReadHeader(reader);
                savegameFile.ReadFooter(reader);
                savegameFile.ReadStringTable(reader);
                savegameFile.ReadVariableTable(reader);
                if (progress != null) progress.Report(true, false, 0, savegameFile.VariableTableEntries.Length);
                savegameFile.ReadVariables(reader, progress);
                if (progress != null) progress.Report(false, false, 0, 0);
                return savegameFile;
            }
        }

        private void ReadVariables(BinaryReader reader, IReadSavegameProgress progress)
        {
            var parser = new VariableParser(VariableNames);
            var parsers = new List<VariableParserBase>
            {
                new ManuVariableParser(),
                new OpVariableParser(),
                new SsVariableParser(parser),
                new VlVariableParser(),
                new BsVariableParser(parser),
                new BlckVariableParser(parser),
                new AvalVariableParser(),
                new PorpVariableParser()
            };
            parser.RegisterParsers(parsers);

            Variable[] variables = new Variable[VariableTableEntries.Length];
            for (int i = 0; i < VariableTableEntries.Length; i++)
            {
                reader.BaseStream.Position = VariableTableEntries[i].Offset;
                try
                {
                    var size = VariableTableEntries[i].Size;
                    
                    // TODO: Add a max recursion parameter to stop stackoverflows for the moment. 
                    variables[i] = parser.Parse(reader, ref size);
                }
                catch (ParseVariableException e)
                {
                    Debug.WriteLine(e.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                if (i % 250 == 0 && progress != null) progress.Report(true, false, i, VariableTableEntries.Length);
            }

            Variables = variables;
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
            ReadNmSection(reader);
            ReadRbSection(reader);
            ReadVariableNameSection(reader);
        }

        private void ReadVariableNameSection(BinaryReader reader)
        {
            reader.BaseStream.Position = StringTableOffset;
            var manuVariableParser = new ManuVariableParser();
            var manuVariableSize = StringTableFooterOffset - StringTableOffset;
            manuVariableParser.Verify(reader, ref manuVariableSize);
            var manuVariable = manuVariableParser.ParseImpl(reader, ref manuVariableSize);
            VariableNames = manuVariable.Strings;
        }

        private void ReadNmSection(BinaryReader reader)
        {
            reader.BaseStream.Position = NmSectionOffset;
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "NM")
            {
                throw new InvalidOperationException();
            }
            StringTableOffset = (int)reader.BaseStream.Position;
        }

        private void ReadRbSection(BinaryReader reader)
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
            //if (TypeCode1 != 54)
            //{
            //    throw new InvalidOperationException();
            //}

            TypeCode2 = reader.ReadInt32();
            //if (TypeCode2 != 10)
            //{
            //    throw new InvalidOperationException();
            //}

            TypeCode3 = reader.ReadInt32();
            //if (TypeCode3 != 162)
            //{
            //    throw new InvalidOperationException();
            //}
        }
    }
}
