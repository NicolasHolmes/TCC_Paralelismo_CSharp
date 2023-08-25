using System;

namespace ExtractBot.Api
{
    public class BotConfig
    {
        public DateTime StartAt { get; set; }

        #region Singleton
        private static BotConfig _instance = new BotConfig();
        private BotConfig() { }
        public static BotConfig Instance { get { return _instance; } }
        #endregion

        public void StartClock()
        {
            StartAt = DateTime.Now;
        }

        public TimeSpan TimePass()
        {
            return DateTime.Now - StartAt;
        }
    }
}
