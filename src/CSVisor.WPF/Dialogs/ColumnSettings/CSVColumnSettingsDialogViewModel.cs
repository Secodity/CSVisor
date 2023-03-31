using CSVisor.Core.BaseClasses;
using CSVisor.Core.Entities;
using System;
using System.Collections.Generic;
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

    }
}
