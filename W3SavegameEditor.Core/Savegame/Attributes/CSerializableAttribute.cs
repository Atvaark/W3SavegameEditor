using System;

namespace W3SavegameEditor.Core.Savegame.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CSerializableAttribute : Attribute
    {
        public string Type { get; set; }

        public CSerializableAttribute(string type)
        {
            Type = type;
        }
    }
}