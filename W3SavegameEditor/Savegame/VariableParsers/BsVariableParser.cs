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

        public override BsVariable ParseImpl(BinaryReader reader, int size)
        {
            short nameStringIndex = reader.ReadInt16();
            string name = Names[nameStringIndex - 1];

            // TODO: size is sum of all inner variables and not the one of the next variable

            size -= sizeof(short);

            VariableBase dbgLastVariable = null;
            int dbgVariableIndex = 0;
            long dbgLoopStartPos = reader.BaseStream.Position;
            if (121830 == dbgLoopStartPos)
            {
                
            }

            while (size > 0)
            {
                long variableStartPosition = reader.BaseStream.Position;
                var newVariable = _parser.Parse(reader, size);
                if (newVariable == Variable.None)
                {
                    break;
                }
                dbgLastVariable = newVariable;

                size -= (int)(reader.BaseStream.Position - variableStartPosition);
                dbgVariableIndex++;
            }


            return new BsVariable
            {
                Name = name,
                InnerVariable = dbgLastVariable
            };
        }
    }
}
