﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    /// <summary>
    ///   Class responsible for everything related to 
    ///   managing a Task object.
    /// </summary>
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

        public DateTime DateTime
        {
            get { return this._dateTime; }
            set { this._dateTime = value; }
        }



        // ===================== Methods ===================== //

        public Task(string description, PriorityLevels priority, DateTime dateTime)
        {
            if (!this.ValidateTaskData(description, priority, dateTime))
                throw new InvalidOperationException("Something went wrong in ValidateTaskData");
            
            this.Description = description;
            this.PriorityLevel = priority;
            this.DateTime = dateTime;
        }

        /// <summary>
        ///   Validates arguments that will later be sent to the Task constructor
        /// </summary>
        /// <returns>True if there is no exception</returns>
        private bool ValidateTaskData(string description, PriorityLevels priority, DateTime dateTime)
        {
            if (String.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description", "description cannot be empty");
            if (!Enum.IsDefined(typeof(PriorityLevels), priority))
                throw new ArgumentException("priority must be one of those specified in the enum",
                    "priority");
            if (dateTime <= DateTime.Now)
                throw new ArgumentException("Date and time must cannot be set to the past");

            return true;
        }

        /// <summary>
        ///   returns string representation of the date for the task
        /// </summary>
        /// <returns>string with date/returns>
        private string GetTaskDateStr()
        {
            return this.DateTime.Date.ToShortDateString();
        }

        /// <summary>
        ///   returns string representation of the time for the task
        /// </summary>
        /// <returns>string with time/returns>
        private string GetTaskTimeStr()
        {
            return String.Format("{0}:{1}",
                this.DateTime.Hour,
                this.DateTime.Minute);
        }

        /// <summary>
        ///   Returns string representation of priority level for the task
        /// </summary>
        /// <returns>string with priority level</returns>
        private string GetPriorityLevelStr()
        {
            return this.PriorityLevel.ToString().Replace("_", " ");
        }

        /// <summary>
        ///   Returns string representing the Task.
        /// </summary>
        /// <returns>String representation of task.</returns>
        public override string ToString()
        {
            return String.Format("    {0, -26} {1, -20} {2, -41} {3}",
                this.GetTaskDateStr(), this.GetTaskTimeStr(),
                this.GetPriorityLevelStr(), this.Description);
        }
    }
}
