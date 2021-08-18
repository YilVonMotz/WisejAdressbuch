
using System;
using System.Collections.Generic;
using Wisej.Web;

namespace WiseyAdressbuch
{
    public partial class Window1 : Form
    {
        public delegate void OnSaveClickedDel(object sender, EventArgs e);
        public OnSaveClickedDel OnSaveClicked;

        public delegate void OnCellValueChangedDel(object sender, DataGridViewCellEventArgs e);
        public OnCellValueChangedDel OnCellValueChanged;
                
        public delegate void OnTabControlClickDel(object sender, EventArgs e);
        public OnTabControlClickDel OnTabControlClick;

        public delegate void OnSearchButtonClickDel(object sender, EventArgs e);
        public OnSearchButtonClickDel OnSearchButtonClick;

        public Dictionary<string, string> searchSelection = new Dictionary<string, string>();

        public Window1
            (
            OnSaveClickedDel onSaveClickedDel
            , OnCellValueChangedDel onCellValueChanged
            , OnTabControlClickDel onTabControlClick
            ,OnSearchButtonClickDel onSearchButtonClick
            )
        {
            InitializeComponent();
            OnSaveClicked += onSaveClickedDel;
            OnCellValueChanged += onCellValueChanged;
            OnTabControlClick += onTabControlClick;
            OnSearchButtonClick += onSearchButtonClick;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnSaveClicked(sender,e);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {        
            OnCellValueChanged(sender, e);  
            
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            OnCellValueChanged(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OnSearchButtonClick(sender, e);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OnSearchButtonClick(sender, e);
            
        }

        private void textBoxFirmenname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAdresse_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTelefon_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxWebseite_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAnrede_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTitel_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxVorname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNachname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTelefonnummer_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxOrganisation_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxEMail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
