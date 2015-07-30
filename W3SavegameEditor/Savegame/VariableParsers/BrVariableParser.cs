using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class BrVariableParser : VariableParserBase
    {
        public override string MagicNumber
        {
            get { return "BR"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {

        }
    }
}
