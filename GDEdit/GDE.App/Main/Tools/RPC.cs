using DiscordRPC;
using osu.Framework.Logging;
using System;

namespace GDE.App.Main.Tools
{
    /// <summary>This class will represnt Discord's RPC Functions (more to be added soon though).</summary>
    public static class RPC
    {
        /// <summary>Represents the application from Discord's dev portal.</summary>
        public static DiscordRpcClient Client = new DiscordRpcClient("533431726463123458");

        #region Update RPC
        /// <summary>Initializes and changes the Rich Presence in Discord.</summary>
        /// <param name="status">The Status text that will appear in Discord.</param>
        /// <param name="details">The Details text that will appear in Discord.</param>
        public static void UpdatePresence(string status, string details) => UpdatePresence(status, details, null, null);
        /// <summary>Initializes and changes the Rich Presence in Discord.</summary>
        /// <param name="status">The Status text that will appear in Discord.</param>
        /// <param name="details">The Details text that will appear in Discord.</param>
        /// <param name="assets">The Assets images that will appear in Discord.</param>
        public static void UpdatePresence(string status, string details, Assets assets) => UpdatePresence(status, details, assets, null);
        /// <summary>Initializes and changes the Rich Presence in Discord.</summary>
        /// <param name="status">The Status text that will appear in Discord.</param>
        /// <param name="details">The Details text that will appear in Discord.</param>
        /// <param name="assets">The Assets images that will appear in Discord.</param>
        /// <param name="timestamp">The Timestamp time that will appear in Discord.</param>
        public static void UpdatePresence(string status, string details, Assets assets, Timestamps timestamp)
        {
            Client.Initialize();

            Client.SetPresence(new RichPresence
            {
                State = status,
                Details = details,
                Timestamps = timestamp,
                Assets = assets
            });

            Logger.Error(new Exception().InnerException, $"Changed RPC to {status}", LoggingTarget.Information);

            Client.Invoke();
        }
        #endregion
    }
}
