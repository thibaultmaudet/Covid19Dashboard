using System.ComponentModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplatePart(Name = PartDescriptionPresenter, Type = typeof(ContentPresenter))]
    public partial class SettingsGroup : ItemsControl
    {
        private const string PartDescriptionPresenter = "DescriptionPresenter";
        private ContentPresenter descriptionPresenter;
        private SettingsGroup settingsGroup;

        public SettingsGroup()
        {
            DefaultStyleKey = typeof(SettingsGroup);
        }

        [Localizable(true)]
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(SettingsGroup), new PropertyMetadata(default(string)));

        [Localizable(true)]
        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(object), typeof(SettingsGroup), new PropertyMetadata(null, OnDescriptionChanged));

        protected override void OnApplyTemplate()
        {
            IsEnabledChanged -= SettingsGroup_IsEnabledChanged;
            settingsGroup = this;
            descriptionPresenter = (ContentPresenter)settingsGroup.GetTemplateChild(PartDescriptionPresenter);
            SetEnabledState();
            IsEnabledChanged += SettingsGroup_IsEnabledChanged;

            base.OnApplyTemplate();
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SettingsGroup)d).Update();
        }

        private void SettingsGroup_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetEnabledState();
        }

        private void SetEnabledState()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }

        private void Update()
        {
            if (settingsGroup == null)
                return;

            settingsGroup.descriptionPresenter.Visibility = settingsGroup.Description != null ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new SettingsGroupAutomationPeer(this);
        }
    }
}
