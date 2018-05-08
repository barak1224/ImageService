using ImageService.Model;
using System;
using ImageService.Infrastructure.Enums;
using ImageService.Server;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        public event EventHandler<CommandRecievedEventArgs> SendCommand;

        /// <summary>
        /// The function passing the information for the directory that needs to be close
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            SendCommand?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, args[0]));
            result = true;
            return String.Format("CloseCommand for {0} was finished", args[0]);
        }
    }
}