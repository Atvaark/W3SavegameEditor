using System.IO;
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
    }
}
