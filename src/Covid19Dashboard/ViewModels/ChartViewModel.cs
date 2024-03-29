﻿using System;
using System.Collections.Generic;

using Covid19Dashboard.Core.Enums;
using Covid19Dashboard.Core.Models;

using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Covid19Dashboard.ViewModels
{
    public class ChartViewModel : ObservableObject
    {
        private ChartType chartType;

        private ISeries[] series;

        private List<ChartIndicators> chartIndicators;

        public ChartType ChartType
        {
            get { return chartType; }
            set { SetProperty(ref chartType, value); }
        }

        public ISeries[] Series
        {
            get { return series; }
            set { SetProperty(ref series, value); }
        }

        public List<ChartIndicators> ChartIndicators
        {
            get { return chartIndicators; }
            set { SetProperty(ref chartIndicators, value); }
        }

        public IEnumerable<ICartesianAxis> XAxes { get; set; }

        public ChartViewModel()
        {
            XAxes = new Axis[] { new Axis { Labeler = value => new DateTime((long)value).ToShortDateString(), UnitWidth = TimeSpan.FromDays(1).Ticks, MinStep = TimeSpan.FromDays(7).Ticks } };
        }
    }
}
