namespace W3SavegameEditor.Core.Savegame.Variables
{
    public class SsVariable : Variable
    {
        public Variable Variable { get; set; }

        public override string ToString()
        {
            return string.Format("SS {0} {1}", base.ToString(), Variable);
        }

    }
}