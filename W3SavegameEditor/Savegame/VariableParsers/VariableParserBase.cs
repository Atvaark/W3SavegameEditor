using System;
using System.IO;
using System.Text;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public abstract class VariableParserBase
    {
        public string[] Names { get; set; }

        public abstract string MagicNumber { get; }

        public abstract VariableBase Parse(BinaryReader reader, ref int size);

        public abstract int Verify(BinaryReader reader);
    }

    public abstract class VariableParserBase<T> : VariableParserBase where T : VariableBase
    {
        public override VariableBase Parse(BinaryReader reader, ref int size)
        {
            return ParseImpl(reader, ref size);
        }

        public abstract T ParseImpl(BinaryReader reader, ref int size);

        public override int Verify(BinaryReader reader)
        {
            var bytesToRead = MagicNumber.Length;
            var readMagicNumber = reader.ReadString(bytesToRead);
            if (readMagicNumber != MagicNumber)
            {
                throw new ParseVariableException(
                    string.Format(
                    "Expeced {0} but read {1} at {2}",
                    MagicNumber,
                    readMagicNumber,
                    reader.BaseStream.Position - bytesToRead));
            }
            return bytesToRead;
        }

        protected void ReadData(BinaryReader reader, string type, ref int size)
        {
            switch (type)
            {
                case "String":
                    {
                        var headerByte = reader.ReadByte();
                        size -= sizeof(byte);

                        bool encodedStringLength = (headerByte & 128) > 0;
                        if (encodedStringLength)
                        {
                            // HACK: Sometimes a single byte has to be skipped
                            if (reader.PeekByte() == 0x01)
                            {
                                reader.ReadByte();
                                size -= sizeof(byte);
                            }

                            byte stringLength = (byte)(headerByte & 127);
                            string value = reader.ReadString(stringLength);
                            size -= stringLength;
                        }
                        else
                        {
                            // TODO: Analyze how this can be read.
                            byte[] unknown = reader.ReadBytes(size);
                            size = 0;
                        }
                        break;
                    }
                case "StringAnsi":
                    {
                        var length = reader.ReadByte();
                        byte[] data = reader.ReadBytes(length);
                        size -= sizeof(byte) + length;
                        string value = Encoding.ASCII.GetString(data).TrimEnd(char.MinValue);
                        break;
                    }
                case "CName":
                    {
                        short cnameIndex = reader.ReadInt16();
                        size -= sizeof(short);
                        if (cnameIndex == 0 || cnameIndex > Names.Length)
                        {
                            break;
                        }

                        var value = Names[cnameIndex - 1];
                        break;
                    }
                case "CGUID":
                    {
                        var guidData = reader.ReadBytes(16);
                        size -= 16;
                        var value = new Guid(guidData);
                        break;
                    }
                case "EngineTime":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(3);
                        size -= 3;
                        break;
                    }
                case "GameTime":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(11);
                        size -= 11;
                        break;
                    }
                case "EntityHandle":
                    {
                        // TODO: Analyze how this can be read.
                        byte unknown1 = reader.ReadByte();
                        size -= sizeof(byte);
                        if (unknown1 > 0)
                        {
                            byte unknown2 = reader.ReadByte();
                            byte[] unknown3 = reader.ReadBytes(16);
                            size -= 17 * sizeof(byte);
                        }
                        break;
                    }
                case "SActionPointId":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(size);
                        size = 0;
                        break;
                    }
                case "EAIAttitude":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(size);
                        size = 0;
                        break;
                    }
                case "EJournalStatus":
                case "eGwintFaction":
                case "EZoneName":
                case "EDifficultyMode":
                    {
                        byte unknown1 = reader.ReadByte();
                        byte unknown2 = reader.ReadByte(); // Index
                        size -= 2 * sizeof(byte);
                        break;
                    }
                case "Vector":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(35);
                        size -= 35;
                        break;
                    }
                case "Vector2":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(19);
                        size -= 19;
                        break;
                    }
                case "IdTag":
                    {
                        // TODO: Analyze how this can be read.
                        byte unknown1 = reader.ReadByte();
                        int unknown2 = reader.ReadInt32();
                        int unknown3 = reader.ReadInt32();
                        int unknown4 = reader.ReadInt32();
                        int unknown5 = reader.ReadInt32();
                        size -= 17;
                        break;
                    }
                case "TagList":
                    {
                        byte tagListHeader = reader.ReadByte();
                        size -= sizeof(byte);
                        bool tagListFlag = (tagListHeader & 128) > 0;
                        byte tagListCount = (byte)(tagListHeader & 127);
                        short[] tagListEntries = new short[tagListCount];
                        for (int i = 0; i < tagListCount; i++)
                        {
                            tagListEntries[i] = reader.ReadInt16(); // NameIndex?
                        }
                        size -= tagListCount * sizeof(short);
                        break;
                    }
                case "SQuestThreadSuspensionData":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(size);
                        size = 0;
                        break;
                    }
                case "W3TutorialManagerUIHandler":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(size);
                        size = 0;
                        break;
                    }
                case "W3EnvironmentManager":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown = reader.ReadBytes(19);
                        size -= 19;
                        break;
                    }
                case "CEntityTemplate":
                    {
                        // Might just be the same format as "String"
                        byte headerByte = reader.ReadByte();
                        size -= sizeof(byte);

                        bool encodedStringLength = (headerByte & 128) > 0;
                        if (encodedStringLength)
                        {
                            // HACK: Sometimes a single byte has to be skipped
                            if (reader.PeekByte() == 0x01)
                            {
                                reader.ReadByte();
                                size -= sizeof(byte);
                            }

                            byte stringLength = (byte)(headerByte & 127);
                            string value = reader.ReadString(stringLength);
                            size -= stringLength;
                        }
                        else
                        {
                            // TODO: Analyze how this can be read.
                            byte[] unknown = reader.ReadBytes(size);
                            size = 0;
                        }
                        break;
                    }
                case "EulerAngles":
                    {
                        // TODO: Analyze how this can be read.
                        byte[] unknown4 = reader.ReadBytes(3);
                        double unknown1 = reader.ReadDouble();
                        double unknown2 = reader.ReadDouble();
                        double unknown3 = reader.ReadDouble();

                        size -= 3 + 3 * sizeof(double);
                        break;
                    }
                case "Bool":
                    {
                        bool value = reader.ReadBoolean();
                        size -= sizeof(bool);
                        break;
                    }
                case "Uint8":
                    {
                        byte value = reader.ReadByte();
                        size -= sizeof(byte);
                        break;
                    }
                case "Uint16":
                    {
                        ushort value = reader.ReadUInt16();
                        size -= sizeof(ushort);
                        break;
                    }
                case "Uint32":
                    {
                        uint value = reader.ReadUInt32();
                        size -= sizeof(uint);
                        break;
                    }
                case "Uint64":
                    {
                        ulong value = reader.ReadUInt64();
                        size -= sizeof(ulong);
                        break;
                    }
                case "Int8":
                    {
                        sbyte value = reader.ReadSByte();
                        size -= sizeof(sbyte);
                        break;
                    }
                case "Int16":
                    {
                        short value = reader.ReadInt16();
                        size -= sizeof(short);
                        break;
                    }
                case "Int32":
                    {
                        int value = reader.ReadInt32();
                        size -= sizeof(int);
                        break;
                    }
                case "Int64":
                    {
                        long value = reader.ReadInt64();
                        size -= sizeof(long);
                        break;
                    }
                case "Double":
                    {
                        double value = reader.ReadDouble();
                        size -= sizeof(double);
                        break;
                    }
                case "Float":
                    {
                        float value = reader.ReadSingle();
                        size -= sizeof(double);
                        break;
                    }
                default:
                    {
                        if (type.StartsWith("array:2,0,"))
                        {
                            var arrayElementType = type.Substring("array:2,0,".Length);

                            int arrayLength = reader.ReadInt32();
                            size -= sizeof(int);

                            for (int i = 0; i < arrayLength; i++)
                            {
                                ReadData(reader, arrayElementType, ref size);
                            }
                        }
                        else if (type.StartsWith("handle:"))
                        {
                            var handleType = type.Substring("handle:".Length);
                            ReadData(reader, handleType, ref size);
                        }
                        else if (type.StartsWith("soft:"))
                        {
                            var handleType = type.Substring("soft:".Length);
                            ReadData(reader, handleType, ref size);
                        }
                        else
                        {
                        }
                        break;
                    }

            }

        }
    }
}
