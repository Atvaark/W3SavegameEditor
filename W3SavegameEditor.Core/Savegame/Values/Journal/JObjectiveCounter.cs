using System;

namespace W3SavegameEditor.Core.Savegame.Values.Journal
{
    public class JObjectiveCounter
    {
        [CName("guid")]
        public Guid Guid { get; set; }

        [CName("count")]
        public int Count { get; set; }
    }
}