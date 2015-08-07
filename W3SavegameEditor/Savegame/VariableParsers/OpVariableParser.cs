using System.IO;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class OpVariableParser : VariableParserBase<OpVariable>
    {
        public override string MagicNumber
        {
            get { return "OP"; }
        }

        public override OpVariable ParseImpl(BinaryReader reader, int size)
        {
            short nameStringIndex = reader.ReadInt16();
            string name = Names[nameStringIndex - 1];
            short opCode2 = reader.ReadInt16();
            byte opCode3 = reader.ReadByte();
            return new OpVariable
            {
                Name = name
            };
        }
    }
}
