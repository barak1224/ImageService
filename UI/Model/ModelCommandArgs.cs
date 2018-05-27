using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class ModelCommandArgs : EventArgs
    {
        public string Message { get; set; }
        public CommandEnum Command { get; set; }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="commandEnum">The command enum.</param>
        /// <param name="message">The message.</param>
        public ModelCommandArgs(CommandEnum commandEnum, string message)
        {
            Command = commandEnum;
            Message = message;
        }
    }
}
