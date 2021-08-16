
using System;
using Wisej.Web;

namespace WiseyAdressbuch
{
    public partial class Window1 : Form
    {
        public delegate void OnSaveClickedDel(object sender, EventArgs e);
        public OnSaveClickedDel OnSaveClicked;

        public delegate void OnCellValueChangedDel(object sender, DataGridViewCellEventArgs e);
        public OnCellValueChangedDel OnCellValueChanged;


        public Window1(OnSaveClickedDel onSaveClickedDel, OnCellValueChangedDel onCellValueChanged)
        {
            InitializeComponent();
            OnSaveClicked += onSaveClickedDel;
            OnCellValueChanged += onCellValueChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnSaveClicked(sender,e);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        
           OnCellValueChanged(sender, e); ;
             
        }
    }
}
