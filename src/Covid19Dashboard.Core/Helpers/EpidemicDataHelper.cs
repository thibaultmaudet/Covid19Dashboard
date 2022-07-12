using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;

namespace Covid19Dashboard.Core.Helpers
{
    public class EpidemicDataHelper
    {
        private static Data Data => Data.Instance;

        public static async Task DownloadEpidemicIndicatorsFiles(string folderPath)
        {
            if (Data.EpidemicIndicators == null)
            {
                Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(folderPath, Area.National);

                Data.Departments = await EpidemicDataService.GetDepartments(folderPath);
            }

            if (Data.VaccinationIndicators == null)
                Data.VaccinationIndicators = await EpidemicDataService.GetVaccinationIndicatorsAsync(folderPath, Area.National);

            if (!string.IsNullOrEmpty(Data.SelectedDepartment) && !Data.IsDepartementIndicatorsDownloaded)
            {
                Data.EpidemicIndicators.AddRange(await EpidemicDataService.GetEpedimicIndicatorsAsync(folderPath, Area.Department));
                Data.VaccinationIndicators.AddRange(await EpidemicDataService.GetVaccinationIndicatorsAsync(folderPath, Area.Department));

                Data.IsDepartementIndicatorsDownloaded = true;
            }
        }

        public static double GetEvolution(string property, bool isAverage, bool isNationalIndicator, Type indicatorType)
        {
            if (!IsInitializedList(indicatorType))
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(indicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));

            Expression ageExpression = indicatorType == typeof(VaccinationIndicator) ? Expression.Equal(Expression.Property(parameterExpression, "AgeClass"), Expression.Constant(0, typeof(int?))) : null;

            Indicator firstIndicator = new();

            if (indicatorType == typeof(EpidemicIndicator))
                firstIndicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                firstIndicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            return Math.Round(CalculateEvolution(GetValue<float>(property, isAverage, isNationalIndicator, 2, firstIndicator.Date.AddDays(-7), indicatorType), GetValue<float>(property, isAverage, isNationalIndicator, 2, indicatorType)), 2);
        }

        public static double GetEvolution(string property, bool isAverage, bool isNationalIndicator, DateTime? date, Type indicatorType)
        {
            if (!IsInitializedList(indicatorType))
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(indicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));
            Expression propertyHasValueExpression = Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?)));
            Expression specificDateExpression = Expression.LessThanOrEqual(Expression.Property(parameterExpression, "Date"), Expression.Constant(date ?? default, typeof(DateTime)));
            Expression ageExpression = indicatorType == typeof(VaccinationIndicator) ? Expression.Equal(Expression.Property(parameterExpression, "AgeClass"), Expression.Constant(0, typeof(int?))) : null;

            Indicator firstIndicator = new();

            if (indicatorType == typeof(EpidemicIndicator))
                firstIndicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());
            else
                firstIndicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

            return Math.Round(CalculateEvolution(GetValue<float>(property, isAverage, isNationalIndicator, 2, firstIndicator.Date.AddDays(-7), indicatorType), GetValue<float>(property, isAverage, isNationalIndicator, 2, date, indicatorType)), 2);
        }

        public static ObservableCollection<ChartIndicator> GetEvolutionForChart(string property, bool isAverage, bool isNationalIndicator, Type indicatorType)
        {
            if (!IsInitializedList(indicatorType))
                return default;

            ObservableCollection<ChartIndicator> chartIndicators = new();

            ParameterExpression parameterExpression = Expression.Parameter(indicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));
            Expression ageExpression = indicatorType == typeof(VaccinationIndicator) ? Expression.Equal(Expression.Property(parameterExpression, "AgeClass"), Expression.Constant(0, typeof(int?))) : null;

            IEnumerable<Indicator> indicators = null;

            if (indicatorType == typeof(EpidemicIndicator))
                indicators = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicators = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            foreach (Indicator epidemicIndicator in indicators.Take(100).Reverse())
                chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = GetEvolution(property, isAverage, isNationalIndicator, epidemicIndicator.Date, indicatorType) });

            return chartIndicators;
        }

        public static string GetLastUpdate(string property, bool isNationalIndicator, Type indicatorType)
        {
            if (!IsInitializedList(indicatorType))
                return default;

            ParameterExpression parameterExpression = Expression.Parameter(indicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));
            Expression ageExpression = indicatorType == typeof(VaccinationIndicator) ? Expression.Equal(Expression.Property(parameterExpression, "AgeClass"), Expression.Constant(0, typeof(int?))) : null;

            Indicator indicator = new Indicator();

            if (indicatorType == typeof(EpidemicIndicator))
                indicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            if (indicator != null)
                return indicator.Date.ToShortDateString();

            return default;
        }

        public static T GetValue<T>(string property, bool isAverage, bool isNationalIndicator, int digits, Type indicatorType)
        {
            return GetValue<T>(property, isAverage, isNationalIndicator, digits, null, indicatorType);
        }

        public static string GetValue(string property, bool isAverage, bool isNationalIndicator, int digits, Type indicatorType)
        {
            return GetValue<string>(property, isAverage, isNationalIndicator, digits, null, indicatorType);
        }

        public static T GetValue<T>(string property, bool isAverage, bool isNationalIndicator, int digits, DateTime? date, Type indicatorType)
        {
            if (!IsInitializedList(indicatorType))
                return (T)Convert.ChangeType(default, typeof(T));

            ParameterExpression parameterExpression = Expression.Parameter(indicatorType);

            Expression propertyHasValueExpression = Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?)));
            Expression specificDateExpression = Expression.LessThanOrEqual(Expression.Property(parameterExpression, "Date"), Expression.Constant(date ?? default, typeof(DateTime)));
            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));
            Expression ageExpression = indicatorType == typeof(VaccinationIndicator) ? Expression.Equal(Expression.Property(parameterExpression, "AgeClass"), Expression.Constant(0, typeof(int?))) : null;

            if (isAverage)
            {
                IEnumerable<Indicator> indicators = null;

                if (indicatorType == typeof(EpidemicIndicator))
                    indicators = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());
                else if (indicatorType == typeof(VaccinationIndicator))
                    indicators = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

                if (indicators == null || indicators.Count() == 0)
                    return (T)Convert.ChangeType(default, typeof(T));

                List<decimal> values = new();

                foreach (var indicator in indicators.Take(7))
                    values.Add(decimal.Parse(indicator.GetType().GetProperty(property).GetValue(indicator, null).ToString()));

                return (T)Convert.ChangeType(Math.Round(values.Average(), digits), typeof(T));
            }
            else
            {
                Indicator indicator = new();

                if (indicatorType == typeof(EpidemicIndicator))
                    indicator = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).DefaultIfEmpty(new EpidemicIndicator()).FirstOrDefault(Expression.Lambda<Func<EpidemicIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());
                else if (indicatorType == typeof(VaccinationIndicator))
                    indicator = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).DefaultIfEmpty(new VaccinationIndicator()).FirstOrDefault(Expression.Lambda<Func<VaccinationIndicator, bool>>(date.HasValue ? Expression.And(propertyHasValueExpression, specificDateExpression) : propertyHasValueExpression, parameterExpression).Compile());

                if (indicator != null)
                    return (T)Convert.ChangeType(Math.Round(decimal.Parse(indicator.GetType().GetProperty(property).GetValue(indicator, null).ToString()), digits), typeof(T));
            }

            return (T)Convert.ChangeType(default, typeof(T));
        }

        public static ObservableCollection<ChartIndicator> GetValuesForChart(string property, bool isAverage, bool isNationalIndicator, int digits, Type indicatorType)
        {
            if (!IsInitializedList(indicatorType))
                return default;

            ObservableCollection<ChartIndicator> chartIndicators = new();

            ParameterExpression parameterExpression = Expression.Parameter(indicatorType);

            Expression departmentExpression = Expression.Equal(Expression.Property(parameterExpression, "Department"), Expression.Constant(isNationalIndicator || string.IsNullOrEmpty(Data.SelectedDepartment) ? default : Data.SelectedDepartment, typeof(string)));
            Expression ageExpression = indicatorType == typeof(VaccinationIndicator) ? Expression.Equal(Expression.Property(parameterExpression, "AgeClass"), Expression.Constant(0, typeof(int?))) : null;

            IEnumerable<Indicator> indicators = null;

            if (indicatorType == typeof(EpidemicIndicator))
                indicators = Data.EpidemicIndicators.Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<EpidemicIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());
            else
                indicators = Data.VaccinationIndicators.Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(departmentExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(ageExpression, parameterExpression).Compile()).Where(Expression.Lambda<Func<VaccinationIndicator, bool>>(Expression.NotEqual(Expression.Property(parameterExpression, property), Expression.Constant(null, typeof(int?))), parameterExpression).Compile());

            foreach (Indicator epidemicIndicator in indicators.Take(70).Reverse())
                chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = GetValue<float>(property, isAverage, isNationalIndicator, digits, epidemicIndicator.Date, indicatorType) });

            return chartIndicators;
        }

        private static bool IsInitializedList(Type indicatorType)
        {
            return !(indicatorType == typeof(EpidemicIndicator) && (Data.EpidemicIndicators == null || Data.EpidemicIndicators.Count == 0)) || (indicatorType == typeof(VaccinationIndicator) && (Data.VaccinationIndicators == null || Data.VaccinationIndicators.Count == 0));
        }

        private static float CalculateEvolution(float? firstValue, float? secondValue)
        {
            if (firstValue == null || secondValue == null || !firstValue.HasValue || !secondValue.HasValue)
                return 0;

            return (secondValue.Value - firstValue.Value) / firstValue.Value * 100;
        }
    }
}
