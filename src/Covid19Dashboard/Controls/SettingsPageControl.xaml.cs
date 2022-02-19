using System.Collections.ObjectModel;

using Covid19Dashboard.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    public sealed partial class SettingsPageControl : UserControl
    {
        public static readonly DependencyProperty ModuleTitleProperty = DependencyProperty.Register("ModuleTitle", typeof(string), typeof(SettingsPageControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ModuleDescriptionProperty = DependencyProperty.Register("ModuleDescription", typeof(string), typeof(SettingsPageControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PrimaryLinksProperty = DependencyProperty.Register("PrimaryLinks", typeof(ObservableCollection<PageLink>), typeof(SettingsPageControl), new PropertyMetadata(new ObservableCollection<PageLink>()));
        public static readonly DependencyProperty SecondaryLinksHeaderProperty = DependencyProperty.Register("SecondaryLinksHeader", typeof(string), typeof(SettingsPageControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty SecondaryLinksProperty = DependencyProperty.Register("SecondaryLinks", typeof(ObservableCollection<PageLink>), typeof(SettingsPageControl), new PropertyMetadata(new ObservableCollection<PageLink>()));
        public static readonly DependencyProperty ModuleContentProperty = DependencyProperty.Register("ModuleContent", typeof(object), typeof(SettingsPageControl), new PropertyMetadata(new Grid()));

        public string ModuleTitle
        {
            get { return (string)GetValue(ModuleTitleProperty); }
            set { SetValue(ModuleTitleProperty, value); }
        }

        public string ModuleDescription
        {
            get => (string)GetValue(ModuleDescriptionProperty);
            set => SetValue(ModuleDescriptionProperty, value);
        }

        public string SecondaryLinksHeader
        {
            get { return (string)GetValue(SecondaryLinksHeaderProperty); }
            set { SetValue(SecondaryLinksHeaderProperty, value); }
        }

        public ObservableCollection<PageLink> SecondaryLinks
        {
            get => (ObservableCollection<PageLink>)GetValue(SecondaryLinksProperty);
            set => SetValue(SecondaryLinksProperty, value);
        }

        public object ModuleContent
        {
            get { return GetValue(ModuleContentProperty); }
            set { SetValue(ModuleContentProperty, value); }
        }

        public SettingsPageControl()
        {
            InitializeComponent();
            PrimaryLinks = new ObservableCollection<PageLink>();
            SecondaryLinks = new ObservableCollection<PageLink>();
        }

        public ObservableCollection<PageLink> PrimaryLinks
        {
            get => (ObservableCollection<PageLink>)GetValue(PrimaryLinksProperty);
            set => SetValue(PrimaryLinksProperty, value);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PrimaryLinksControl.Focus(FocusState.Programmatic);
        }
    }
}
