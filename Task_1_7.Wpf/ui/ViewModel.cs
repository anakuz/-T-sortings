#region Imports (6)

using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Task_1_7.Wpf.Annotations;

#endregion Imports (6)

namespace Task_1_7.Wpf
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly SortingEngine _engine = new SortingEngine();

        public ICommand GenerateItems { get; private set; }

        public IEnumerable MethodTypes
        {
            get { return SortingEngine.MethodTypes; }
        }

        public string OriginalItemsText
        {
            get { return _engine.OriginalItemsText; }
            set { _engine.OriginalItemsText = value; }
        }

        public MethodType SelectedMethodType
        {
            get { return _engine.MethodType; }
            set { _engine.MethodType = value; }
        }

        public SortType SelectedSortType
        {
            get { return _engine.SortType; }
            set { _engine.SortType = value; }
        }

        public ElemsType SelectedTypeOfElems
        {
            get { return _engine.TypeOfElems; }
            set { _engine.TypeOfElems = value; }
        }

        public ICommand Sort { get; private set; }

        public string SortedItemsText
        {
            get
            {
                try
                {
                    return _engine.SortedItemsText;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }
        }

        public IEnumerable SortTypes
        {
            get { return SortingEngine.SortTypes; }
        }

        public IEnumerable TypesOfElems
        {
            get { return SortingEngine.TypesOfElems; }
        }

        public ViewModel()
        {
            GenerateItems = new Command(() => { _engine.GenerateItems(); });
            Sort = new Command(() => _engine.Sort());
            _engine.Sorted += _engine_Sorted;
            _engine.ItemsGenerated += _engine_ItemsGenerated;
        }

        private void _engine_ItemsGenerated()
        {
            OnPropertyChanged("OriginalItemsText");
        }

        private void _engine_Sorted()
        {
            OnPropertyChanged("SortedItemsText");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
