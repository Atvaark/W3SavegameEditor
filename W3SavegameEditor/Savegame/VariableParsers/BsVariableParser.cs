using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class BsVariableParser : VariableParserBase
    {
        private readonly VariableParser _parser;

        public BsVariableParser(VariableParser parser)
        {
            _parser = parser;
        }

        public override string MagicNumber
        {
            get { return "BS"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {
            byte bsCode1 = reader.ReadByte();
            byte bsCode2 = reader.ReadByte();

            _parser.Parse(reader, MagicNumber.Length - 2);
        }
    }
}
