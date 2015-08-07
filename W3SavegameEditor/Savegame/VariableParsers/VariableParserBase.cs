using System;
using System.IO;
using System.Text;
using System.Xml.Schema;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public abstract class VariableParserBase
    {
        public string[] Names { get; set; }

        public abstract string MagicNumber { get; }

        public abstract VariableBase Parse(BinaryReader reader, int size);

        public abstract int Verify(BinaryReader reader);
    }

    public abstract class VariableParserBase<T> : VariableParserBase where T : VariableBase
    {
        public override VariableBase Parse(BinaryReader reader, int size)
        {
            return ParseImpl(reader, size);
        }

        public abstract T ParseImpl(BinaryReader reader, int size);

        public override int Verify(BinaryReader reader)
        {
            var bytesToRead = MagicNumber.Length;
            var magicNumber = reader.ReadString(bytesToRead);
            if (magicNumber != MagicNumber)
            {
                throw new ParseVariableException(string.Format("Read {0} while expecting {1}", magicNumber, MagicNumber));
            }
            return bytesToRead;
        }

        protected void ReadData(BinaryReader reader, string type, ref int size)
        {
            if (type == "String")
            {
                var headerByte = reader.ReadByte();
                size -= sizeof(byte);

                bool encodedStringLength = (headerByte & 128) > 0;
                if (encodedStringLength)
                {
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
            }
            else if (type == "StringAnsi")
            {
                var length = reader.ReadByte();
                byte[] data = reader.ReadBytes(length);
                size -= sizeof(byte) + length;

                string value = Encoding.ASCII.GetString(data).TrimEnd(char.MinValue);
            }
            else if (type == "CName")
            {
                short cnameIndex = reader.ReadInt16();
                size -= sizeof(short);
                var value = Names[cnameIndex];
            }
            else if (type == "CGUID")
            {
                var guidData = reader.ReadBytes(16);
                size -= 16;
                var value = new Guid(guidData);
            }
            else if (type == "EngineTime")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(size);
                size = 0;
            }
            else if (type == "GameTime")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(size);
                size = 0;
            }
            else if (type == "EntityHandle")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(size);
                size = 0;
            }
            else if (type == "SActionPointId")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(size);
                size = 0;
            }
            else if (type == "EAIAttitude")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(size);
                size = 0;
            }
            else if (type == "EJournalStatus")
            {
                short value = reader.ReadInt16();
                size -= sizeof(short);
            }
            else if (type == "eGwintFaction")
            {
                short value = reader.ReadInt16();
                size -= sizeof(short);
            }
            else if (type == "EZoneName")
            {
                short value = reader.ReadInt16();
                size -= sizeof(short);
            }
            else if (type == "Vector")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(35);
                size -= 35;
            }
            else if (type == "Vector2")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(19);
                size -= 19;
            }
            else if (type == "IdTag")
            {
                // TODO: Analyze how this can be read.
                byte unknown1 = reader.ReadByte();
                int unknown2 = reader.ReadInt32();
                int unknown3 = reader.ReadInt32();
                int unknown4 = reader.ReadInt32();
                int unknown5 = reader.ReadInt32();
                size -= 17;
            }
            else if (type == "TagList")
            {
                byte value = reader.ReadByte();
                size -= sizeof(byte);
            }
            else if (type == "W3TutorialManagerUIHandler")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown = reader.ReadBytes(size);
                size = 0;
            }
            else if (type == "CEntityTemplate")
            {
                byte headerByte1 = reader.ReadByte();
                byte headerByte2 = reader.ReadByte();
                size -= 2 * sizeof(byte);

                byte stringLength = (byte)(headerByte1 & 127);
                string value = reader.ReadString(stringLength);
                size -= stringLength;
            }
            else if (type == "EulerAngles")
            {
                // TODO: Analyze how this can be read.
                byte[] unknown4 = reader.ReadBytes(3);
                double unknown1 = reader.ReadDouble();
                double unknown2 = reader.ReadDouble();
                double unknown3 = reader.ReadDouble();

                size -= 3 * sizeof(double) + 3;
            }
            else if (type == "Bool")
            {
                bool value = reader.ReadBoolean();
                size -= sizeof(bool);
            }
            else if (type == "Uint8")
            {
                byte value = reader.ReadByte();
                size -= sizeof(byte);
            }
            else if (type == "Uint16")
            {
                ushort value = reader.ReadUInt16();
                size -= sizeof(ushort);
            }
            else if (type == "Uint32")
            {
                uint value = reader.ReadUInt32();
                size -= sizeof(uint);
            }
            else if (type == "Uint64")
            {
                ulong value = reader.ReadUInt64();
                size -= sizeof(ulong);
            }
            else if (type == "Int8")
            {
                sbyte value = reader.ReadSByte();
                size -= sizeof(sbyte);
            }
            else if (type == "Int16")
            {
                short value = reader.ReadInt16();
                size -= sizeof(short);
            }
            else if (type == "Int32")
            {
                int value = reader.ReadInt32();
                size -= sizeof(int);
            }
            else if (type == "Int64")
            {
                long value = reader.ReadInt64();
                size -= sizeof(long);
            }
            else if (type == "Double")
            {
                double value = reader.ReadDouble();
                size -= sizeof(double);
            }
            else if (type == "Float")
            {
                float value = reader.ReadSingle();
                size -= sizeof(double);
            }
            else
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
            }

        }
    }
}
