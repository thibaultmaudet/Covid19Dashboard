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
        private static Data Data => Data.Instance;

        public static Type IndicatorType { get; set; }

        public static double GetEvolution(string property, bool isAverage, bool isNationalIndicator)
        {
            if (!IsInitializedList())
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));

            Indicator firstIndicator = new Indicator();
            
            if (IndicatorType == typeof(EpidemicIndicator))
                firstIndicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                firstIndicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            return Math.Round(CalculateEvolution(GetValue<float>(property, isAverage, isNationalIndicator, 2, firstIndicator.Date.AddDays(-7)), GetValue<float>(property, isAverage, isNationalIndicator, 2)), 2);
        }

        public static string GetLastUpdate(string property, bool isNationalIndicator)
        {
            if (!IsInitializedList())
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);
            
            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));

            Indicator indicator = new Indicator();

            if (IndicatorType == typeof(EpidemicIndicator))
                indicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            if (indicator != null)
                return indicator.Date.ToShortDateString();

            return default;
        }

        public static T GetValue<T>(string property, bool isAverage, bool isNationalIndicator, int digits)
        {
            return GetValue<T>(property, isAverage, isNationalIndicator, digits, null);
        }
        
        public static string GetValue(string property, bool isAverage, bool isNationalIndicator, int digits)
        {
            return GetValue<string>(property, isAverage, isNationalIndicator, digits, null);
        }

        public static T GetValue<T>(string property, bool isAverage, bool isNationalIndicator, int digits, DateTime? date)
        {
            if (!IsInitializedList())
                return (T)Convert.ChangeType(default, typeof(T));

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            Expression propertyHasValueExpression = Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?)));
            Expression specificDateExpression = Expression.LessThanOrEqual(Expression.Property(parameterExpression, "Date"), Expression.Constant(date ?? default, typeof(DateTime)));
            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));

            if (isAverage)
            {
                IEnumerable<Indicator> indicators = null;

                if (IndicatorType == typeof(EpidemicIndicator))
                    indicators = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());
                else if (IndicatorType == typeof(VaccinationIndicator))
                    indicators = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

                if (indicators == null || indicators.Count() == 0)
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
                    indicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).DefaultIfEmpty(new EpidemicIndicator()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());
                else if (IndicatorType == typeof(VaccinationIndicator))
                    indicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).DefaultIfEmpty(new VaccinationIndicator()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

                if (indicator != null)
                    return (T)Convert.ChangeType(Math.Round(decimal.Parse(indicator.GetType().GetProperty(property).GetValue(indicator, null).ToString()), digits), typeof(T));
            }

            return (T)Convert.ChangeType(default, typeof(T));
        }

        public static ObservableCollection<ChartIndicator> GetValuesForChart(string property, bool isAverage, bool isNationalIndicator, int digits)
        {
            if (!IsInitializedList())
                return default;

            ObservableCollection<ChartIndicator> chartIndicators = new ObservableCollection<ChartIndicator>();

            ParameterExpression parameterExpression = Expression.Parameter(IndicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));

            IEnumerable<Indicator> indicators = null;

            if (IndicatorType == typeof(EpidemicIndicator))
                indicators = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicators = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            foreach (Indicator epidemicIndicator in indicators.Take(70).Reverse())
                chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = GetValue<float>(property, isAverage, isNationalIndicator, digits, epidemicIndicator.Date) });

            return chartIndicators;
        }

        public static ObservableCollection<KeyValuePair<string, string>> GetDepartments()
        {
            if (!IsInitializedList())
                return default;

            ObservableCollection<KeyValuePair<string, string>> departments = new ObservableCollection<KeyValuePair<string, string>>();

            Data.EpidemicIndicators.Where(x => x.Date == Data.EpidemicIndicators.First().Date && x.Department != null).ToList().ForEach(x => departments.Add(new KeyValuePair<string, string>(x.Department,x.DepartmentLabel)));

            return departments;
        }

        private static bool IsInitializedList()
        {
            return !(IndicatorType == typeof(EpidemicIndicator) && (Data.EpidemicIndicators == null || Data.EpidemicIndicators.Count == 0)) || (IndicatorType == typeof(VaccinationIndicator) && (Data.VaccinationIndicators == null || Data.VaccinationIndicators.Count == 0));
        }

        private static float CalculateEvolution(float? firstValue, float? secondValue)
        {
            if (firstValue == null || secondValue == null || !firstValue.HasValue || !secondValue.HasValue)
                return 0;

            return (secondValue.Value - firstValue.Value) / firstValue.Value * 100;
        }
    }
}
