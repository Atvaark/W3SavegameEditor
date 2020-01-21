using System.IO;
using W3SavegameEditor.Core.Exceptions;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame.VariableParsers
{
    public class AvalVariableParser : VariableParserBase<AvalVariable>
    {
        public override string MagicNumber => "AVAL";

        public override AvalVariable ParseImpl(BinaryReader reader, ref int size)
        {
            short nameIndex = reader.ReadInt16();
            string name = Names[nameIndex - 1];
            short typeIndex = reader.ReadInt16();
            string type = Names[typeIndex - 1];
            size -= 2 * sizeof(short);

            int unknown = reader.ReadInt32();
            size -= sizeof(int);

            var value = ReadValue(reader, type, ref size);

            return new AvalVariable
            {
                Name = name,
                Type = type,
                Value = value
            };
        }
    }
}
