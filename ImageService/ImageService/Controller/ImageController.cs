using Service.Commands;
using Service.Infrastructure;
using Service.Infrastructure.Enums;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModel m_Model;                      // The Model Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModel Model)
        {
            m_Model = Model;                    // Storing the Model Of The System
            commands = new Dictionary<int, ICommand>()
            {
                { (int)CommandEnum.NewFileCommand, new NewFileCommand(Model) }
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand command;
            if (commands.TryGetValue(commandID, out command))
            {
                return command.Execute(args, out resultSuccesful);
            }
            else
            {
                resultSuccesful = false;
                return "Command was not found";
            }
        }
    }
}
