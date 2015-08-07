namespace W3SavegameEditor.Savegame.Variables
{
    public class VlVariable : VariableBase
    {
        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("VL {0} {1}", Type, base.ToString());
        }

    }
}