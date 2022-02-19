using System.ComponentModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplatePart(Name = PartIconPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartDescriptionPresenter, Type = typeof(ContentPresenter))]
    public class Setting : ContentControl
    {
        private const string PartIconPresenter = "IconPresenter";
        private const string PartDescriptionPresenter = "DescriptionPresenter";
        private ContentPresenter iconPresenter;
        private ContentPresenter descriptionPresenter;
        private Setting setting;

        public Setting()
        {
            DefaultStyleKey = typeof(Setting);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Setting), new PropertyMetadata(default(string), OnHeaderChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(object), typeof(Setting), new PropertyMetadata(null, OnDescriptionChanged));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(Setting), new PropertyMetadata(default(string), OnIconChanged));

        public static readonly DependencyProperty ActionContentProperty = DependencyProperty.Register("ActionContent", typeof(object), typeof(Setting), null);

        [Localizable(true)]
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        [Localizable(true)]
        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public object ActionContent
        {
            get => GetValue(ActionContentProperty);
            set => SetValue(ActionContentProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            IsEnabledChanged -= Setting_IsEnabledChanged;
            setting = this;
            iconPresenter = (ContentPresenter)setting.GetTemplateChild(PartIconPresenter);
            descriptionPresenter = (ContentPresenter)setting.GetTemplateChild(PartDescriptionPresenter);
            Update();
            SetEnabledState();
            IsEnabledChanged += Setting_IsEnabledChanged;

            base.OnApplyTemplate();
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Setting)d).Update();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Setting)d).Update();
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Setting)d).Update();
        }

        private void Setting_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetEnabledState();
        }

        private void SetEnabledState()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }

        private void Update()
        {
            if (setting == null)
                return;

            if (setting.ActionContent != null && setting.ActionContent.GetType() != typeof(Button) && !string.IsNullOrEmpty(setting.Header))
                AutomationProperties.SetName((UIElement)setting.ActionContent, setting.Header);

            if (setting.iconPresenter != null)
                setting.iconPresenter.Visibility = setting.Icon == null ? Visibility.Collapsed : Visibility.Visible;

            if (setting.descriptionPresenter != null)
                setting.descriptionPresenter.Visibility = setting.Description == null ? Visibility.Collapsed :  Visibility.Visible;
        }
    }
}
