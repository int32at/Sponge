using NLog.Targets;

namespace Sponge.Logging
{
    [Target("UlsTarget")]
    public class UlsTarget : TargetWithLayout
    {
        protected override void Write(NLog.LogEventInfo logEvent)
        {
            var msg = Layout.Render(logEvent);
            var lvl = logEvent.Level;

            UlsLogger.Log(lvl, msg);
        }
    }
}
