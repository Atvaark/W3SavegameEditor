namespace W3SavegameEditor.Core.Savegame.Values
{
    public class AdditionalContent
    {
        [CName("count")]
        public uint Count { get; set; }

        public string[] Items { get; set; }
    }
}