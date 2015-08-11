using System.Collections.ObjectModel;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Gui.Model
{
    public class SavegameDataModel
    {
        public int Version1 { get; set; }
        public int Version2 { get; set; }
        public int Version3 { get; set; }

        public ObservableCollection<string> VariableNames { get; set; }

        public ObservableCollection<Variable> Variables { get; set; }
    }
}