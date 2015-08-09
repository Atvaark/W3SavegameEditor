namespace W3SavegameEditor.Savegame.Variables
{
    public class VariableSet : VariableBase
    {
        public VariableBase[] Variables { get; set; }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", base.ToString(), Variables.Length);
        }
    }
}
