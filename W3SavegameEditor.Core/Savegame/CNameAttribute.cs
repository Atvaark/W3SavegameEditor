using System;

namespace W3SavegameEditor.Core.Savegame
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class CNameAttribute : Attribute
    {
        private string Name { get; set; }

        public CNameAttribute(string name)
        {
            Name = name;
        }
    }
}
