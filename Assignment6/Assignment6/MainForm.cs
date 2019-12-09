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

        private readonly TaskManager _taskManager = new TaskManager(SortingOptions.description_ascending);
        private ErrorHandler _errorHandler = new ErrorHandler();


        // ===================== Properties ===================== //

        private TaskManager TaskManager
        {
            get { return this._taskManager; }
        }

        private ErrorHandler ErrorHandler
        {
            get { return this._errorHandler; }
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
            this.listBoxToDos.Items.Clear();
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

        /// <summary>
        ///   Connects tooltips to controls in GUI
        /// </summary>
        private void SetToolTips()
        {
            this.SetDateTimePickerToolTip();
        }

        /// <summary>
        ///   Sets a tooltip for the datetime picker
        /// </summary>
        private void SetDateTimePickerToolTip()
        {
            ToolTip toolTipDateTimePicker = new ToolTip();
            toolTipDateTimePicker.SetToolTip(dateTimePicker1, "Click to open calendar for date." +
                " Write time here");
        }

        /// <summary>
        ///   Sets the controls to the default state
        /// </summary>
        private void SetFormToDefaultState()
        {
            this.ResetInputFields();

            this.btnAdd.Enabled = true;
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSaveChanges.Enabled = false;
            this.btnCancelChanges.Enabled = false;
            this.comboBoxPriority.SelectedIndex = 0;
            this.comboBoxSorting.SelectedIndex = 0;

            this.listBoxToDos.SelectedIndex = -1;
        }

        /// <summary>
        ///   Sets controls to active state (when tasks are selected).
        /// </summary>
        private void SetFormToActiveState()
        {
            this.btnAdd.Enabled = true;
            this.btnChange.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnSaveChanges.Enabled = false;
            this.btnCancelChanges.Enabled = false;
        }

        /// <summary>
        ///   Sets controls to edit state (when user is editing a task
        /// </summary>
        private void SetFormToEditState()
        {
            this.btnAdd.Enabled = false;
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSaveChanges.Enabled = true;
            this.btnCancelChanges.Enabled = true;

        }

        /// <summary>
        ///   Configures the ToolStripMenu.
        /// </summary>
        private void InitializeMenu()
        {
            // Configuring the "new" item
            this.ToolStripMenuSubitemNew.ShortcutKeys = Keys.Control | Keys.N;

            // Configuring the "exit" item
            this.ToolStripMenuSubitemExit.ShortcutKeys = Keys.Alt | Keys.F4;
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

        /// <summary>
        ///   Shows error messages in a dialog box.
        /// </summary>
        private void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Info");
        }

        /// <summary>
        ///   Validates all input required for a task
        /// </summary>
        /// <returns>boolean indicating if input was ok or not</returns>
        private bool ValidateInput()
        {
            bool dateTimeOk = this.ValidateDateTime();
            bool toDoOk = this.ValidateToDoText();
            bool priorityOk = this.ValidatePriority();

            return dateTimeOk && toDoOk && priorityOk;
        }

        /// <summary>
        ///   validates datetime agains set criterias
        /// </summary>
        /// <returns>boolean indicating if datetime was ok or not</returns>
        private bool ValidateDateTime()
        {
            if (this.dateTimePicker1.Value <= DateTime.Now.AddMinutes(5))
            {
                this.ErrorHandler.AddErrorMessage("The date and time must be set at least 5 minutes into the future");
                return false;
            }

            return true;
        }

        /// <summary>
        ///   validates description / title agains set criterias
        /// </summary>
        /// <returns>boolean indicating if description was ok or not</returns>
        private bool ValidateToDoText()
        {
            if (String.IsNullOrWhiteSpace(this.textBoxToDo.Text.Trim()))
            {
                this.ErrorHandler.AddErrorMessage("You must provide a description of the task");
                return false;
            }

            return true;
        }

        /// <summary>
        ///   validates priority agains set criterias
        /// </summary>
        /// <returns>boolean indicating if priority was ok or not</returns>
        private bool ValidatePriority()
        {
            if (this.comboBoxPriority.SelectedIndex == -1)
            {
                this.ErrorHandler.AddErrorMessage("You must select a priority");
                return false;
            }

            if (!Enum.IsDefined(typeof(PriorityLevels),
                this.comboBoxPriority.SelectedItem.ToString().Replace(" ", "_")))
            {
                this.ErrorHandler.AddErrorMessage("You must select a priority in the list");
                return false;
            }

            return true;
        }

        /// <summary>
        ///   Creates a task object with data submitted by user
        /// </summary>
        /// <returns>Task object</returns>
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

        /// <summary>
        ///   Fills input fields with data from a Task object
        /// </summary>
        private void FillFieldsWithTaskData(Task task)
        {
            this.textBoxToDo.Text = task.Description;
            this.dateTimePicker1.Value = task.DateTime;
            this.comboBoxPriority.SelectedIndex = (int) task.PriorityLevel;
        }

        /// <summary>
        ///   Adds task to storage and GUI
        /// </summary>
        private void AddTask(Task task)
        {
            this.TaskManager.AddTask(task);
            this.AddTaskToGUI(task);
            this.SortTasks(this.TaskManager.SortingOption);
            this.SetFormToDefaultState();
        }

        /// <summary>
        ///   Adds task to GUI
        /// </summary>
        private void AddTaskToGUI(Task task)
        {
            this.listBoxToDos.Items.Add(task.ToString());
        }

        /// <summary>
        ///   Updates a task by replacing it with a new Task object.
        /// </summary>
        /// <param name="task"></param>
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
            this.SortTasks(this.TaskManager.SortingOption);
            this.SetFormToDefaultState();
        }

        /// <summary>
        ///   Updates a task in the GUI by overwriting task at a certain index
        ///   with data from the Task object passed in
        /// </summary>
        private void UpdateTaskInGUI(int index, Task task)
        {
            if (index == -1)
            {
                MessageBox.Show("No task is chosen to be updated", "Info");
                return;
            }

            this.listBoxToDos.Items[index] = task.ToString();
        }

        /// <summary>
        ///   Deletes a task from storage and GUI.
        /// </summary>
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

        /// <summary>
        ///   Deletes task from GUI
        /// </summary>
        /// <param name="index"></param>
        private void DeleteTaskFromGUI(int index) 
        {
            if (index == -1)
            {
                MessageBox.Show("No task was chosen", "Info");
                return;
            }

            this.listBoxToDos.Items.RemoveAt(index);
        }

        /// <summary>
        ///   Sorts task and updates GUI accordingly
        /// </summary>
        /// <param name="sortingOption"></param>
        private void SortTasks(SortingOptions sortingOption)
        {
            if (!Enum.IsDefined(typeof(SortingOptions), sortingOption))
                throw new ArgumentException("sortingOption must be a SortingOptions", "sortingOption");
            
            this.TaskManager.SortTasks(sortingOption);
            this.UpdateListWithTasks(this.TaskManager.GetTasks());
        }

        /// <summary>
        ///   Filters (pattern matching) tasks and hides those
        ///   tasks that do not match agains the search query
        /// </summary>
        private void FilterTasks(string query)
        {
            this.listBoxToDos.Items.Clear();

            List<Task> filteredTasks = this.TaskManager.FilterTasks(query);

            foreach (Task task in filteredTasks)
            {
                this.listBoxToDos.Items.Add(task.ToString());
            }
        }

        /// <summary>
        ///   Updates the entire listbox with tasks from a 
        ///   list with tasks.
        /// </summary>
        private void UpdateListWithTasks(List<Task> tasks)
        {
            this.listBoxToDos.Items.Clear();

            foreach (Task task in tasks)
                this.listBoxToDos.Items.Add(task.ToString());
        }

        private bool VerifyAppExit()
        {
            DialogResult exit = MessageBox.Show("Sure to exit program?",
                                                "Confirm",
                                                MessageBoxButtons.OKCancel,
                                                MessageBoxIcon.Warning);

            return (exit == DialogResult.OK) ? true : false;
        }






        // ======================= Events ======================= //
        
        /// <summary>
        ///   Event for adding a task.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.ValidateInput())
            {
                Task task = this.CreateTaskObject();
                this.AddTask(task);
            }
            else
            {
                this.ShowErrorMessage(this.ErrorHandler.GetErrorMessages());
            }
        }

        /// <summary>
        ///   Event triggered when user clicks on a task.
        ///   Sets form to active state.
        /// </summary>
        private void listBoxToDos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxToDos.SelectedIndex != -1)
                this.SetFormToActiveState();
            else
                this.SetFormToDefaultState();
        }

        /// <summary>
        ///   Fills input fields with data from the selected task.
        /// </summary>
        private void btnChange_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBoxToDos.SelectedIndex;
            if (selectedIndex != -1)
            {
                this.FillFieldsWithTaskData(this.TaskManager.GetTask(selectedIndex));
                this.SetFormToEditState();
            }
        }

        /// <summary>
        ///   Deletes a task. Asks user to confirm.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBoxToDos.SelectedIndex;

            if (selectedIndex != -1)
            {
                DialogResult cancel = MessageBox.Show("Are you sure you want to delete this task?",
                                             "Confirm",
                                             MessageBoxButtons.YesNo);

                if (cancel == DialogResult.Yes)
                {
                    this.DeleteTask(selectedIndex);
                }
            }
        }

        /// <summary>
        ///   Cancels any eventual changes made to a task.
        /// </summary>
        private void btnCancelChanges_Click(object sender, EventArgs e)
        {
            this.listBoxToDos.SelectedIndex = -1;
            this.SetFormToDefaultState();
        }

        /// <summary>
        ///   Saves any changes made to a task. Validates 
        ///   input just as if the user was adding a new task.
        /// </summary>
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (this.ValidateInput())
            {
                Task task = this.CreateTaskObject();
                this.UpdateTask(task);
            }
            else
            {
                this.ShowErrorMessage(this.ErrorHandler.GetErrorMessages());
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

        /// <summary>
        ///   Event triggered at a interval specified in the timer (1 sec).
        ///   and updates the current time of day.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Updates current time label
            this.lblCurrentTime.Text = DateTime.Now.ToLongTimeString();
        }





        // ====================================
        //      Events for ToolStripMenu
        // ====================================


        /// <summary>
        ///   Resetting everything in the form. Deleting todos and reseting controls
        /// </summary>
        private void ToolStripMenuSubitemNew_Click(object sender, EventArgs e)
        {
            this.InitializeGUI();
        }

        /// <summary>
        ///   Closes the form
        /// </summary>
        private void ToolStripMenusubitemExit_Click(object sender, EventArgs e)
        {
            if (this.VerifyAppExit())
            {
                this.Close();
            }
        }

        /// <summary>
        ///   Opens info about the app when clicking on "about"
        /// </summary>
        private void ToolStripMenuSubitemAbout_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( ! this.VerifyAppExit())
            {
                e.Cancel = true;
            }
        }
    }
}
