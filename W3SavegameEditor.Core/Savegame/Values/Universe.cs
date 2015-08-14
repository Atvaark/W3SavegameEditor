﻿using System.Media;
using W3SavegameEditor.Core.Savegame.Values.Engine;

namespace W3SavegameEditor.Core.Savegame.Values
{
    public class Universe
    {
        [CName("Player")]
        public Player Player { get; set; } 
    }

    public class Player
    {
        [CName("id")]
        public IdTag Id { get; set; }

        [CName("position")]
        [CType("Vector")]
        public object Position { get; set; }

        [CName("Rotation")]
        public EulerAngles Rotation { get; set; }

        [CName("template")]
        public string Template { get; set; }
    }
}