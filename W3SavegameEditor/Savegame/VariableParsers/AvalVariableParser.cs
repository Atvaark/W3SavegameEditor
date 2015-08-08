using System.IO;
using W3SavegameEditor.Exceptions;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class AvalVariableParser : VariableParserBase<AvalVariable>
    {
        private const string FullMagicNumber = "AVAL";
        
        public override string MagicNumber
        {
            get { return "AV"; }
        }

        public override AvalVariable ParseImpl(BinaryReader reader, ref int size)
        {
            short nameIndex = reader.ReadInt16();
            string name = Names[nameIndex - 1];
            short typeIndex = reader.ReadInt16();
            string type = Names[typeIndex - 1];
            size -= 2 * sizeof(short);

            int unknown = reader.ReadInt32();
            size -= sizeof(int);

            ReadData(reader, type, ref size);

            return new AvalVariable
            {
                Name = name,
                Type = type
            };
        }

        public override void Verify(BinaryReader reader, ref int size)
        {
            var bytesToRead = FullMagicNumber.Length;
            var magicNumber = reader.ReadString(bytesToRead);
            if (magicNumber != FullMagicNumber)
            {
                throw new ParseVariableException(
                    string.Format(
                    "Expeced AVAL but read {0} at {1}",
                    magicNumber,
                    reader.BaseStream.Position - 4));
            }

            size -= bytesToRead;
        }
    }
}
