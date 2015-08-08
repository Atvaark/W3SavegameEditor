namespace W3SavegameEditor.Savegame.Variables
{
    /// <summary>
    /// A single variable
    /// </summary>
    public class VlVariable : TypedVariable
    {
        public override string ToString()
        {
            return "VL " +  base.ToString();
        }
    }
}