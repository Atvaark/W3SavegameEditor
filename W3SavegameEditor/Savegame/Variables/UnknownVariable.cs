namespace W3SavegameEditor.Savegame.Variables
{
    public class UnknownVariable : VariableBase
    {
        public static readonly UnknownVariable None = new UnknownVariable { Name = "None" };

        public byte[] Data { get; set; }
    }
}