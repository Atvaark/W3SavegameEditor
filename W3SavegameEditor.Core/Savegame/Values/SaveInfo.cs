namespace W3SavegameEditor.Core.Savegame.Values
{
    [CName("saveInfo")]
    public class SaveInfo
    {
        [CName("magic_number")]
        public byte[] MagicNumber { get; set; }

        [CName("")]
        public string Description { get; set; }

        [CName("")]
        public ulong RuntimeGuidCounter { get; set; }

        [CName("count")]
        public uint Count { get; set; }
    }
}