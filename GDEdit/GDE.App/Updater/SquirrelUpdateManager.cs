using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using Squirrel;
using System;
using System.Threading.Tasks;

namespace GDE.App.Updater
{
    public class SquirrelUpdateManager : Component
    {
        private UpdateManager updateManager;

        public void PrepareUpdate()
        {
            UpdateManager.RestartAppWhenExited().Wait();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Schedule(() => Task.Run(() => CheckForUpdateAsync()));
            Logger.Log("Called \"checkForUpdateAsync()\"");
        }

        private async void CheckForUpdateAsync(bool useDeltaPatching = true)
        {
            bool scheduleRetry = true;
            try
            {
                if (updateManager == null)
                    updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/gd-edit/GDE", "GDEdit", null, null, true);

                var info = await updateManager.CheckForUpdate(!useDeltaPatching);
                if (info.ReleasesToApply.Count == 0)
                    return;

                Logger.Log("Checking for updates");

                try
                {
                    await updateManager.DownloadReleases(info.ReleasesToApply);
                    await updateManager.ApplyReleases(info);
                }
                catch (Exception e)
                {
                    if (useDeltaPatching)
                    {
                        Logger.Error(e, @"delta patching failed!");

                        CheckForUpdateAsync(false);
                        scheduleRetry = false;
                    }
                    else
                        Logger.Error(e, @"update failed!");
                }
            }
            catch (Exception) { }
            finally
            {
                if (scheduleRetry)
                    Scheduler.AddDelayed(() => CheckForUpdateAsync(), 6000 * 30);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            updateManager?.Dispose();
        }
    }
}
