using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class BsVariableParser : VariableParserBase
    {
        private readonly VlVariableParser _vlVariableParser = new VlVariableParser();

        public override string MagicNumber
        {
            get { return "BS"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {
            byte bsCode1 = reader.ReadByte();
            byte bsCode2 = reader.ReadByte();
            _vlVariableParser.Verify(reader);
            _vlVariableParser.Parse(reader, size - MagicNumber.Length - 2);
        }
    }
}
