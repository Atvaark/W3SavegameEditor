using System.Collections.ObjectModel;

namespace W3SavegameEditor.Models
{
    public class VariableModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public ObservableCollection<VariableModel> Children { get; set; }
    }
}