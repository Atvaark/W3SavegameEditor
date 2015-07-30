using System;
using System.IO;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class ManuVariableParser : VariableParserBase
    {
        private const string FullMagicNumber = "MANU";

        public override string MagicNumber
        {
            get { return "MA"; }
        }

        public override void Parse(BinaryReader reader, int size)
        {

        }

        public override void Verify(BinaryReader reader)
        {
            var magicNumber = reader.ReadString(FullMagicNumber.Length);
            if (magicNumber != FullMagicNumber)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
