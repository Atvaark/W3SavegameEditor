using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using W3SavegameEditor.Core.Exceptions;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame.VariableParsers
{
    public class SsVariableParser : VariableParserBase<SsVariable>
    {
        private readonly VariableParser _parser;

        public SsVariableParser(VariableParser parser)
        {
            _parser = parser;
        }

        public override string MagicNumber => "SS";

        public override void Verify(BinaryReader reader, ref int size)
        {
            base.Verify(reader, ref size);

            var position = reader.BaseStream.Position;
            var sizeInner = reader.ReadInt32();
            reader.BaseStream.Position = position;
            var expectedSize = sizeof(int) + sizeInner;

            if (size != expectedSize)
                throw new ParseVariableException($"SSVariable: Expected to read {expectedSize} bytes but found {size} at {reader.BaseStream.Position}");
        }

        public override SsVariable ParseImpl(BinaryReader reader, ref int size)
        {
            int sizeInner = reader.ReadInt32();
            size -= sizeof(int);
            Debug.Assert(sizeInner == size);

            List<Variable> variables = new List<Variable>(); 
            while (size > 0)
            {
                var variable = _parser.Parse(reader, ref size);
                variables.Add(variable);
            }

            Debug.Assert(size == 0);

            return new SsVariable
            {
                Name = "None",
                Variables = variables.ToArray()
            };
        }
    }
}
