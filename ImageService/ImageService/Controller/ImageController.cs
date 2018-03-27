using ImageService.Commands;
using ImageService.Controller;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
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

        /**
         * The function executing the command by dechipering the command ID
         * Input: commandID, args - of the command to be execute, 
         * resultSuccesful - check if the command was execut succesfully
         * Output: string message, if command was not found - error
         */
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand command;
            if (commands.TryGetValue(commandID, out command))
            {
                Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
                {
                    bool result;
                    string msg = command.Execute(args, out result);
                    return Tuple.Create(msg,result);
                });
                task.Start();
                Tuple<string, bool> taskArgs = task.Result;
                resultSuccesful = taskArgs.Item2;
                return taskArgs.Item1;
            }
            else
            {
                resultSuccesful = false;
                return "Command was not found";
            }
        }
    }
}
