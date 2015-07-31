using System;
using System.IO;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class ManuVariableParser : VariableParserBase<ManuVariable>
    {
        private const string FullMagicNumber = "MANU";

        public override string MagicNumber
        {
            get { return "MA"; }
        }

        public override ManuVariable ParseImpl(BinaryReader reader, int size)
        {
            int stringCount = reader.ReadInt32();
            reader.Skip(4);

            var strings = new string[stringCount];
            for (int i = 0; i < stringCount; i++)
            {
                byte stringSize = reader.ReadByte();
                strings[i] = reader.ReadString(stringSize);
            }

            reader.Skip(4);
            string doneMagicNumber = reader.ReadString(4);
            if (doneMagicNumber != "ENOD")
            {
                throw new ParseVariableException();
            }
            
            return new ManuVariable
            {
                Strings = strings
            };
        }

        public override void Verify(BinaryReader reader)
        {
            var magicNumber = reader.ReadString(FullMagicNumber.Length);
            if (magicNumber != FullMagicNumber)
            {
                throw new ParseVariableException();
            }
        }
    }
}
