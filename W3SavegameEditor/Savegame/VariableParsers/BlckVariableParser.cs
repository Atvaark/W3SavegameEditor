using System.IO;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class BlckVariableParser : VariableParserBase<BlckVariable>
    {
        private const string FullMagicNumber = "BLCK";

        private readonly VariableParser _parser;
        
        public BlckVariableParser(VariableParser parser)
        {
            _parser = parser;
        }

        public override string MagicNumber
        {
            get { return "BL"; }
        }

        public override BlckVariable ParseImpl(BinaryReader reader, ref int size)
        {
            ushort unknown1 = reader.ReadUInt16();
            ushort blckSize = reader.ReadUInt16();
            ushort unknown3 = reader.ReadUInt16();
            size -= 3 * sizeof(short);

            // TODO: Only read blckSize
            VariableBase lastVariable = null;
            long startPos = reader.BaseStream.Position;
            if (startPos == 4007431)
            {
                
            }

            int i = 0;
            while (size > 0)
            {
                lastVariable = _parser.Parse(reader, ref size);
                i++;
            }

            if (size < 0)
            {
                
            }

            reader.ReadBytes(size);
            size = 0;
            return new BlckVariable
            {
                Name = "None"
            };
        }

        public override void Verify(BinaryReader reader, ref int size)
        {
            var bytesToRead = FullMagicNumber.Length;
            var magicNumber = reader.ReadString(bytesToRead);
            if (magicNumber != FullMagicNumber)
            {
                throw new ParseVariableException(
                    string.Format(
                    "Expeced BLCK but read {0} at {1}",
                    magicNumber,
                    reader.BaseStream.Position - 4));
            }

            size -= bytesToRead;
        }
    }
}
