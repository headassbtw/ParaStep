using System.Linq;

namespace ParaStep.QtErrorHandler.QML_Types
{
    public class NetExitHandler
    {
        public NetExit GetNetExit()
        {
            return new NetExit();
        } 
        
        public class NetExit
        {
            public void Run()
            {
                System.Diagnostics.Process.GetProcessesByName("ParaStep").FirstOrDefault().Kill();
            }
        }
    }
}