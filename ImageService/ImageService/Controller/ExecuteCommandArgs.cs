using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ExecuteCommandArgs
    {
        public string Message { get; }
        public bool ResultSuccesful { get; } 
        public ExecuteCommandArgs(string message, bool result)
        {
            Message = message;
            ResultSuccesful = result;
        }
    }
}
