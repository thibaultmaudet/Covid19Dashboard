using System;

using Covid19Dashboard.Helpers;
using Covid19Dashboard.Models;
using Covid19Dashboard.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    public sealed partial class TileControl : UserControl
    {
        public static readonly DependencyProperty DataTileProperty = DependencyProperty.Register("DataTile", typeof(DataTile), typeof(TileControl), new PropertyMetadata(default(DataTile)));

        public DataTile DataTile
        {
            get { return (DataTile)GetValue(DataTileProperty); }
            set { SetValue(DataTileProperty, value); }
        }

        public TileControl()
        {
            InitializeComponent();
        }

        private void HyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hyperlink = sender as HyperlinkButton;
            Type pageType = hyperlink?.GetValue(NavHelper.NavigateToChartProperty) as Type;

            if (pageType != null)
                NavigationService.Navigate(pageType, DataTile.ChartIndicators);
        }
    }
}
