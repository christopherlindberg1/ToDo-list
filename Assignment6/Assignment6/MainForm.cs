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

        private readonly TaskManager _taskManager = new TaskManager(SortingOptions.dateTime_ascending);
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
            this.InitializeMenu();
            this.InitializeTimer();
        }

        private void InitializeGUI()
        {
            this.comboBoxPriority.Items.Clear();
            this.comboBoxPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPriority.Items.AddRange(PriorityLevelsManager.ParsePriorityLevels());
            this.comboBoxPriority.SelectedIndex = 0;
            this.comboBoxSorting.Items.Clear();
            this.comboBoxSorting.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSorting.Items.AddRange(Enum.GetNames(typeof(SortingOptions)));
            this.comboBoxSorting.SelectedIndex = 0;
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            this.dateTimePicker1.CustomFormat = " yyyy/MM/dd - HH:mm";
            this.listBoxToDos.HorizontalScrollbar = true;
            this.lblCurrentTime.Text = DateTime.Now.ToLongTimeString();

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
            this.ResetInputFields();

            this.btnAdd.Enabled = true;
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSaveChanges.Enabled = false;
            this.btnCancelChanges.Enabled = false;

            this.listBoxToDos.SelectedIndex = -1;
        }

        private void SetFormToActiveState()
        {
            this.btnAdd.Enabled = true;
            this.btnChange.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnSaveChanges.Enabled = false;
            this.btnCancelChanges.Enabled = false;
        }

        private void SetFormToEditState()
        {
            this.btnAdd.Enabled = false;
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSaveChanges.Enabled = true;
            this.btnCancelChanges.Enabled = true;

        }

        private void InitializeMenu()
        {
            
        }

        /// <summary>
        ///   Initializes a timer and connects it to an event handler
        /// </summary>
        private void InitializeTimer()
        {
            this.timer1.Interval = 1000;
            this.timer1.Tick += new EventHandler(timer1_Tick);

            this.timer1.Enabled = true;
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

        private Task CreateTaskObject()
        {
            if (this.comboBoxPriority.SelectedIndex == -1)
                throw new InvalidOperationException("No priprity was chosen");

            string description = this.textBoxToDo.Text.Trim();

            PriorityLevels priority;
            Enum.TryParse(this.comboBoxPriority.SelectedItem.ToString().Replace(" ", "_"),
                out priority);

            DateTime dateTime = this.dateTimePicker1.Value;

            return new Task(description, priority, dateTime);
        }

        private void ResetInputFields()
        {
            this.textBoxToDo.Text = "";
            this.comboBoxPriority.SelectedIndex = 0;
            this.dateTimePicker1.Value = DateTime.Now;
        }

        private void FillFieldsWithTaskData(Task task)
        {
            this.textBoxToDo.Text = task.Description;
            this.dateTimePicker1.Value = task.DateTime;
            this.comboBoxPriority.SelectedIndex = (int) task.PriorityLevel;
        }

        private void AddTask(Task task)
        {
            this.TaskManager.AddTask(task);
            this.AddTaskToGUI(task);
            this.SetFormToDefaultState();
        }

        private void AddTaskToGUI(Task task)
        {
            this.listBoxToDos.Items.Add(task.ToString());
        }

        private void UpdateTask(Task task)
        {
            int selectedIndex = this.listBoxToDos.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("No task is chosen to be updated", "Info");
                return;
            }

            Task newTask = this.CreateTaskObject();

            this.TaskManager.UpdateTask(selectedIndex, newTask);
            this.UpdateTaskInGUI(selectedIndex, newTask);
            this.SetFormToDefaultState();
        }

        private void UpdateTaskInGUI(int index, Task task)
        {
            if (index == -1)
            {
                MessageBox.Show("No task is chosen to be updated", "Info");
                return;
            }

            this.listBoxToDos.Items[index] = task.ToString();
        }

        private void DeleteTask(int index)
        {
            if (index == -1)
            {
                MessageBox.Show("No task was chosen", "Info");
                return;
            }

            this.TaskManager.DeleteTask(index);
            this.DeleteTaskFromGUI(index);
        }

        private void DeleteTaskFromGUI(int index) 
        {
            if (index == -1)
            {
                MessageBox.Show("No task was chosen", "Info");
                return;
            }

            this.listBoxToDos.Items.RemoveAt(index);
        }

        private void SortTasks(SortingOptions sortingOption)
        {
            if (!Enum.IsDefined(typeof(SortingOptions), sortingOption))
                throw new ArgumentException("sortingOption must be a SortingOptions", "sortingOption");
            
            this.TaskManager.SortTasks(sortingOption);
            this.UpdateListWithTasks(this.TaskManager.GetTasks());
        }

        private void FilterTasks(string query)
        {
            this.listBoxToDos.Items.Clear();

            List<Task> filteredTasks = this.TaskManager.FilterTasks(query);

            foreach (Task task in filteredTasks)
            {
                this.listBoxToDos.Items.Add(task.ToString());
            }
        }

        private void UpdateListWithTasks(List<Task> tasks)
        {
            this.listBoxToDos.Items.Clear();

            foreach (Task task in tasks)
                this.listBoxToDos.Items.Add(task.ToString());
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

            MessageBox.Show(errorMessage.ToString(), "Info");

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
                Task task = this.CreateTaskObject();
                this.AddTask(task);
                this.SortTasks(this.TaskManager.SortingOption);
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

        private void btnChange_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBoxToDos.SelectedIndex;
            if (selectedIndex != -1)
            {
                this.FillFieldsWithTaskData(this.TaskManager.GetTask(selectedIndex));
                this.SetFormToEditState();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBoxToDos.SelectedIndex;

            if (selectedIndex != -1)
            {
                var cancel = MessageBox.Show("Are you sure you want to delete this task?",
                                             "Confirm",
                                             MessageBoxButtons.YesNo);

                if (cancel == DialogResult.Yes)
                {
                    this.DeleteTask(selectedIndex);
                }
            }
        }

        private void btnCancelChanges_Click(object sender, EventArgs e)
        {
            this.listBoxToDos.SelectedIndex = -1;
            this.SetFormToDefaultState();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (this.ValidateInput())
            {
                Task task = this.CreateTaskObject();
                this.UpdateTask(task);
                this.SortTasks(this.TaskManager.SortingOption);
            }
            else
            {
                this.ShowErrorMessages();
            }
        }

        /// <summary>
        ///   Event for sorting tasks
        /// </summary>
        private void comboBoxSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxToDos.Items.Count > 1)
            {
                if (this.comboBoxSorting.SelectedIndex != -1)
                {

                    SortingOptions sortingOption;
                    var result = Enum.TryParse(this.comboBoxSorting.SelectedItem.ToString(),
                        out sortingOption);

                    if (result)
                    {
                        this.SortTasks(sortingOption);
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Updates current time label
            this.lblCurrentTime.Text = DateTime.Now.ToLongTimeString();
        }

        /// <summary>
        ///   Event for searching tasks based on their description.
        ///   Event is triggered when user types in search field
        ///   and lets key up.
        /// </summary>
        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            string query = this.textBoxSearch.Text;

            this.FilterTasks(query);
        }
    }
}
