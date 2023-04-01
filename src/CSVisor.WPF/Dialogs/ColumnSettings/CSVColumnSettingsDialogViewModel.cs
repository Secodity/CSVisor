using CSVisor.Core.BaseClasses;
using CSVisor.Core.Entities;
using CSVisor.Core.Extender;
using CSVisor.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CSVisor.WPF.Dialogs.ColumnSettings
{
    public class CSVColumnSettingsDialogViewModel : ViewModelBase
    {
        private const string FakeAssemblyName = "tmpCSVEntity";
        private const string FakeAsseblyModule = "FakeAssemblyModule";
        private const string FakeCSVLineType = "FakeCSVLineType";
        private const string FakePropertyBaseName = "DynamicProperty";
        private CSVFileOptions _Options;
        public CSVColumnSettingsDialogViewModel(CSVFileOptions options)
        {
            _Options = options;
            __Init();
        }

        private void __Init()
        {
            var sampleLines = _Options.File.Lines.Take(10);
            __DefineDynamicAssemblyEntityAndAdd(sampleLines);
        }

        public object SampleDataSource
        {
            get => Get<object>();
            set => Set(value);
        }

        public List<string> PropertyNames
        {
            get => Get<List<string>>();
            set => Set(value);
        }

        public List<string> AlreadyUsedGroupSortColumns
        {
            get => Get<List<string>>();
            set => Set(value);
        }
        public List<string> AlreadyUsedStateSortColumns
        {
            get => Get<List<string>>();
            set => Set(value);
        }

        public ObservableCollection<CSVColumnSortingViewModel> GroupSortEntries
        {
            get => Get<ObservableCollection<CSVColumnSortingViewModel>>();
            set => Set(value);
        }

        public ObservableCollection<CSVColumnSortingViewModel> StateSortEntries
        {
            get => Get<ObservableCollection<CSVColumnSortingViewModel>>();
            set => Set(value);
        }

        public string SelectedProperty
        {
            get => Get<string>();
            set
            {
                Set(value);
                _Options.UniqueIdentifyColumnType = typeof(string);
                _Options.UniqueIdentifyColumn = (uint)PropertyNames.IndexOf(value);
                __RefreshBorders();
            }
        }

        public DataGrid PreviewGrid
        {
            get => Get<DataGrid>();
            set => Set(value);
        }

        private void __RefreshBorders()
        {
            if (PreviewGrid == null)
                return;
            var notSelectedStyle = new Style(typeof(DataGridColumnHeader), new Style(typeof(DataGridColumnHeader)));
            notSelectedStyle.Setters.Add(new Setter(DataGridColumnHeader.BorderBrushProperty, Brushes.Transparent));
            notSelectedStyle.Setters.Add(new Setter(DataGridColumnHeader.BorderThicknessProperty, new Thickness(2)));
            notSelectedStyle.Setters.Add(new Setter(DataGridColumnHeader.BackgroundProperty, Brushes.Transparent));
            var column = PreviewGrid.Columns.FirstOrDefault(c => c.Header.Equals(SelectedProperty));
            if (column != null)
            {
                PreviewGrid.Columns.Except(new List<DataGridColumn> { column }).ToList().ForEach(c => c.HeaderStyle = notSelectedStyle);
                var style = new Style(typeof(DataGridColumnHeader), new Style(typeof(DataGridColumnHeader)));
                style.Setters.Add(new Setter(DataGridColumnHeader.BorderBrushProperty, Brushes.Red));
                style.Setters.Add(new Setter(DataGridColumnHeader.BorderThicknessProperty, new Thickness(2)));
                column.HeaderStyle = style;
            }
        }

        private void __DefineDynamicAssemblyEntityAndAdd(IEnumerable<IReadOnlyList<string>> sampleLines)
        {
            GroupSortEntries = new ObservableCollection<CSVColumnSortingViewModel>();
            StateSortEntries = new ObservableCollection<CSVColumnSortingViewModel>();
            AlreadyUsedGroupSortColumns = new List<string>();
            AlreadyUsedStateSortColumns = new List<string>();
            GroupSortEntries.CollectionChanged += __SortEntries_CollectionChanged;
            StateSortEntries.CollectionChanged += __SortEntries_CollectionChanged;
            var assemblyName = new AssemblyName(FakeAssemblyName);
            var ab = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var mb = ab.DefineDynamicModule(FakeAsseblyModule);

            var fakeType = mb.DefineType(FakeCSVLineType, TypeAttributes.Public);

            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var tmpPropertyNameList = new List<string>();

            // Define all the fields for the new fakeType.
            for (int i = 0; i < _Options.File.Lines.Max(l => l.Count); i++)
            {
                var propertyName = FakePropertyBaseName + i;
                tmpPropertyNameList.Add(propertyName);
                var field = fakeType.DefineField("m_" + propertyName, typeof(string), FieldAttributes.Private);

                var property = fakeType.DefineProperty(propertyName, PropertyAttributes.HasDefault, typeof(string), null);

                var dGetAccessor = fakeType.DefineMethod("get_" + propertyName, getSetAttr, typeof(string), Type.EmptyTypes);

                var numberGetIl = dGetAccessor.GetILGenerator();
                numberGetIl.Emit(OpCodes.Ldarg_0);
                numberGetIl.Emit(OpCodes.Ldfld, field);
                numberGetIl.Emit(OpCodes.Ret);

                var dSetAccessor = fakeType.DefineMethod("set_" + propertyName, getSetAttr, null, new Type[] { typeof(string) });
                var numberSetIl = dSetAccessor.GetILGenerator();
                numberSetIl.Emit(OpCodes.Ldarg_0);
                numberSetIl.Emit(OpCodes.Ldarg_1);
                numberSetIl.Emit(OpCodes.Stfld, field);
                numberSetIl.Emit(OpCodes.Ret);

                property.SetGetMethod(dGetAccessor);
                property.SetSetMethod(dSetAccessor);
            }
            PropertyNames = tmpPropertyNameList;
            __AddEmptySortingEntry(true, null, null);

            fakeType.CreateType();

            var dynamicLines = new List<dynamic>();
            foreach (var line in sampleLines)
            {
                var instance = Activator.CreateInstance(fakeType);
                var type = instance.GetType();
                for (int i = 0; i < line.Count; i++)
                {
                    var prop = type.GetProperty(FakePropertyBaseName + i);
                    if (prop != null)
                    {
                        prop.SetValue(instance, line[i]);
                    }
                }
                dynamicLines.Add(instance);
            }
            SampleDataSource = dynamicLines;
        }

        private void __AddEmptySortingEntry(bool init, CSVColumnSortingViewModel baseSender, ObservableCollection<CSVColumnSortingViewModel> newCollection)
        {
            if (init)
            {
                __InitEmptyItems();
                return;
            }
            var alreadyUsedCollection = __GetAlreadyUsedCollection(baseSender);
            if(newCollection == null)
                newCollection = __GetSortCollection(baseSender);
            var otherEntries = PropertyNames.Except(alreadyUsedCollection).ToList();
            if (!otherEntries.Any())
                return;
            __AddNewOnCollection(newCollection, otherEntries);
        }

        private void __AddNewOnCollection(ObservableCollection<CSVColumnSortingViewModel> newCollection, List<string> otherEntries)
        {
            var sortingViewModel = new CSVColumnSortingViewModel(otherEntries);
            sortingViewModel.ColumnSortingChanged += __SortingViewModel_ColumnSortingChanged;
            sortingViewModel.RemoveRequested += __SortingViewModel_RemoveRequested;
            sortingViewModel.AddNewEmptyRequired += __SortingViewModel_AddNewEmptyRequired; ;
            newCollection.Add(sortingViewModel);
        }

        private void __InitEmptyItems()
        {
            __AddNewOnCollection(StateSortEntries, PropertyNames);
            __AddNewOnCollection(GroupSortEntries, PropertyNames);
        }

        private void __SortingViewModel_AddNewEmptyRequired(object? sender, EventArgs e)
        {
            __AddEmptySortingEntry(false, sender as CSVColumnSortingViewModel, null);
        }

        private void __SortingViewModel_RemoveRequested(object? sender, CSVColumnSortingViewModel e)
        {
            var alreadyUsedCollection = __GetAlreadyUsedCollection(e);
            var sortCollection = __GetSortCollection(e);
            if (sortCollection == null || alreadyUsedCollection == null)
                return;
            alreadyUsedCollection.Remove(e.SelectedColumn);
            sortCollection.Remove(e);
            if (!sortCollection.Any())
            {
                alreadyUsedCollection.Clear();
                __AddEmptySortingEntry(false, sender as CSVColumnSortingViewModel, sortCollection);
            }
        }

        private List<string> __GetAlreadyUsedCollection(CSVColumnSortingViewModel e)
        {
            if (GroupSortEntries.Contains(e))
            {
                return AlreadyUsedGroupSortColumns;
            }
            else if (StateSortEntries.Contains(e))
            {
                return AlreadyUsedStateSortColumns;
            }
            return new List<string>();
        }

        private ObservableCollection<CSVColumnSortingViewModel> __GetSortCollection(CSVColumnSortingViewModel e)
        {
            if (GroupSortEntries.Contains(e))
            {
                return GroupSortEntries;
            }
            else if (StateSortEntries.Contains(e))
            {
                return StateSortEntries;
            }
            return new ObservableCollection<CSVColumnSortingViewModel>();
        }

        private void __SortingViewModel_ColumnSortingChanged(object? sender, string e)
        {
            var alreadyUsedCollection = __GetAlreadyUsedCollection(sender as CSVColumnSortingViewModel);
            if (alreadyUsedCollection == null)
                return;
            if (sender is CSVColumnSortingViewModel vm && vm.AlreadyAddedNew && vm.PreviousSelectedColumn.IsNotNullNorEmpty())
                alreadyUsedCollection.Remove(vm.PreviousSelectedColumn);
            alreadyUsedCollection.Add(e);
        }

        private void __SortEntries_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(GroupSortEntries));
            OnPropertyChanged(nameof(StateSortEntries));
        }
    }
}
