using System;
using Wisej.Web;
using System.Data.SQLite;
using System.Data;
using System.Collections.Generic;

namespace WiseyAdressbuch
{
    static class Program
    {

        private static string connectionString = @"dataSource = C:\Users\Yil\source\repos\WiseyAdressbuch\WiseyAdressbuch\Adressbuch.db";
        private static SQLiteConnection connection;

        private static SQLiteCommand selectCommand;
        private static SQLiteCommand insertIntoMitarbeiter;
        private static SQLiteCommand insertIntoOrganisation;
        private static SQLiteCommand modifyMitarbeiter;
        private static SQLiteCommand modifyOrganisation;
        private static SQLiteCommand deleteMitarbeiter;
        private static SQLiteCommand deleteOrganisation;

        private static SQLiteDataAdapter dataAdapter;        

        private static DataTable dataTable;       

        private static SQLiteTransaction transaction;
        private static Window1 windowHandle;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            Application.Desktop = new MyDesktop();

            Window1 window = new Window1(OnSaveClicked, OnCellValueChanged, OnTabControlClicked);
            windowHandle = window;
            window.Show();

            windowHandle.TabPage1.Text = "Mitarbeiter";
            windowHandle.TabPage2.Text = "Organisation";

            connection = new SQLiteConnection(connectionString);

            BuildSelectCommand();

            //dataAdapter
            dataAdapter = new SQLiteDataAdapter(selectCommand);
            dataAdapter.AcceptChangesDuringUpdate = true;
            dataAdapter.RowUpdated += DataAdapter_RowUpdated;
            //--

            windowHandle.TabControl1.Selected += TabControl1_Selected;                       
            
            connection.Open();
            transaction = connection.BeginTransaction();
            FillCurrentDataGrid();
            

        }


        private static void BuildSelectCommand()
        {
            //select command
            selectCommand = new SQLiteCommand();
            selectCommand.Connection = connection;
            selectCommand.CommandText = "SELECT * FROM " + windowHandle.TabControl1.GetControl(windowHandle.TabControl1.SelectedIndex).Text;
            //--
        }


        private static void BuildInsertCommand()
        {

        }


        private static void BuildDeleteCommand()
        {

        }


        private static void BuildUpdateCommand()
        {

        }


        private static void TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            FillCurrentDataGrid();
        }

        private static void FillCurrentDataGrid()
        {
            BuildSelectCommand();
            dataTable = new DataTable();
            dataAdapter.SelectCommand = selectCommand;
            dataAdapter.Fill(dataTable);
            Control.ControlCollection controls = windowHandle.TabControl1.GetControl(windowHandle.TabControl1.SelectedIndex).Controls;

            foreach (Control item in controls)
            {
                if (item.GetType() == typeof(DataGridView))
                {
                    ((DataGridView)item).Fill(dataTable.DefaultView);
                }
            }
        }


        private static void DataAdapter_RowUpdated(object sender, System.Data.Common.RowUpdatedEventArgs e)
        {
            dataAdapter.Fill(dataTable);
            //windowHandle.dataGridView.Fill(dataTable.DefaultView);
        }
        


        public static void OnSaveClicked(object sender, EventArgs e)
        {
            dataAdapter.Update(dataTable);
            transaction.Commit();

        }
       

        public static void OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView currentDataGridView = ((DataGridView)sender);
            if (e.RowIndex == currentDataGridView.RowCount-1)
            {
                MessageBox.Show("Ignore");
            }
            else
            {
                object cellValue = currentDataGridView.CurrentCell.Value;
                dataTable.Rows[e.RowIndex][e.ColumnIndex] = cellValue;

                dataAdapter.UpdateCommand = CreateInsertCommand(windowHandle.TabControl1.GetControl(windowHandle.TabControl1.SelectedIndex).Text, e.ColumnIndex, cellValue);

            }

        }



        public static void OnTabControlClicked(object sender , EventArgs e)
        {
            FillCurrentDataGrid();
        }




        private static SQLiteCommand CreateInsertCommand(string tableName, int columnIndex, object value )
        {
            SQLiteCommand insertCommand = new SQLiteCommand();
            insertCommand.Connection = connection;
            insertCommand.CommandText = "Insert Into " + tableName + " ('"+dataTable.Columns[columnIndex]+ "') VALUES ('" + value + "')";
            return insertCommand;
        }

        //
        // You can use the entry method below
        // to receive the parameters from the URL in the args collection.
        //
        //static void Main(NameValueCollection args)
        //{
        //}
    }
}