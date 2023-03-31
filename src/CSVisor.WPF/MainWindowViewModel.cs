using CSVisor.Core.BaseClasses;
using CSVisor.Core.Entities;
using CSVisor.WPF.Dialogs.ColumnSettings;
using Microsoft.Win32;
using System;
using System.Linq;

namespace CSVisor.WPF
{
    public class MainWindowViewModel : ViewModelBase
    {
        private CSVFile _CSV;

        public MainWindowViewModel()
        {
        }

        public void SelectFile()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                var file = dlg.FileName;
                _CSV = new CSVFile(file);
                if(_CSV != null && _CSV.Lines.Any()) 
                {
                    __SetUserChoosenSettings();
                }
            }
        }

        private void __SetUserChoosenSettings()
        {
            var options = new CSVFileOptions(_CSV);
            var window = new CSVColumnSettingsDialog(options);
            window.Show();
        }
    }
}
