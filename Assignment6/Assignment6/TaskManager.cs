using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    /// <summary>
    ///   Class responsible for everything related
    ///   to managing a TaskManager object
    /// </summary>
    public class TaskManager
    {

        private List<Task> _tasks = new List<Task>();
        //private List<Task> _temporaryTaskList = new List<Task>();
        private SortingOptions _sortingOption;
        private SortingDirections _sortingDirection;


        // ===================== Properties ===================== //
        private List<Task> Tasks
        {
            get { return this._tasks; }
            set { this._tasks = value; }
        }

        /*private List<Task> TemporaryTaskList
        {
            get { return this._temporaryTaskList; }
            set { this._temporaryTaskList = value; }
        }*/

        public SortingOptions SortingOption
        {
            get { return this._sortingOption; }
            set { this._sortingOption = value; }
        }


        public int NumOfTasks
        {
            get { return this.Tasks.Count; }
        }



        // ======================= Methods ======================= //


        public TaskManager(SortingOptions sortingOption)
        {
            this.SortingOption = sortingOption;
        }

        public void AddTask(Task task)
        {
            this.Tasks.Add(task);
        }

        public void UpdateTask(int index, Task task)
        {
            if (this.ValidateIndex(index))
                this.Tasks[index] = task;  
        }

        public void DeleteTask(int index)
        {
            if (this.ValidateIndex(index))
                this.Tasks.RemoveAt(index);
        }

        public Task GetTask(int index)
        {
            if (this.ValidateIndex(index))
                return this.Tasks[index];
            else
                throw new ArgumentException("Index is out of range", "index");
        }

        public List<Task> GetTasks()
        {
            return this.Tasks;
        }

        private bool ValidateIndex(int index)
        {
            if (index < 0)
                throw new ArgumentException("Index cannot be smaller than 0", "index");
            if (index > this.NumOfTasks)
                throw new ArgumentException("Index argument is bigger than the highest index", "index");

            return true;
        }

        public void SortTasks(SortingOptions sortingOption)
        {
            if (!Enum.IsDefined(typeof(SortingOptions), sortingOption))
                throw new ArgumentException("sortingOption must be a SortingOptions", "sortingOption");

            if (sortingOption == SortingOptions.dateTime_ascending)
                this.Tasks = this.SortTasksByDateTime(SortingDirections.ascending);

            else if (sortingOption == SortingOptions.dateTime_descending)
                this.Tasks = this.SortTasksByDateTime(SortingDirections.descending);
            
            else if (sortingOption == SortingOptions.description_ascending)
                this.Tasks = this.SortTasksByDescription(SortingDirections.ascending);
            
            else if (sortingOption == SortingOptions.description_descending)
                this.Tasks = this.SortTasksByDescription(SortingDirections.descending);
            
            else if (sortingOption == SortingOptions.priority_ascending)
                this.Tasks = this.SortTasksByPriority(SortingDirections.ascending);
            
            else if (sortingOption == SortingOptions.priority_descending)
                this.Tasks = this.SortTasksByPriority(SortingDirections.descending);

            // Store the most recent sorting option in memory
            this.SortingOption = sortingOption;
        }

        private List<Task> SortTasksByDateTime(SortingDirections direction)
        {
            IOrderedEnumerable<Task> sortedTasks;

            if (direction == SortingDirections.ascending)
            {
                sortedTasks =
                    from task in this.Tasks
                    orderby task.DateTime ascending
                    select task;
            }
            else if (direction == SortingDirections.descending)
            {
                sortedTasks =
                    from task in this.Tasks
                    orderby task.DateTime descending
                    select task;
            }
            else
                throw new ArgumentException("direction was not valid", "direction");

            return sortedTasks.ToList<Task>();
        }

        private List<Task> SortTasksByDescription(SortingDirections direction)
        {
            IOrderedEnumerable<Task> sortedTasks;

            if (direction == SortingDirections.ascending)
            {
                sortedTasks =
                    from task in this.Tasks
                    orderby task.Description ascending
                    select task;
            }
            else if (direction == SortingDirections.descending)
            {
                sortedTasks =
                    from task in this.Tasks
                    orderby task.Description descending
                    select task;
            }
            else
                throw new ArgumentException("direction was not valid", "direction");   

            return sortedTasks.ToList<Task>();
        }

        private List<Task> SortTasksByPriority(SortingDirections direction)
        {
            IOrderedEnumerable<Task> sortedTasks;

            if (direction  == SortingDirections.ascending)
            {
                sortedTasks =
                    from task in this.Tasks
                    orderby task.PriorityLevel descending
                    select task;
            }
            else if (direction == SortingDirections.descending)
            {
                sortedTasks =
                    from task in this.Tasks
                    orderby task.PriorityLevel ascending
                    select task;
            }
            else
                throw new ArgumentException("direction was not valid", "direction");

            return sortedTasks.ToList<Task>();
        }

        /// <summary>
        ///   Returns a list with those tasks whose description contains
        ///   the search query passed into the method.
        ///   It loops through the Tasks list and returns a temporary list
        ///   with the tasks that match
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Task> FilterTasks(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
                return this.Tasks;

            // this.TemporaryTaskList.Clear();
            List<Task> filteredTasks = new List<Task>();

            foreach (Task task in this.Tasks)
            {
                if (task.Description.Contains(query))
                {
                    filteredTasks.Add(task);
                }
            }

            return filteredTasks;
        }
    }
}
