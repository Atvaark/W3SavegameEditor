namespace W3SavegameEditor.Savegame.Variables
{
    public abstract class Variable
    {
        public string Name { get; set; }

        public Variable()
        {
            Name = "None";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
