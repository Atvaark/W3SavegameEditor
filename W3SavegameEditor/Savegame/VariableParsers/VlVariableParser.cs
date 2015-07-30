using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class VlVariableParser : VariableParserBase
    {
        public override string MagicNumber
        {
            get { return "VL"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {
            short unknown1 = reader.ReadInt16();
            short unknown2 = reader.ReadInt16();
            byte stringLength = reader.ReadByte();
            string value = reader.ReadString(stringLength & 127);
        }
    }
}
