using System.Collections.Generic;
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

            // HACK: Huge sections (200000 bytes+) will cause a stackoverflow
            switch (name)
            {
                case "CJournalManager":
                case "JActiveEntries":
                case "communities":
                case "community":
                case "stubs":
                    return new BsVariable
                    {
                        Name = name,
                        Variables = new Variable[0]
                    };
            }

            List<Variable> variables = new List<Variable>();
            Variable debugLastVariable = null;
            int debugVariableIndex = 0;
            long debugLoopStartPos = reader.BaseStream.Position;
            while (size > 0)
            {
                // HACK: This is just for easy debugging.
                if (debugLoopStartPos == 0 && debugVariableIndex == 0)
                {

                }

                long variableStartPosition = reader.BaseStream.Position;
                var variable = _parser.Parse(reader, ref size);
                variables.Add(variable);

                if (variable.GetType() == typeof(UnknownVariable))
                {
                    break;
                }
                debugLastVariable = variable;
                debugVariableIndex++;
                Debug.Assert(reader.BaseStream.Position != variableStartPosition);
            }

            return new BsVariable
            {
                Name = name,
                Variables = variables.ToArray()
            };
        }
    }
}
