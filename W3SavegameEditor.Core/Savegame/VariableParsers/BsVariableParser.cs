using System.Diagnostics;
using System.IO;
using W3SavegameEditor.Core.Exceptions;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame.VariableParsers
{
    /// <summary>
    /// A set of variables
    /// </summary>
    public class BsVariableParser : VariableParserBase<BsVariable>
    {
        private readonly VariableParser _parser;

        public BsVariableParser(VariableParser parser)
        {
            _parser = parser;
        }

        public override string MagicNumber => "BS";

        public override void Verify(BinaryReader reader, ref int size)
        {
            base.Verify(reader, ref size);

            const int expectedSize = sizeof(short);
            if (size != expectedSize)
                throw new ParseVariableException($"BSVariable: Expected to read {expectedSize} bytes but found {size} at {reader.BaseStream.Position}");
        }

        public override BsVariable ParseImpl(BinaryReader reader, ref int size)
        {
            short nameStringIndex = reader.ReadInt16();
            string name = Names[nameStringIndex - 1];
            size -= sizeof(short);
            Debug.Assert(size == 0);
            
            return new BsVariable
            {
                Name = name,
                Variables = new Variable[0]
            };
        }
    }
}
