using CSVisor.Core.BaseClasses;
using CSVisor.Core.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CSVisor.WPF.ViewModels
{
    public class CSVColumnSortingViewModel : ViewModelBase
    {
        public bool AlreadyAddedNew { get; private set; }
        public event EventHandler<string> ColumnSortingChanged;
        public event EventHandler<CSVColumnSortingViewModel> RemoveRequested;
        public event EventHandler AddNewEmptyRequired;
        public CSVColumnSortingViewModel(List<string> columns)
        {
            SortingEntity = new CSVColumnSorting();
            Columns = columns;
            SwitchSortDirection();
        }

        public CSVColumnSorting SortingEntity
        {
            get => Get<CSVColumnSorting>();
            set => Set(value);
        }

        public string SelectedColumn
        {
            get => SortingEntity.SelectedColumn;
            set
            {
                PreviousSelectedColumn = SortingEntity.SelectedColumn;
                SortingEntity.SelectedColumn = value;
                OnPropertyChanged(nameof(SelectedColumn));
                ColumnSortingChanged?.Invoke(this, SelectedColumn);
                if (!AlreadyAddedNew)
                {
                    AddNewEmptyRequired?.Invoke(this, EventArgs.Empty);
                    AlreadyAddedNew = true;
                }
            }
        }

        public string PreviousSelectedColumn
        {
            get => Get<string>();
            set => Set(value);
        }

        public eSortDirection SortDirection
        {
            get => Get<eSortDirection>();
            set => Set(value);
        }

        public List<string> Columns
        {
            get => Get<List<string>>();
            set => Set(value);
        }

        public Visibility IsDescending => SortDirection == eSortDirection.Descending ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsNotDescending => SortDirection == eSortDirection.Descending ? Visibility.Collapsed : Visibility.Visible;

        public void Remove()
        {
            RemoveRequested?.Invoke(this, this);
        }

        public void SwitchSortDirection()
        {
            if (SortDirection == eSortDirection.Ascending)
                SortDirection = eSortDirection.Descending;
            else
                SortDirection = eSortDirection.Ascending;
            OnPropertyChanged(nameof(IsDescending));
            OnPropertyChanged(nameof(IsNotDescending));
        }


    }
}
