using Covid19Dashboard.Models;
using System;
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

        public event EventHandler SeeMoreDetailsClick;

        public TileControl()
        {
            InitializeComponent();
        }

        private void SeeMoreDetailsHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            SeeMoreDetailsClick?.Invoke(sender, new EventArgs());
        }
    }
}
