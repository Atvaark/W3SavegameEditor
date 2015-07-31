using System.IO;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class BrVariableParser : VariableParserBase<Variable>
    {
        public override string MagicNumber
        {
            get { return "BR"; }
        }

        public override Variable ParseImpl(BinaryReader reader, int size)
        {
            return Variable.None;
        }
    }
}
