using System.Diagnostics;
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
            int sizeInner = reader.ReadInt32();
            // TODO: Parse inner values
            byte[] data = reader.ReadBytes(sizeInner);
            size -= sizeInner;
            
            return new SsVariable
            {

            };
        }
    }
}
