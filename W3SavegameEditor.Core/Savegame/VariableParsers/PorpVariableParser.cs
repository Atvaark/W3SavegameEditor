using System.Diagnostics;
using System.IO;
using W3SavegameEditor.Core.Exceptions;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame.VariableParsers
{
    public class PorpVariableParser : VariableParserBase<PorpVariable>
    {
        public override string MagicNumber => "PORP";

        public override PorpVariable ParseImpl(BinaryReader reader, ref int size)
        {
            short nameIndex = reader.ReadInt16();
            string name = Names[nameIndex - 1];

            short typeIndex = reader.ReadInt16();
            string type = Names[typeIndex - 1];

            size -= 2 * sizeof(short);
            
            int valueSize = reader.ReadInt32();
            size -= sizeof(int);

            int readValueSize = valueSize;
            var value = ReadValue(reader, type, ref readValueSize);
            size -= valueSize;
            Debug.Assert(readValueSize == 0);

            return new PorpVariable
            {
                Name = name,
                Type = type,
                Value = value
            };
        }
    }
}
