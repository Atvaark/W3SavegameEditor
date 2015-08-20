using W3SavegameEditor.Core.Savegame.Attributes;
using W3SavegameEditor.Core.Savegame.Values.Gwint;
using W3SavegameEditor.Core.Savegame.Values.Journal;

namespace W3SavegameEditor.Core.Savegame.Values
{

    [CSerializable("entry")]
    public class FactEntry
    {
        [CName("value")]
        public int Value { get; set; }
        [CName("time")]
        public double Time { get; set; }
        [CName("expiryTime")]
        public double ExpiryTime { get; set; }
    }


    [CSerializable("fact")]
    public class Fact
    {
        [CName("id")]
        public string Id { get; set; }
        [CName("expiringCount")]
        public uint ExpiringCount { get; set; }
        [CArray("entryCount")]
        public FactEntry[] Entries { get; set; }
    }

    [CSerializable("facts")]
    public class Facts
    {
        public Facts()
        {
            
        }

        [CArray]
        public Fact[] Items { get; set; }
    }

    [CSerializable("SavegameRoot")]
    public class SavegameRoot
    {
        [CName("CWitcherGameResource")]
        public CWitcherGameResource WitcherGameResource { get; set; }
        [CName("saveInfo")]
        public SaveInfo SaveInfo { get; set; }
        [CName("facts")]
        public Facts Facts { get; set; }
        public object QuestSystem { get; set; }
        public object Community { get; set; }
        public object Attitudes { get; set; }
        public object ParentGroups { get; set; }
        public object ActorAttitudes { get; set; }
        public CWitcherJournalManager JournalManager { get; set; }
        public object TelemetrySessionId { get; set; }
        public GwintManager GwintManager { get; set; }
        public object StrayActiors { get; set; }
        public object SoundClues { get; set; }
        public object ScentClues { get; set; }
        public object FocusModeVisibility { get; set; }
        public object CCommonMapManager { get; set; }
        public object LootManager { get; set; }
        public object TutorialSystem { get; set; }
        public object SoundSystem { get; set; }
        public object NewGamePlus { get; set; }
        public object WorldInfo { get; set; }
        public object AdditionalSaveInfo { get; set; }
        public object AdditionalContent { get; set; }
        public object ContainerManagerSaveInfo { get; set; }
        public object TimeScale { get; set; }
        public object TickManager { get; set; }
        public object Universe { get; set; }
        public object WorldState { get; set; }
        public object GameProp { get; set; }
        public object TimeManager { get; set; }
        public object IdTagManger { get; set; }
        public object EnvManger { get; set; }
        public object StringTable { get; set; }
    }
}
