using W3SavegameEditor.Core.Savegame.Attributes;

namespace W3SavegameEditor.Core.Savegame.Values.Quest
{
    [CSerializable("questSystem")]
    public class QuestSystem
    {
        [CName("questExternalScenePlayers")]
        public QuestExternalScenePlayer QuestExternalScenePlayer { get; set; }

    }
}