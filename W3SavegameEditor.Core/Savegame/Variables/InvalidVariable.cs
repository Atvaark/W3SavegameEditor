namespace W3SavegameEditor.Core.Savegame.Variables
{
    public class InvalidVariable : Variable
    {
        private readonly string message;

        public InvalidVariable(string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return message;
        }
    }
}