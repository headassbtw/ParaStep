using System;
using System.Linq;

namespace ParaStep.QtErrorHandler.QML_Types
{
    public class QuitButton
    {
        public void QuitApp()
        {
            Console.WriteLine("Hi from Qt!");
            //System.Diagnostics.Process.GetProcessesByName("ParaStep").FirstOrDefault().Kill();
        }
    }
}