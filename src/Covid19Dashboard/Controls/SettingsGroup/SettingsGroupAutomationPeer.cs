
using Windows.UI.Xaml.Automation.Peers;

namespace Covid19Dashboard.Controls
{
    public class SettingsGroupAutomationPeer : FrameworkElementAutomationPeer
    {
        public SettingsGroupAutomationPeer(SettingsGroup owner) : base(owner)
        {
        }

        protected override string GetNameCore()
        {
            return ((SettingsGroup)Owner).Header;
        }
    }
}
