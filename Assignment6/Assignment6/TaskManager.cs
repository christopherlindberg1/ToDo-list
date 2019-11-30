using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    public class TaskManager
    {

        private readonly List<Task> _tasks = new List<Task>();


        // ===================== Properties ===================== //
        private List<Task> Tasks
        {
            get { return this._tasks; }
        }

        public int NumOfTasks
        {
            get { return this.Tasks.Count; }
        }



        // ======================= Methods ======================= //
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

        private bool ValidateIndex(int index)
        {
            if (index < 0)
                throw new ArgumentException("Index cannot be smaller than 0", "index");
            if (index > this.NumOfTasks)
                throw new ArgumentException("Index argument is bigger than the highest index", "index");

            return true;
        }



    }
}
