using GDE.App.Main;
using osu.Framework;
using osu.Framework.Platform;

namespace GDE.App
{
    public static class Program
    {
        public static void Main()
        {
            Game app = new GDEApp();
            DesktopGameHost host = Host.GetSuitableHost("GDE");

            host.Run(app);
        }
    }
}