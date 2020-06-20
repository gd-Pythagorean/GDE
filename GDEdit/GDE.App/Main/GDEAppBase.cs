using System;
using System.Threading;
using System.Threading.Tasks;
using GDAPI.Application;
using GDAPI.Application.Editors;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Development;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace GDE.App.Main
{
    public class GDEAppBase : Game
    {
        private const string mainResourceFile = "GDE.Resources.dll";

        private static int allowableExceptions = DebugUtils.IsDebugBuild ? 0 : 1;
        private readonly Storage storage;

        private DependencyContainer dependencies;

        public GDEAppBase()
        {
            storage = new NativeStorage("GD Edit");
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(mainResourceFile));

            dependencies.Cache(storage);

            dependencies.Cache(this);
            dependencies.CacheAs(new Editor(null));
            dependencies.CacheAs(new DatabaseCollection());
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            var config = new FrameworkConfigManager(storage);

            Window.Title = @"GD Edit";

            host.ExceptionThrown += ExceptionHandler;
        }

        protected virtual bool ExceptionHandler(Exception arg)
        {
            var continueExecution = Interlocked.Decrement(ref allowableExceptions) >= 0;

            Logger.Log(
                $"Unhandled exception has been {(continueExecution ? $"allowed with {allowableExceptions} more allowable exceptions" : "denied")}.");
            Task.Delay(1000).ContinueWith(_ => Interlocked.Increment(ref allowableExceptions));

            return continueExecution;
        }
    }
}