using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    /// <summary>
    ///   Class responsible for handling error data
    ///   and formating it appropriately.
    /// </summary>
    public class ErrorHandler
    {
        private readonly List<string> _errors = new List<string>();
        private bool _haveShownErrors = true;



        // ===================== Properties ===================== //

        private List<string> Errors
        {
            get { return this._errors; }
        }

        private int NumOfErrors
        {
            get { return this.Errors.Count; }
        }

        private bool HaveShownErrors
        {
            get { return this._haveShownErrors; }
            set { this._haveShownErrors = value; }
        }



        // ======================= Methods ======================= //

        /// <summary>
        ///   Adds error message to the list of errors.
        ///   Checks if errors has been sent (false is default).
        ///   If they have been sent the methods clears the array and
        ///   sets HaveBeenSent to false to keep track of state.
        /// </summary>
        /// <param name="error"></param>
        public void AddErrorMessage(string error)
        {
            if (this.HaveShownErrors)
            {
                this.ClearErrorList();
                this.HaveShownErrors = false;
            }

            if (String.IsNullOrWhiteSpace(error))
                throw new ArgumentNullException("error", "error list cannot be empty");

            this.Errors.Add(error);
        }

        /// <summary>
        ///   Returns a string with all error messages.
        ///   Sets HaveShownErrors to true to keep track of state.
        /// </summary>
        public string GetErrorMessages()
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (string error in this.Errors)
            {
                errorMessage.Append("* ");
                errorMessage.Append(error);
                errorMessage.Append("\n");
            }

            this.HaveShownErrors = true;

            return errorMessage.ToString();
        }

        private void ClearErrorList()
        {
            this.Errors.Clear();
        }
    }
}
