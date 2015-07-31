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

            var innerVariable = _parser.Parse(reader, MagicNumber.Length - sizeof(short));

            return new BsVariable
            {
                NameStringIndex = nameStringIndex,
                InnerVariable = innerVariable
            };
        }
    }
}
