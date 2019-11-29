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
        public MainForm()
        {
            InitializeComponent();
            this.InitializeGUI();
        }

        private void InitializeGUI()
        {
            this.comboBoxPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            this.SetFormToDefaultState();
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
    }
}
