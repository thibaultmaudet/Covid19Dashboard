using Covid19Dashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Covid19Dashboard.Core.Helpers
{
    public class EpidemicDataHelper
    {
        public static List<EpidemicIndicator> EpidemicIndicators { get; set; }

        public static List<VaccinationIndicator> VaccinationIndicators { get; set; }

        public static Type IndicatorType { get; set; }

        public static double GetEvolution(string property)
        {
            return GetEvolution(property, false);
        }

        public static double GetEvolution(string property, bool isAverage)
        {
            if (!IsInitilizedList())
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            Indicator firstIndicator = new Indicator();
            
            if (IndicatorType == typeof(EpidemicIndicator))
                firstIndicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                firstIndicator = VaccinationIndicators.FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            return Math.Round(CalculateEvolution(GetValue<float>(property, isAverage, 2, firstIndicator.Date.AddDays(-7)), GetValue<float>(property, isAverage, 2)), 2);
        }

        public static string GetLastUpdate(string property)
        {
            if (!IsInitilizedList())
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            Indicator indicator = new Indicator();

            if (IndicatorType == typeof(EpidemicIndicator))
                indicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicator = VaccinationIndicators.FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            if (indicator != null)
                return indicator.Date.ToShortDateString();

            return default;
        }

        public static string GetValue(string property)
        {
            return GetValue<string>(property, false, 0, null);
        }

        public static string GetValue(string property, bool isAverage)
        {
            return GetValue<string>(property, isAverage, 2, null);
        }

        public static string GetValue(string property, int digits)
        {
            return GetValue<string>(property, false, digits, null);
        }

        public static T GetValue<T>(string property, bool isAverage, int digits)
        {
            return GetValue<T>(property, isAverage, digits, null);
        }

        public static T GetValue<T>(string property, bool isAverage, int digits, DateTime? date)
        {
            if (!IsInitilizedList())
                return (T)Convert.ChangeType(default, typeof(T));

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            Expression propertyHasValueExpression = Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?)));
            Expression specificDateExpression = Expression.LessThanOrEqual(Expression.Property(parameterExpression, "Date"), Expression.Constant(date ?? default, typeof(DateTime)));

            if (isAverage)
            {
                IEnumerable<Indicator> indicators = null;

                if (IndicatorType == typeof(EpidemicIndicator))
                    indicators = EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());            
                else if (IndicatorType == typeof(VaccinationIndicator))
                    indicators = VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

                if (indicators == null)
                    return (T)Convert.ChangeType(default, typeof(T));
                
                List<decimal> values = new List<decimal>();

                foreach (var indicator in indicators.Take(7))
                    values.Add(decimal.Parse(indicator.GetType().GetProperty(property).GetValue(indicator, null).ToString()));

                return (T)Convert.ChangeType(Math.Round(values.Average(), digits), typeof(T));
            }
            else
            {
                Indicator indicator = new Indicator();

                if (IndicatorType == typeof(EpidemicIndicator))
                    indicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());
                else if (IndicatorType == typeof(VaccinationIndicator))
                    indicator = VaccinationIndicators.FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

                if (indicator != null)
                    return (T)Convert.ChangeType(Math.Round(decimal.Parse(indicator.GetType().GetProperty(property).GetValue(indicator, null).ToString()), digits), typeof(T));
            }

            return (T)Convert.ChangeType(default, typeof(T));
        }

        public static ObservableCollection<ChartIndicator> GetValuesForChart(string property, bool isAverage, int digits)
        {
            if (!IsInitilizedList())
                return default;

            ObservableCollection<ChartIndicator> chartIndicators = new ObservableCollection<ChartIndicator>();

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            IEnumerable<Indicator> indicators = null;

            if (IndicatorType == typeof(EpidemicIndicator))
                indicators = EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicators = VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            foreach (Indicator epidemicIndicator in indicators.Take(70).Reverse())
                chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = GetValue<float>(property, isAverage, digits, epidemicIndicator.Date) });

            return chartIndicators;
        }

        private static bool IsInitilizedList()
        {
            return !(IndicatorType == typeof(EpidemicIndicator) && (EpidemicIndicators == null || EpidemicIndicators.Count == 0)) || (IndicatorType == typeof(VaccinationIndicator) && (VaccinationIndicators == null || VaccinationIndicators.Count == 0));
        }

        private static float CalculateEvolution(float? firstValue, float? secondValue)
        {
            if (firstValue == null || secondValue == null || !firstValue.HasValue || !secondValue.HasValue)
                return 0;

            return (secondValue.Value - firstValue.Value) / firstValue.Value * 100;
        }
    }
}
