using System.Collections.ObjectModel;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Model
{
    public class SavegameDataModel
    {
        public int Version1 { get; set; }
        public int Version2 { get; set; }
        public int Version3 { get; set; }

        public ObservableCollection<string> VariableNames { get; set; }

        public ObservableCollection<VariableModel> Variables { get; set; }
    }
}