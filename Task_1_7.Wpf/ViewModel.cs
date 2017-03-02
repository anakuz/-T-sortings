using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Task_1_7.Wpf.Annotations;

namespace Task_1_7.Wpf
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly SortingEngine _engine = new SortingEngine();

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

        public ICommand GenerateItems { get; private set; }

        public ICommand Sort { get; private set; }

        public IEnumerable TypesOfElems
        {
            get { return SortingEngine.TypesOfElems; }
        }

        public IEnumerable SortTypes
        {
            get { return SortingEngine.SortTypes; }
        }

        public IEnumerable MethodTypes
        {
            get { return SortingEngine.MethodTypes; }
        }

        

        public ElemsType SelectedTypeOfElems
        {
            get { return _engine.TypeOfElems; }
            set { _engine.TypeOfElems = value; }
        }

        public SortType SelectedSortType
        {
            get { return _engine.SortType; }
            set { _engine.SortType = value; }
        }

        public MethodType SelectedMethodType
        {
            get { return _engine.MethodType; }
            set { _engine.MethodType = value; }
        }

        public string OriginalItemsText
        {
            get { return _engine.OriginalItemsText; }
            set { _engine.OriginalItemsText = value; }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}