using ControlzEx.Theming;
using CSVisor.Core.Entities;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSVisor.WPF.Dialogs.ColumnSettings
{
    public class CSVColumnSettingsDialog
    {
        private CSVColumnSettingsWindow _Window;
        private CSVColumnSettingsDialogContent _Content;

        public CSVColumnSettingsDialog(CSVFileOptions options)
        {
            _Window = new CSVColumnSettingsWindow();
            _Content = new CSVColumnSettingsDialogContent();
            _Window.Content = _Content;
            _Content.DataContext = new CSVColumnSettingsDialogViewModel(options);
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
