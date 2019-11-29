using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment6
{
    public partial class MainForm : Form
    {


        // ======================= Methods ======================= //

        public MainForm()
        {
            InitializeComponent();
            this.InitializeGUI();
        }

        private void InitializeGUI()
        {
            this.comboBoxPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPriority.Items.AddRange(PriorityLevelsManager.ParsePriorityLevels());
            this.comboBoxPriority.SelectedIndex = 0;
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.CustomFormat = " yyyy/MM/dd - hh:mm:ss";
            this.listBoxToDos.HorizontalScrollbar = true;
            this.SetToolTips();
            
            this.SetFormToDefaultState();
        }

        private void SetToolTips()
        {
            this.SetDateTimePickerToolTip();
        }

        private void SetDateTimePickerToolTip()
        {
            ToolTip toolTipDateTimePicker = new ToolTip();
            toolTipDateTimePicker.SetToolTip(dateTimePicker1, "Click to open calendar for date." +
                " Write time here");
        }

        private void SetFormToDefaultState()
        {
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
        }

        private void SetFormToActiveState()
        {
            this.btnChange.Enabled = true;
            this.btnDelete.Enabled = true;
        }

        private bool ValidateInput()
        {
            return false;
        }

        private bool ValidateDateTime()
        {
            return false;
        }

        private bool ValidateToDoText()
        {
            return false;
        }

        private bool ValidatePriority()
        {
            return false;
        }




        // ======================= Events ======================= //

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
