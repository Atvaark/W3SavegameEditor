namespace W3SavegameEditor.Savegame.Variables
{
    public abstract class VariableBase
    {
        public string Name { get; set; }

        public VariableBase()
        {
            Name = "None";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
