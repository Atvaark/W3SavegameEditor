using W3SavegameEditor.Core.Savegame.Attributes;

namespace W3SavegameEditor.Core.Savegame.Values.Quest
{
    [CSerializable("CQuestExternalScenePlayer")]
    public class QuestExternalScenePlayer
    {
        [CArray("tagsCount")]
        public ExternalDialog[] ExternalDialogs { get; set; }
    }
}