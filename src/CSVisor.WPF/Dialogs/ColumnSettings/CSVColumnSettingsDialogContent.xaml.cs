using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSVisor.WPF.Dialogs.ColumnSettings
{
    /// <summary>
    /// Interaction logic for CSVColumnSettingsDialogContent.xaml
    /// </summary>
    public partial class CSVColumnSettingsDialogContent
    {
        public CSVColumnSettingsDialogContent()
        {
            InitializeComponent();
            DataContextChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
            {
                if (DataContext is CSVColumnSettingsDialogViewModel vm)
                    vm.PreviewGrid = previewDataGrid;
            };
            ThemeManager.Current.ChangeTheme(this, "Dark.Orange");
        }

        private void __CSVColumnSettingsDialogContent_DataContextChanged()
        {
            throw new NotImplementedException();
        }
    }
}
