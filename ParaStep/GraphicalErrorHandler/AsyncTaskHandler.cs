using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParaStep.GraphicalErrorHandler
{
    public class AsyncTaskHandler
    {
        public static async Task Handle(Task<MessageBox.MessageBoxResult> task)
        {
            await task;
            switch (task.Result)
            {
                case MessageBox.MessageBoxResult.Ok:
                    Program.Game.Exit();
                    //AHAHHAHAHAHHAA
                    Process.GetProcessesByName("ParaStep").First().Kill();
                    break;
                case MessageBox.MessageBoxResult.Cancel:
                    
                    break;
            }
            
        }
    }
}