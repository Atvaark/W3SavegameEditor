using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class OpVariableParser : VariableParserBase
    {
        public override string MagicNumber
        {
            get { return "OP"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {
            short opCode1 = reader.ReadInt16();
            short opCode2 = reader.ReadInt16();
            byte opCode3 = reader.ReadByte();
        }
    }
}
