﻿using System;
using W3SavegameEditor.Core.Savegame.Attributes;

namespace W3SavegameEditor.Core.Savegame.Values.Journal
{
    public class JHighlightedObjective
    {
        [CName("guid")]
        public Guid Guid { get; set; }
    }
}