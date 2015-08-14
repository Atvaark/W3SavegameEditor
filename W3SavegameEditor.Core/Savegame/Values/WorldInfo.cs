using System;

namespace W3SavegameEditor.Core.Savegame.Values
{
    [CName("worldInfo")]
    public class WorldInfo
    {
        [CName("world")]
        public String World { get; set; }
    }
}