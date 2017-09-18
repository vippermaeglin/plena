using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace M4.WebControlSample.Model
{
    class DownloadCollection : ObservableCollection<Download>
    {
        private NotifyCollectionChangedEventArgs notify;

        public DownloadCollection()
        {
            notify = new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset );
        }

        protected override void InsertItem( int index, Download item )
        {
            base.InsertItem( index, item );
            item.PropertyChanged += OnItemPropertyChanged;
        }

        protected override void RemoveItem( int index )
        {
            if ( this.Count > index )
            {
                Download item = this[ index ];
                item.PropertyChanged -= OnItemPropertyChanged;
            }

            base.RemoveItem( index );
        }

        protected override void SetItem( int index, Download item )
        {
            throw new NotSupportedException();
        }

        protected override void ClearItems()
        {
            foreach ( Download item in this )
                item.PropertyChanged -= OnItemPropertyChanged;

            base.ClearItems();
        }

        private void OnItemPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            this.OnCollectionChanged( notify );
        }
    }
}
