namespace W3SavegameEditor.Savegame.Variables
{
    public class ManuVariable : VariableBase
    {
        public string[] Strings { get; set; }
        public override string ToString()
        {
            return string.Format("MANU({0}) {1}", (Strings == null ? "null" : Strings.Length.ToString()), base.ToString());
        }
    }
}