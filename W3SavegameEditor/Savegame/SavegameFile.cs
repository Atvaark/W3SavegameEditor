using System;
using System.IO;
using System.Text;

namespace W3SavegameEditor.Savegame
{
    public class SavegameFile
    {
        public int VariableTableOffset { get; set; }
        public SavegameVariableTableEntry[] VariableTableEntries { get; set; }
        public string[] Strings { get; set; }

        public static SavegameFile Read(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.ASCII, true))
            {
                var savegameFile = new SavegameFile();
                savegameFile.ReadFooter(reader);
                savegameFile.ReadVariableTable(reader);
                savegameFile.ReadVariables(reader);

                //savegameFile.ReadHeader(reader);
                //savegameFile.ReadVariables(reader);
                savegameFile.ReadStringTable(reader);
                return savegameFile;
            }
        }

        private void ReadVariables(BinaryReader reader)
        {
            foreach (var tableEntry in VariableTableEntries)
            {
                reader.BaseStream.Position = tableEntry.Offset;

                string peek = reader.PeekString(2);

                switch (peek)
                {
                    case "VL":
                        // byte size2 = reader.ReadByte();
                        // string value = reader.ReadString(size2 & 127);
                        break;
                    case "BS":
                        // byte bsCode1 = reader.ReadByte();
                        // byte bsCode2 = reader.ReadByte();
                        // ReadVariable(reader);
                        break;
                    case "OP":
                        // short opCode1 = reader.ReadInt16();
                        // short opCode2 = reader.ReadInt16();
                        // byte opCode3 = reader.ReadByte();
                        // break;
                        break;
                    case "SS":
                        break;
                    case "BR":
                        break;
                    case "MA":
                        if (reader.PeekString(4) == "MANU")
                        {
                            ReadStringTable(reader);
                        }
                        break;
                    default:
                        // int size1 = reader.ReadInt32();
                        // reader.Skip(2*size1);
                        break;
                }

            }
        }

        private void ReadVariableTable(BinaryReader reader)
        {
            reader.BaseStream.Seek(VariableTableOffset, SeekOrigin.Begin);
            int entryCount = reader.ReadInt32();
            SavegameVariableTableEntry[] entires = new SavegameVariableTableEntry[entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                entires[i] = new SavegameVariableTableEntry
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
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "SE")
            {
                throw new InvalidOperationException();
            }
        }
        
        private void ReadStringTable(BinaryReader reader)
        {
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

            int nmVariableOffset = reader.ReadInt32();
            int rbVariableOffset = reader.ReadInt32();

            reader.BaseStream.Position = nmVariableOffset;
            ReadNmVariable(reader);
            reader.BaseStream.Position = rbVariableOffset;
            ReadRbVariable(reader);
        }

        private void ReadNmVariable(BinaryReader reader)
        {
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "NM")
            {
                throw new InvalidOperationException();
            }
        }

        private void ReadRbVariable(BinaryReader reader)
        {
            string magicNumber = reader.ReadString(2);
            if (magicNumber != "RB")
            {
                throw new InvalidOperationException();
            }
            int count = reader.ReadInt32();
        }

        private void ReadHeader(BinaryReader reader)
        {
            string magicNumber = reader.ReadString(4);
            if (magicNumber != "SAV3")
            {
                throw new InvalidOperationException();
            }

            int typeCode1 = reader.ReadInt32();
            if (typeCode1 != 53)
            {
                throw new InvalidOperationException();
            }

            int typeCode2 = reader.ReadInt32();
            if (typeCode2 != 9)
            {
                throw new InvalidOperationException();
            }

            int typeCode3 = reader.ReadInt32();
            if (typeCode3 != 162)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
