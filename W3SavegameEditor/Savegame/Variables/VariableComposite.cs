namespace W3SavegameEditor.Savegame.Variables
{
    public class VariableComposite : VariableBase
    {
        public VariableBase InnerVariable { get; set; }

        public override void ResolveNames(string[] names)
        {
            if (InnerVariable != null)
            {
                InnerVariable.ResolveNames(names);
            }

            base.ResolveNames(names);
        }
    }
}
