using System.IO;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class BsVariableParser : VariableParserBase<BsVariable>
    {
        private readonly VariableParser _parser;

        public BsVariableParser(VariableParser parser)
        {
            _parser = parser;
        }

        public override string MagicNumber
        {
            get { return "BS"; }
        }

        public override BsVariable ParseImpl(BinaryReader reader, int size)
        {
            short nameStringIndex = reader.ReadInt16();
            string name = Names[nameStringIndex - 1];
            var innerVariable = _parser.Parse(reader, size - sizeof(short));

            return new BsVariable
            {
                Name = name,
                InnerVariable = innerVariable
            };
        }
    }
}
