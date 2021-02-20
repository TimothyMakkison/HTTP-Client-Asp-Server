using HTTP_Client_Asp_Server.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public interface IBuilder
    {
        public IBuilder AddCommand(CommandModel command);
        public IBuilder SetContainer(Container container);
        public IBuilder Scan();
        public CommandLineHandler Build();

    }
}
