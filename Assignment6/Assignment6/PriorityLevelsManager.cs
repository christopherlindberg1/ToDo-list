using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    /// <summary>
    ///   Struct for handling the priority levels, and mainly
    ///   returning an array with the levels in a readable form.
    /// </summary>
    public struct PriorityLevelsManager
    {
        
        /// <summary>
        ///   Takes all valies in the enun PriorityLevels, stores them in
        ///   an array, and removes all underscores from the levels.
        /// </summary>
        /// <returns>string[] with priority levels</returns>
        public static string[] ParsePriorityLevels()
        {
            string[] priorityLevels = Enum.GetNames(typeof(PriorityLevels));

            for (int i = 0; i < priorityLevels.Length; i++)
                priorityLevels[i] = priorityLevels[i].Replace("_", " ");

            return priorityLevels;
        }
    }
}
