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



        // ======================= Methods ======================= //
        public void AddTask(Task task)
        {
            this.Tasks.Add(task);
        }

        public void UpdateTask(int index, Task task)
        {

        }

        public void RemoveTask(int index)
        {

        }



    }
}
