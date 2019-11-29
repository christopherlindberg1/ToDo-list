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

        private readonly TaskManager _taskManager = new TaskManager();
        private readonly List<string> _errors = new List<string>();


        // ===================== Properties ===================== //

        private TaskManager TaskManager
        {
            get { return this._taskManager; }
        }

        private List<string> Errors
        {
            get { return this._errors; }
        }




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
            this.btnSaveChanges.Enabled = false;
        }

        private void SetFormToActiveState()
        {
            this.btnChange.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnSaveChanges.Enabled = true;
        }

        private bool ValidateInput()
        {
            bool dateTimeOk = this.ValidateDateTime();
            bool toDoOk = this.ValidateToDoText();
            bool priorityOk = this.ValidatePriority();

            return dateTimeOk && toDoOk && priorityOk;
        }

        private bool ValidateDateTime()
        {
            if (this.dateTimePicker1.Value <= DateTime.Now.AddMinutes(5))
            {
                this.AddErrorMessage("The date and time must be set at least 5 minutes into the future");
                return false;
            }

            return true;
        }

        private bool ValidateToDoText()
        {
            if (String.IsNullOrWhiteSpace(this.textBoxToDo.Text.Trim()))
            {
                this.AddErrorMessage("You must provide a description of the task");
                return false;
            }

            return true;
        }

        private bool ValidatePriority()
        {
            if (this.comboBoxPriority.SelectedIndex == -1)
            {
                this.AddErrorMessage("You must select a priority");
                return false;
            }

            if (!Enum.IsDefined(typeof(PriorityLevels),
                this.comboBoxPriority.SelectedItem.ToString().Replace(" ", "_")))
            {
                this.AddErrorMessage("You must select a priority in the list");
                return false;
            }

            return true;
        }

        /// <summary>
        ///   Creates Task object from data input by user
        /// </summary>
        /// <returns>Task object</returns>
        private Task CreateTaskObject(string description, PriorityLevels priority, DateTime dateTime)
        {
            return new Task(description, priority, dateTime);
        }

        private void AddTask(Task task)
        {
            this.TaskManager.AddTask(task);
            this.AddTaskToGUI(task);
            this.ClearInputFields();
        }

        private void AddTaskToGUI(Task task)
        {
            this.listBoxToDos.Items.Add(task.ToString());
        }

        private void ClearInputFields()
        {
            this.textBoxToDo.Text = "";
            this.comboBoxPriority.SelectedIndex = 0;
        }





        // ============== Methods for error handling ============== //

        private void AddErrorMessage(string error)
        {
            if (String.IsNullOrWhiteSpace(error))
                throw new ArgumentNullException("error", "error list cannot be empty");

            this.Errors.Add(error);
        }

        private void ShowErrorMessages()
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (string error in this.Errors)
            {
                errorMessage.Append("* ");
                errorMessage.Append(error);
                errorMessage.Append("\n");
            }

            MessageBox.Show(errorMessage.ToString());

            this.ClearErrorList();
        }

        private void ClearErrorList()
        {
            this.Errors.Clear();
        }





        // ======================= Events ======================= //
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.ValidateInput())
            {
                string description = this.textBoxToDo.Text.Trim();

                PriorityLevels priority;
                Enum.TryParse(this.comboBoxPriority.SelectedItem.ToString().Replace(" ", "_"),
                    out priority);

                DateTime dateTime = this.dateTimePicker1.Value;

                Task task = new Task(description, priority, dateTime);
                this.AddTask(task);
            }
            else
            {
                this.ShowErrorMessages();
            }
        }

        private void listBoxToDos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxToDos.SelectedIndex != -1)
                this.SetFormToActiveState();
            else
                this.SetFormToDefaultState();
        }
    }
}
