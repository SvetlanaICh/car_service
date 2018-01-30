using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService
{
    class Startup
    {
        [STAThread()]
        static void Main()
        {
            Application app = new Application();
            AllCreator allCreator = new AllCreator();
            app.Run(allCreator.TheMainWindow);
        }
    }
}
