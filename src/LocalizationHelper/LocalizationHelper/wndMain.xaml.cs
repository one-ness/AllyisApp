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
using LocalizationHelper.Classes;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data;
using System.Text.RegularExpressions;

namespace LocalizationHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        MasterData master;
        CollectionViewSource viewSource;


        public wndMain()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            master = new MasterData();
            SetBindings();
        }

        private void SetBindings()
        {
            trvMain.ItemsSource = master.FolderStructure;
        }


        private void trvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
            {
                DataTable DT = master.GlobalStringsDT(((MasterData.Folder)e.NewValue).FullPath);
                viewSource = (CollectionViewSource)this.Resources["cvsStrings"];
                viewSource.Source = DT;
                cmdSave.DataContext = DT;
                DT.RowChanged += DT_RowChanged;
            }
            
            bool isEditable = (((MasterData.Folder)e.NewValue).SubFolders.Count == 0);
            dgMain.IsEnabled = isEditable;
            dgMain.IsReadOnly = !isEditable;
            dgMain.CanUserAddRows = isEditable;
            while (grdDetail.Children.Count > 1)
                grdDetail.Children.RemoveAt(1);
            int i = 0;
            foreach (DataGridColumn col in dgMain.Columns)
            {
                //col.MaxWidth = 250;
                //col.MinWidth = 50;

                grdDetail.RowDefinitions.Add(new RowDefinition());
                TextBlock lbl = new TextBlock() { Text = col.Header.ToString(), Margin = new Thickness(5) };
                lbl.SetValue(Grid.ColumnProperty, 1);
                lbl.SetValue(Grid.RowProperty, i);
                grdDetail.Children.Add(lbl);

                TextBox txt = new TextBox();
                if (col.Header.ToString() == "Key")
                {
                    txt.PreviewTextInput += Txt_PreviewTextInput;
                    txt.TextChanged += Txt_TextChanged;
                    col.IsReadOnly = true;
                }
                txt.SetValue(Grid.ColumnProperty, 2);
                txt.HorizontalAlignment = HorizontalAlignment.Stretch;
                Binding bnd = new Binding(String.Format("Item[{0}]", col.Header.ToString()));
                txt.Margin = new Thickness(5);
                txt.SetBinding(TextBox.TextProperty, bnd);
                txt.SetValue(Grid.RowProperty, i);
                grdDetail.Children.Add(txt);
                col.Width = 200;
                i++;
            }
            //viewSource.View.Refresh();
        }

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox src = (TextBox)e.Source;
            if (src.Text.Contains(" "))
                src.Text = src.Text.Replace(" ", "");
            e.Handled = true;
        }

        private void Txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex(@"[^A-Z^a-z^0-9]");
            if (rgx.IsMatch(e.Text))
                e.Handled = true;
        }

        private void DT_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Delete)
            {
                MessageBox.Show(this, "Deleting Rows is not supported!  Please remove them directly from the .resx files");
                e.Row.RejectChanges();
                return;
            }
            if (e.Action == DataRowAction.Change &&  e.Row.Field<string>("Key", DataRowVersion.Original) != e.Row.Field<string>("Key", DataRowVersion.Current))
            {
                MessageBox.Show(this, "Modifying the key is not supported!  Please edit keys directly in the .resx files");
                e.Row.RejectChanges();
                return;
            }
            if (e.Action == DataRowAction.Commit)
            {
                master.UpdateData(e, ((MasterData.Folder)trvMain.SelectedItem).FullPath);
                viewSource.View.Refresh();
                cmdSave.IsEnabled = false;
            }
            else
            {
                cmdSave.IsEnabled = true;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            viewSource = (CollectionViewSource)this.Resources["cvsStrings"];
            ((DataTable)viewSource.Source).AcceptChanges();
        }
    }
}
