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

        public ModelCommandArgs(CommandEnum commandEnum, string message)
        {
            Command = commandEnum;
            Message = message;
        }
    }
}
