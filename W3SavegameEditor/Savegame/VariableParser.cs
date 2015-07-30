using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using W3SavegameEditor.Savegame.VariableParsers;

namespace W3SavegameEditor.Savegame
{
    public class VariableParser
    {
        private readonly Dictionary<string, VariableParserBase> _parsers;

        public VariableParser(IEnumerable<VariableParserBase> parsers)
        {
            _parsers = parsers.ToDictionary(p => p.MagicNumber, p => p);
        }

        public void Parse(BinaryReader reader, int size)
        {
            string magicNumber = reader.PeekString(2);

            VariableParserBase parser;
            if (_parsers.TryGetValue(magicNumber, out parser))
            {
                parser.Verify(reader);
                parser.Parse(reader, size);
            }
            else
            {
                Debug.WriteLine(
                    "Failed to parse {0} bytes of data at {1}. Magic number was {2}",
                    size,
                    reader.BaseStream.Position,
                    magicNumber);
            }
        }
    }

}
