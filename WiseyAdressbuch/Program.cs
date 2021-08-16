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

        private static SQLiteCommand selectFromMitarbeiter;
        private static SQLiteCommand selectFromOrganisation;
        private static SQLiteCommand insertIntoMitarbeiter;
        private static SQLiteCommand insertIntoOrganisation;
        private static SQLiteCommand modifyMitarbeiter;
        private static SQLiteCommand modifyOrganisation;
        private static SQLiteCommand deleteMitarbeiter;
        private static SQLiteCommand deleteOrganisation;

        private static SQLiteDataAdapter dataAdapterMitarbeiter;
        private static SQLiteDataAdapter dataAdapterOrganisation;

        private static DataTable dataTableMitarbeiter;
        private static DataTable dataTableOrganisation;

        private static SQLiteTransaction transaction;
        private static Window1 windowHandle;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            Application.Desktop = new MyDesktop();

            Window1 window = new Window1(OnSaveClicked, OnCellValueChanged);
            windowHandle = window;
            window.Show();

            connection = new SQLiteConnection(connectionString);

            //dataAdapter Mitarbeiter 
            dataAdapterMitarbeiter = new SQLiteDataAdapter(selectFromMitarbeiter);
            dataAdapterMitarbeiter.AcceptChangesDuringUpdate = true;
            dataAdapterMitarbeiter.RowUpdated += DataAdapter_RowUpdated;
            //--

            //dataAdapter Organisation
            dataAdapterOrganisation = new SQLiteDataAdapter();
            //--

            //selectMitarbeiter command
            selectFromMitarbeiter = new SQLiteCommand();
            selectFromMitarbeiter.Connection = connection;
            selectFromMitarbeiter.CommandText = "SELECT * FROM Mitarbeiter";
            //--

            //selectOrganisation command
            selectFromOrganisation = new SQLiteCommand();
            selectFromOrganisation.Connection = connection;
            selectFromOrganisation.CommandText = "SELECT * FROM Organisation";
            //--

            dataTableMitarbeiter = new DataTable();
            
            comBuilder = new SQLiteCommandBuilder(dataAdapterMitarbeiter);
            connection.Open();
            transaction = connection.BeginTransaction();
            dataAdapterMitarbeiter.Fill(dataTableMitarbeiter);
            window.dataGridView.Fill(dataTableMitarbeiter.DefaultView);
            

        }

        private static void DataAdapter_RowUpdated(object sender, System.Data.Common.RowUpdatedEventArgs e)
        {
            dataAdapterMitarbeiter.Fill(dataTableMitarbeiter);
            //windowHandle.dataGridView.Fill(dataTable.DefaultView);
        }

        

        public static void OnSaveClicked(object sender, EventArgs e)
        {
            dataAdapterMitarbeiter.Update(dataTableMitarbeiter);
            transaction.Commit();

        }


       

        public static void OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == windowHandle.dataGridView.RowCount-1)
            {
                MessageBox.Show("Ignore");
            }
            else
            {
                object cellValue = ((DataGridView)sender).CurrentCell.Value;
                dataTableMitarbeiter.Rows[e.RowIndex][e.ColumnIndex] = cellValue;

                dataAdapterMitarbeiter.UpdateCommand = CreateInsertCommand("Mitarbeiter", e.ColumnIndex, cellValue);


            }



        }


        private static SQLiteCommand CreateInsertCommand(string tableName, int columnIndex, object value )
        {
            SQLiteCommand insertCommand = new SQLiteCommand();
            insertCommand.Connection = connection;
            insertCommand.CommandText = "Insert Into " + tableName + " ('"+dataTableMitarbeiter.Columns[columnIndex]+ "') VALUES ('" + value + "')";
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