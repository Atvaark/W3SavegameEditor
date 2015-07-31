using System.Diagnostics;

namespace W3SavegameEditor.Savegame.Variables
{
    public abstract class VariableBase
    {
        public short? NameStringIndex { get; set; }
        public string Name { get; set; }

        public VariableBase()
        {
            Name = "None";
        }

        public virtual void ResolveNames(string[] names)
        {
            if (NameStringIndex.HasValue)
            {
                Debug.Assert(NameStringIndex.Value < names.Length);
                Name = names[NameStringIndex.Value];
            }
        }
    }
}
