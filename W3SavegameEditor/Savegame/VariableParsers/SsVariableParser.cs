using System.IO;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class SsVariableParser : VariableParserBase<SsVariable>
    {
        public override string MagicNumber
        {
            get { return "SS"; }
        }

        public override SsVariable ParseImpl(BinaryReader reader, int size)
        {
            return new SsVariable
            {

            };
        }
    }
}
