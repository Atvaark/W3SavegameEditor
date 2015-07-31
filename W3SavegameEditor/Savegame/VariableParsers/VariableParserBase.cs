using System;
using System.IO;
using W3SavegameEditor.Exceptions;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public abstract class VariableParserBase
    {
        public abstract string MagicNumber { get; }
        public abstract void Parse(BinaryReader reader, int size);

        public virtual void Verify(BinaryReader reader)
        {
            var magicNumber = reader.ReadString(MagicNumber.Length);
            if (magicNumber != MagicNumber)
            {
                throw new ParseVariableException(string.Format("Read {0} while expecting {1}", magicNumber, MagicNumber));
            }
        }
    }
}
