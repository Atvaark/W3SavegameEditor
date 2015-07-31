using System.IO;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class VlVariableParser : VariableParserBase<VlVariable>
    {
        public override string MagicNumber
        {
            get { return "VL"; }
        }

        public override VlVariable ParseImpl(BinaryReader reader, int size)
        {
            short nameStringIndex = reader.ReadInt16();
            short unknown2 = reader.ReadInt16();
            byte stringLength = reader.ReadByte();
            string value = reader.ReadString(stringLength & 127);
            return new VlVariable
            {
                NameStringIndex = nameStringIndex
            };
        }
    }
}
