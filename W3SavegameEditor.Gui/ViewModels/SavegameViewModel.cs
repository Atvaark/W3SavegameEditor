using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using W3SavegameEditor.Gui.Model;
using W3SavegameEditor.Savegame;
using W3SavegameEditor.Savegame.Variables;

namespace W3SavegameEditor.Gui.ViewModels
{
    public class SavegameViewModel : INotifyPropertyChanged
    {
        private class OpenSavegameProgress : IProgress<Tuple<long, long>>
        {
            private readonly SavegameViewModel _viewModel;

            public OpenSavegameProgress(SavegameViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void Report(Tuple<long, long> value)
            {
                _viewModel.ProgressValue = value.Item1;
                _viewModel.ProgressMax = value.Item2;
            }
        }

        private SavegameModel _selectedSavegame;
        private long _progressMax;
        private long _progressValue;

        public ObservableCollection<SavegameModel> Savegames { get; set; }

        public SavegameModel SelectedSavegame
        {
            get
            {
                return _selectedSavegame;
            }
            set
            {
                if (_selectedSavegame != value)
                {
                    _selectedSavegame = value;
                    OnPropertyChanged();
                }
            }
        }

        public long ProgressMax
        {
            get { return _progressMax; }
            set
            {
                if (_progressMax != value)
                {
                    _progressMax = value;
                    OnPropertyChanged();
                }
            }
        }

        public long ProgressValue
        {
            get { return _progressValue; }
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand InitializeSavegames { get; private set; }

        public ICommand OpenSavegame { get; private set; }

        public SavegameViewModel()
        {
            InitializeSavegames = new DelegateCommand(ExecuteInitializeSavegameList);
            OpenSavegame = new DelegateCommand(ExecuteOpenSavegame);
            Savegames = new ObservableCollection<SavegameModel>();
            ProgressValue = 0;
            ProgressMax = 1;

            ExecuteInitializeSavegameList(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteInitializeSavegameList(object obj)
        {
            Savegames.Clear();
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string gamesavesPath = Path.Combine(userProfilePath, "Documents\\The Witcher 3\\gamesaves");
            var filesPaths = Directory.GetFiles(gamesavesPath, "*.sav");

            foreach (var filePath in filesPaths)
            {
                Savegames.Add(new SavegameModel
                {
                    Name = Path.GetFileName(filePath),
                    Path = filePath
                });
            }
        }

        private void ExecuteOpenSavegame(object parameter)
        {
            var savegame = parameter as SavegameModel;
            if (savegame == null) throw new ArgumentNullException("savegame");

            SavegameFile.ReadAsync(savegame.Path, new OpenSavegameProgress(this))
                .ContinueWith(t =>
                {
                    ProgressValue = 0;
                    ProgressMax = 1;
                    
                    var file = t.Result;
                    SelectedSavegame = new SavegameModel
                    {
                        Name = savegame.Name,
                        Path = savegame.Path,
                        Data = new SavegameDataModel
                        {
                            Version1 = file.TypeCode1,
                            Version2 = file.TypeCode2,
                            Version3 = file.TypeCode3,
                            VariableNames = new ObservableCollection<string>(file.VariableNames),
                            Variables = new ObservableCollection<Variable>(file.Variables)
                        }
                    };
                });
        }
    }
}
