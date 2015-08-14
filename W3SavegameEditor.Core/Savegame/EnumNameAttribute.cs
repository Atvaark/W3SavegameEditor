using System;

namespace W3SavegameEditor.Core.Savegame
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property)]
    public class EnumNameAttribute : Attribute
    {
        private string Name { get; set; }

        public EnumNameAttribute(string name)
        {
            Name = name;
        }
    }
}