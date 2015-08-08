namespace W3SavegameEditor.Savegame.Variables
{
    public class VariableComposite : VariableBase
    {
        public VariableBase InnerVariable { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", base.ToString(), InnerVariable);
        }
    }
}
