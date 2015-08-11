using System.IO;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame.VariableParsers
{
    public class VlVariableParser : VariableParserBase<VlVariable>
    {
        public override string MagicNumber
        {
            get { return "VL"; }
        }

        public override VlVariable ParseImpl(BinaryReader reader, ref int size)
        {
            short nameIndex = reader.ReadInt16();
            string name = Names[nameIndex - 1];

            short typeIndex = reader.ReadInt16();
            string type = Names[typeIndex - 1];

            size -= 2 * sizeof(short);

            var value = ReadValue(reader, type, ref size);

            return new VlVariable
            {
                Name = name,
                Type = type,
                Value = value
            };
        }
    }
}
