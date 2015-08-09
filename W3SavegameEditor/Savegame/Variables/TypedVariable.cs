namespace W3SavegameEditor.Savegame.Variables
{
    public class TypedVariable : VariableBase
    {
        public string Type { get; set; }
        public VariableValue Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Type, base.ToString(), Value);
        }
    }
}