namespace W3SavegameEditor.Savegame.Variables
{
    public class OpVariable : VariableBase
    {
        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("OP {0} {1}", Type, base.ToString());
        }
    }
}