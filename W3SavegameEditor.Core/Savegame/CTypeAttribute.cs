using System;

namespace W3SavegameEditor.Core.Savegame
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class CTypeAttribute : Attribute
    {
        private string Type { get; set; }

        public CTypeAttribute(string type)
        {
            Type = type;
        }
    }
}