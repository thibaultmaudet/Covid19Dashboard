using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Helpers
{
    public class NavHelper
    {
        // This helper class allows to specify the page that will be shown when you click on a NavigationViewItem
        //
        // Usage in xaml:
        // <winui:NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="views:MainPage" />
        //
        // Usage in code:
        // NavHelper.SetNavigateTo(navigationViewItem, typeof(MainPage));
        public static Type GetNavigateTo(Microsoft.UI.Xaml.Controls.NavigationViewItem item)
        {
            return (Type)item.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(Microsoft.UI.Xaml.Controls.NavigationViewItem item, Type value)
        {
            item.SetValue(NavigateToProperty, value);
        }

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(Type), typeof(NavHelper), new PropertyMetadata(null));

        public static Type GetNavigateToChart(HyperlinkButton item)
        {
            return (Type)item.GetValue(NavigateToChartProperty);
        }

        public static void SetNavigateToChart(HyperlinkButton item, Type value)
        {
            item.SetValue(NavigateToChartProperty, value);
        }

        public static readonly DependencyProperty NavigateToChartProperty =
            DependencyProperty.RegisterAttached("NavigateToChart", typeof(Type), typeof(NavHelper), new PropertyMetadata(null));
    }
}
