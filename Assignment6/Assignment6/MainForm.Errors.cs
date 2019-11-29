using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    public partial class MainForm
    {
        private readonly List<string> _errors = new List<string>();

        // ===================== Properties ===================== //

        private List<string> Errors
        {
            get { return this._errors; }
        }



        // ======================= Methods ======================= //

        private void AddErrorMessage(string error)
        {
            if (String.IsNullOrWhiteSpace(error))
                throw new ArgumentNullException("error", "error cannot be empty");

            this.Errors.Add(error);
        }

        private string ShowErrorMessages()
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (string error in this.Errors)
            {
                errorMessage.Append("* ");
                errorMessage.Append(error);
                errorMessage.Append("\n");
            }

            return errorMessage.ToString();
        }
    }

}
