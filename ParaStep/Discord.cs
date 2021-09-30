using System.Reflection;
using System.Threading;
using DiscordRPC;

namespace ParaStep
{
    public class Discord
    {
        public RichPresence state;
        public DiscordRpcClient Client;
        public Thread PresenceThread;
        public Discord(string id)
        {
            Client = new DiscordRpcClient(id);
            Client.Initialize();

            state = new RichPresence()
            {
                Assets = new Assets()
                {
                    LargeImageKey = "two",
                    LargeImageText = $"ParaStep Engine, v{Assembly.GetExecutingAssembly().GetName().Version}"
                },
                Timestamps = new Timestamps()
            };
            PresenceThread = new Thread(async =>
            {
                while (true)
                {
                    Client.SetPresence(state);
                    Thread.Sleep(2274);
                }
            });
            PresenceThread.Start();
        }

        
    }
}