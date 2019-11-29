using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    public class Task
    {
        
        private string _description;
        private PriorityLevels _priorityLevel;
        private DateTime _dateTime;


        // ===================== Properties ===================== //

        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        public PriorityLevels PriorityLevel
        {
            get { return this._priorityLevel; }
            set { this._priorityLevel = value; }
        }

        private DateTime DateTime
        {
            get { return this._dateTime; }
            set { this._dateTime = value; }
        }



        // =================== Methods =================== //


        private string GetTaskDateStr()
        {
            return this.DateTime.Date.ToShortDateString();
        }

        private string GetTaskTimeStr()
        {
            return String.Format("{0}:{1}:{2}",
                this.DateTime.Hour,
                this.DateTime.Minute,
                this.DateTime.Second);
        }

        private string GetPriorityLevelStr()
        {
            return this.PriorityLevel.ToString();
        }

        public override string ToString()
        {
            return String.Format("{0, -10} {1, -20} {2, -10} {3}",
                this.GetTaskDateStr(), this.GetTaskTimeStr(),
                this.GetPriorityLevelStr(), this.Description);
        }
    }
}
