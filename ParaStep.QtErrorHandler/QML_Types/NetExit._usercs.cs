using System.Linq;

namespace ParaStep.QtErrorHandler.QML_Types
{
    public class NetExitHandler_User
    {
        public NetExit_User GetNetExit()
        {
            return new NetExit_User();
        } 
        
        public class NetExit_User
        {
            public void Run()
            {
                Program.UserCrashApp.Dispose();
            }
        }
    }
}