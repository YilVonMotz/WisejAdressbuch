using System;
using Wisej.Web;
using System.Data.SQLite;
using System.Data;
using System.Collections.Generic;
using System.Text;

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

        private static string[] mitarbeiterColumnNames = new string[8];
        private static string[] organisationColumnNames = new string[4];

        


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {            
            Application.Desktop = new MyDesktop();

            Window1 window = new Window1
                (
                OnSaveClicked
                , OnCellValueChanged
                , OnTabControlClicked
                , OnSearchButtonClick
                );

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
            FillCurrentDataGrid(BuildSelectCommand());
            InitializeSearchUI();
            
            

        }


        private static int GetDBEntriesCount(string tableName, string columnName, string searchPhrase)
        {
            SQLiteDataReader reader;
            SQLiteCommand com = null;
            try
            {
                com = connection.CreateCommand();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }

            com.CommandText = "select * from " + tableName + " where " + columnName + " like '" + searchPhrase + "'";


            int stepCount = 0;

            try
            {
                reader = com.ExecuteReader();
                while (reader.Read()) ;
                stepCount = reader.StepCount;
                reader.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return stepCount;
        }



        private static SQLiteCommand BuildSelectCommand()
        {
            selectCommand = new SQLiteCommand();
            selectCommand.Connection = connection;
            selectCommand.CommandText = "SELECT * FROM " + windowHandle.TabControl1.GetControl(windowHandle.TabControl1.SelectedIndex).Text;
            return selectCommand;
        }



        private static SQLiteCommand BuildSelectCommand(string[] columns, object[] values)
        {            
            selectCommand = new SQLiteCommand();
            selectCommand.Connection = connection;
            string selectedTable = windowHandle.TabControl1.GetControl(windowHandle.TabControl1.SelectedIndex).Text;            

            StringBuilder sb = new StringBuilder("select * from " + selectedTable);  

            for (int i = 0; i < columns.Length; i++)
            {                

                if (i == 0)
                {
                    sb.Append(" where ");
                }

                if((string)values[i] != string.Empty)
                {
                    if (i > 1)
                    {
                        sb.Append(" AND ");
                    }


                    if (GetDBEntriesCount(selectedTable,columns[i] ,(string)values[i]) == 0)
                    {
                        sb.Append(columns[i] + " like '" + values[i] + "%' ");

                    }
                    else
                    {
                        sb.Append(columns[i] + " like '" + values[i] + "' ");

                    }
                }              


            }

            selectCommand.CommandText = sb.ToString();
            return selectCommand;
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
            FillCurrentDataGrid(BuildSelectCommand());
        }

        private static void FillCurrentDataGrid(SQLiteCommand _selectCommand)
        {
            BuildSelectCommand();
            dataTable = new DataTable();
            dataAdapter.SelectCommand = _selectCommand;
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
            FillCurrentDataGrid(BuildSelectCommand());
        }


        public static void OnSearchButtonClick(object sender, EventArgs e)
        {
            string tableName = windowHandle.TabControl1.GetControl(windowHandle.TabControl1.SelectedIndex).Text;
            FillCurrentDataGrid(BuildSelectCommand());
        }

      



        private static SQLiteCommand CreateInsertCommand(string tableName, int columnIndex, object value )
        {
            SQLiteCommand insertCommand = new SQLiteCommand();
            insertCommand.Connection = connection;
            insertCommand.CommandText = "Insert Into " + tableName + " ('"+dataTable.Columns[columnIndex]+ "') VALUES ('" + value + "')";
            return insertCommand;
        }

       
        private static void InitializeSearchUI()
        {
            DataTable mitarbeiterTable = new DataTable();
            DataTable organisationTable = new DataTable();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();

            windowHandle.InitSearchControls();

            //tab1 Mitarbeiter
            dataAdapter.SelectCommand = new SQLiteCommand("SELECT * FROM Mitarbeiter LIMIT 1",connection);
            dataAdapter.Fill(mitarbeiterTable);

            for(int i = 0; i < Program.mitarbeiterColumnNames.Length; i++)
            {
                mitarbeiterColumnNames[i] = mitarbeiterTable.Columns[i].ColumnName;
            }


            


            //tab2 Organisation
            dataAdapter.SelectCommand = new SQLiteCommand("SELECT * FROM Organisation LIMIT 1", connection);
            dataAdapter.Fill(organisationTable);

            for(int i = 0; i < Program.organisationColumnNames.Length; i++)
            {
                organisationColumnNames[i] = organisationTable.Columns[i].ColumnName;
            }


           

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