using ControlzEx.Theming;
using CSVisor.Core.Entities;
using CSVisor.Core.Extender;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Documents;

namespace CSVisor.WPF.Dialogs.ColumnSettings
{
    public class CSVColumnSettingsDialog
    {
        private CSVColumnSettingsWindow _Window;
        private CSVColumnSettingsDialogContent _Content;
        private CSVColumnSettingsDialogViewModel _ViewModel;
        private CSVFileOptions _FileOptions;

        public CSVColumnSettingsDialog(CSVFileOptions options)
        {
            _FileOptions = options;
            _Window = new CSVColumnSettingsWindow();
            _Content = new CSVColumnSettingsDialogContent();
            _Content.OkRequested += __Content_OkRequested;
            _Content.CancelRequested += __Content_CancelRequested;
            _Window.Content = _Content;
            _ViewModel = new CSVColumnSettingsDialogViewModel(options);
            _Content.DataContext = _ViewModel;
        }

        private void __Content_CancelRequested(object? sender, EventArgs e)
        {
            _Window.Close();
        }

        private void __Content_OkRequested(object? sender, EventArgs e)
        {
            if (__CanClose())
            {
                _ViewModel.StateSortEntries.ToList().ForEach(s => _FileOptions.StateSortingColumns.Add(new Tuple<uint, eSortDirection>((uint)_ViewModel.StateSortEntries.IndexOf(s), s.SortDirection)));
                _ViewModel.GroupSortEntries.ToList().ForEach(s => _FileOptions.GroupingSortingColumns.Add(new Tuple<uint, eSortDirection>((uint)_ViewModel.GroupSortEntries.IndexOf(s), s.SortDirection)));
                _Window.Close();
            }
        }

        private bool __CanClose()
        {
            bool canClose = true;
            if (_ViewModel.SelectedPrimaryProperty.IsNullOrEmpty())
            {
                canClose = false;
                _Window.ShowMessageAsync("Cannot close", "No grouping key set");
            }

            return canClose;
        }

        public void Show()
        {
            _Window.ShowDialog();
        }

    }

    class CSVColumnSettingsWindow : MetroWindow
    {
        public CSVColumnSettingsWindow()
        {
            ThemeManager.Current.ChangeTheme(this, "Dark.Orange");
        }
    }
}
