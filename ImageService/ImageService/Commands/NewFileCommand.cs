using ImageService.Infrastructure;
using ImageService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel m_Model;

        public NewFileCommand(IImageServiceModel Model)
        {
            m_Model = Model;            // Storing the Model
        }

        /// <summary>
        /// The function executing the command
        /// </summary>
        /// <param name="args"> the arguments of the command </param>
        /// <param name="result"> declears if the executing was succeed </param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
            return m_Model.AddFile(args[0], out result);
        }
    }
}
