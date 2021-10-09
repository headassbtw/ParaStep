using System;

namespace ParaStep.QtErrorHandler.QML_Types
{
    public class NetExceptionModel
    {
        public NetException GetNetException()
        {
            return new NetException()
            {
                Header = Program.Exception.Message,
                StackTrace = Program.Exception.StackTrace
            };
        } 
        
        public class NetException
        {
            public string Header { get; set; }
            public string StackTrace { get; set; }
        }
    }
}