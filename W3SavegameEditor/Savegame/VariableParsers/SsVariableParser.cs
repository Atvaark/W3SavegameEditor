using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class SsVariableParser : VariableParserBase
    {
        public override string MagicNumber
        {
            get { return "SS"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {

        }
    }
}
