using System.Diagnostics;
using System.IO;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
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

        public override string MagicNumber
        {
            get { return "BS"; }
        }

        public override BsVariable ParseImpl(BinaryReader reader, ref int size)
        {
            short nameStringIndex = reader.ReadInt16();
            string name = Names[nameStringIndex - 1];
            
            size -= sizeof(short);

            // BUG: Huge sections (200000 bytes+) will cause a stackoverflow
            switch (name)
            {
                case "communities":
                case "community":
                case "JActiveEntries":
                case "CJournalManager":
                    return new BsVariable
                    {
                        Name = name,
                        InnerVariable = null
                    };
            }

            VariableBase debugLastVariable = null;
            int debugVariableIndex = 0;
            long debugLoopStartPos = reader.BaseStream.Position;
            while (size > 0)
            {
                long variableStartPosition = reader.BaseStream.Position;
                var newVariable = _parser.Parse(reader, ref size);
                if (newVariable.GetType() == typeof(UnknownVariable))
                {
                    break;
                }
                debugLastVariable = newVariable;

                Debug.Assert(reader.BaseStream.Position != variableStartPosition);
                debugVariableIndex++;
            }
            
            return new BsVariable
            {
                Name = name,
                InnerVariable = debugLastVariable
            };
        }
    }
}
