using System.IO;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class SsVariableParser : VariableParserBase<SsVariable>
    {
        private readonly VariableParser _parser;

        public SsVariableParser(VariableParser parser)
        {
            _parser = parser;
        }

        public override string MagicNumber
        {
            get { return "SS"; }
        }

        public override SsVariable ParseImpl(BinaryReader reader, ref int size)
        {
            int sizeInner = reader.ReadInt32();
            size -= sizeof(int);
            
            ParseSxap(reader, ref size);
            var variable =  _parser.Parse(reader, ref size);
            return new SsVariable
            {
                Name = "None",
                Variable = variable
            };
        }

        private void ParseSxap(BinaryReader reader, ref int size)
        {
            string magicNumber = reader.ReadString(4);
            size -= 4;
            if (magicNumber != "SXAP")
            {
                throw new ParseVariableException(
                    string.Format(
                    "Expeced SXAP but read {0} at {1}",
                    magicNumber,
                    reader.BaseStream.Position - 4));
            }
            int typeCode1 = reader.ReadInt32();
            int typeCode2 = reader.ReadInt32();
            int typeCode3 = reader.ReadInt32();
            size -= 3 * sizeof(int);
        }
    }
}
