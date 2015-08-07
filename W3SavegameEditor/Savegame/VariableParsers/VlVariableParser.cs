using System.IO;
using System.Linq;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Savegame.VariableParsers
{
    public class VlVariableParser : VariableParserBase<VlVariable>
    {
        public override string MagicNumber
        {
            get { return "VL"; }
        }

        public override VlVariable ParseImpl(BinaryReader reader, int size)
        {
            short nameIndex = reader.ReadInt16();
            string name = Names[nameIndex - 1]; 
            
            short typeIndex = reader.ReadInt16();
            string type = Names[typeIndex - 1];
            byte stringLength = (byte)(reader.ReadByte() & 127);

            if (stringLength == size - 5)
            {
                string value = reader.ReadString(stringLength);
                if (stringLength > 0)
                {

                }
            }

            return new VlVariable
            {
                Name = name,
                Type = type
            };
        }
    }
}
