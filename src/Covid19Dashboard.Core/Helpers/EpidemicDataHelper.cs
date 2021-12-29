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

        public static double GetEvolution(string property)
        {
            return GetEvolution(property, false);
        }

        public static double GetEvolution(string property, bool isAverage)
        {
            if (EpidemicIndicators == null || EpidemicIndicators.Count == 0)
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(typeof(EpidemicIndicator));

            EpidemicIndicator firstEpidemicIndicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            return Math.Round(CalculateEvolution(GetValue<float>(property, isAverage, 2, firstEpidemicIndicator.Date.AddDays(-7)), GetValue<float>(property, isAverage, 2)), 2);
        }

        public static string GetLastUpdate(string property)
        {
            if (EpidemicIndicators == null || EpidemicIndicators.Count == 0)
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(typeof(EpidemicIndicator));

            EpidemicIndicator epidemicIndicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            if (epidemicIndicator != null)
                return epidemicIndicator.Date.ToShortDateString();

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
            if (EpidemicIndicators == null || EpidemicIndicators.Count == 0)
                return (T)Convert.ChangeType(default, typeof(T));

            ParameterExpression parameterExpression = Expression.Parameter(typeof(EpidemicIndicator));

            Expression propertyHasValueExpression = Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?)));
            Expression specificDateExpression = Expression.LessThanOrEqual(Expression.Property(parameterExpression, "Date"), Expression.Constant(date ?? default, typeof(DateTime)));

            if (isAverage)
            {
                IEnumerable<EpidemicIndicator> epidemicIndicators = null;

                if (date.HasValue)
                    epidemicIndicators = EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.And(propertyHasValueExpression, specificDateExpression), parameterExpression).Compile());
                else
                    epidemicIndicators = EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(propertyHasValueExpression, parameterExpression).Compile());

                List<decimal> values = new List<decimal>();

                if (epidemicIndicators == null)
                    return (T)Convert.ChangeType(default, typeof(T));

                foreach (EpidemicIndicator epidemicIndicator in epidemicIndicators.Take(7))
                    values.Add(decimal.Parse(epidemicIndicator.GetType().GetProperty(property).GetValue(epidemicIndicator, null).ToString()));

                return (T)Convert.ChangeType(Math.Round(values.Average(), digits), typeof(T));
            }
            else
            {
                EpidemicIndicator epidemicIndicator = null;
                if (date.HasValue)
                    epidemicIndicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.And(propertyHasValueExpression, specificDateExpression), parameterExpression).Compile());
                else
                    epidemicIndicator = EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(propertyHasValueExpression, parameterExpression).Compile());
                
                EpidemicIndicators.FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

                if (epidemicIndicator != null)
                    return (T)Convert.ChangeType(Math.Round(decimal.Parse(epidemicIndicator.GetType().GetProperty(property).GetValue(epidemicIndicator, null).ToString()), digits), typeof(T));
            }

            return (T)Convert.ChangeType(default, typeof(T));
        }

        public static ObservableCollection<ChartIndicator> GetValuesForChart(string property, bool isAverage, int digits)
        {
            if (EpidemicIndicators == null || EpidemicIndicators.Count == 0)
                return default;

            ObservableCollection<ChartIndicator> chartIndicators = new ObservableCollection<ChartIndicator>();

            ParameterExpression parameterExpression = Expression.Parameter(typeof(EpidemicIndicator));

            IEnumerable<EpidemicIndicator> epidemicIndicators = EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            foreach (EpidemicIndicator epidemicIndicator in epidemicIndicators.Take(70).Reverse())
                chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = GetValue<float>(property, isAverage, digits, epidemicIndicator.Date) });

            return chartIndicators;
        }

        private static float CalculateEvolution(float? firstValue, float? secondValue)
        {
            if (firstValue == null || secondValue == null || !firstValue.HasValue || !secondValue.HasValue)
                return 0;

            return (secondValue.Value - firstValue.Value) / firstValue.Value * 100;
        }
    }
}
