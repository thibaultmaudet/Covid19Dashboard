using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Models;

namespace Covid19Dashboard.Helpers
{
    public class TileHelper
    {
        private static Data Data { get => Data.Instance; }

        public static ObservableCollection<DataTiles> GetDataTiles()
        {
            DataTiles epidemilogicalDataTiles = new() { CategoryTitle = "Home_EpidemiologyDataTitle".GetLocalized(), IsHomeTiles = true, Page = Page.Epidemiologic };
            epidemilogicalDataTiles.AddRange(GetEpidemilogicalDataTiles());

            DataTiles hospitalDataTiles = new() { CategoryTitle = "Home_HospitalDataTitle".GetLocalized(), IsHomeTiles = true, Page = Page.Hospital };
            hospitalDataTiles.AddRange(GetHospitalDataTiles());

            DataTiles vaccinationDataTiles = new() { CategoryTitle = "Home_VaccinationTrackingTitle".GetLocalized(), IsHomeTiles = true, Page = Page.Vaccination };
            vaccinationDataTiles.AddRange(GetVaccinacinationDataTiles());

            DataTiles evolutionDataTiles = new() { IsHomeTiles = false, Page = Page.Evolution };
            evolutionDataTiles.AddRange(GetEvolutionIndicators());

            return new() { epidemilogicalDataTiles, hospitalDataTiles, vaccinationDataTiles, evolutionDataTiles };
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool isNationalIndicator, bool withEvolution, Type indicatorType, List<DataIndicator> dataIndicators)
        {
            return SetDataTile(property, isAverage, isNationalIndicator, withEvolution, 0, indicatorType, dataIndicators, false); ;
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool isNationalIndicator, bool withEvolution, Type indicatorType, List<DataIndicator> dataIndicators, bool isHomeTile)
        {
            return SetDataTile(property, isAverage, isNationalIndicator, withEvolution, 0, indicatorType, dataIndicators, isHomeTile);
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool withEvolution, Type indicatorType, List<DataIndicator> dataIndicators)
        {
            return SetDataTile(property, isAverage, withEvolution, 0, indicatorType, dataIndicators, false);
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool withEvolution, Type indicatorType, List<DataIndicator> dataIndicators, bool isHomeTile)
        {
            return SetDataTile(property, isAverage, withEvolution, 0, indicatorType, dataIndicators, isHomeTile);
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool withEvolution, int digits, Type indicatorType, List<DataIndicator> dataIndicators)
        {
            return SetDataTile(property, isAverage, withEvolution, digits, indicatorType, dataIndicators, false);
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool withEvolution, int digits, Type indicatorType, List<DataIndicator> dataIndicators, bool isHomeTile)
        {
            return SetDataTile(property, isAverage, false, withEvolution, digits, indicatorType, dataIndicators, isHomeTile);
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool isNationalIndicator, bool withEvolution, int digits, Type indicatorType, List<DataIndicator> dataIndicators)
        {
            return SetDataTile(property, isAverage, isNationalIndicator, withEvolution, digits, indicatorType, dataIndicators, false);
        }

        public static DataTile SetDataTile(string property, bool isAverage, bool isNationalIndicator, bool withEvolution, int digits, Type indicatorType, List<DataIndicator> dataIndicators, bool isHomeTile)
        {
            DataTile dataTile = new(dataIndicators, dataIndicators[0].IsEvolutionIndicator)
            {
                Data = EpidemicDataHelper.GetValue(property, isAverage, isNationalIndicator, digits, indicatorType),
                IndicatorType = indicatorType,
                IsAverage = isAverage,
                IsHomeTile = isHomeTile,
                LastUpdate = EpidemicDataHelper.GetLastUpdate(property, isNationalIndicator, indicatorType),
                Property = property
            };

            if (withEvolution)
                dataTile.Evolution = EpidemicDataHelper.GetEvolution(property, isAverage, isNationalIndicator, indicatorType);

            return dataTile;
        }

        public static ChartIndicators GetChartIndicators(string property, ChartType chartType, bool isAverage, bool withAverage, bool isNationalIndicator, int digits, Type indicatorType)
        {
            ChartIndicators chartIndicators = new() { Name = !withAverage ? (!isAverage ? property.GetLocalized() : (property + "Average").GetLocalized()) : "SlidingAverage".GetLocalized(), ChartType = chartType };
            chartIndicators.AddRange(EpidemicDataHelper.GetValuesForChart(property, withAverage ? true : isAverage, isNationalIndicator, digits, indicatorType));

            return chartIndicators;
        }

        public static ChartIndicators GetChartEvolutionIndicators(string property, ChartType chartType, bool withAverage, Type indicatorType)
        {
            return GetChartEvolutionIndicators(property, chartType, withAverage, false, indicatorType);
        }

        public static ChartIndicators GetChartEvolutionIndicators(string property, ChartType chartType, bool withAverage, bool isNationalIndicator, Type indicatorType)
        {
            ChartIndicators chartIndicators = new() { Name = (!withAverage ? "EvolutionTracking_EvolutionPrefix".GetLocalized() + property.GetLocalized().ToLower() : "SlidingAverage".GetLocalized()) + "EvolutionTracking_EvolutionSuffix".GetLocalized(), ChartType = chartType };
            chartIndicators.AddRange(EpidemicDataHelper.GetEvolutionForChart(property, withAverage, isNationalIndicator, indicatorType));

            return chartIndicators;
        }

        private static IEnumerable<DataTile> GetEpidemilogicalDataTiles()
        {
            List<DataTile> dataTiles = new();

            List<DataIndicator> dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsNationalIndicator = true, Property = "DailyConfirmedNewCases" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, IsNationalIndicator = true, Property = "DailyConfirmedNewCases", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("DailyConfirmedNewCases", false, true, true, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), Property = "PositiveCases" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "PositiveCases", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("PositiveCases", false, true, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), Digits = 2, Property = "IncidenceRate" }
            };

            dataTiles.Add(SetDataTile("IncidenceRate", false, true, 2, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), Digits = 2, Property = "PositivityRate" }
            };

            dataTiles.Add(SetDataTile("PositivityRate", false, true, 2, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), Digits = 2, Property = "ReproductionRate" }
            };

            dataTiles.Add(SetDataTile("ReproductionRate", false, true, 2, typeof(EpidemicIndicator), dataIndicators, true));

            return dataTiles;
        }

        private static IEnumerable<DataTile> GetHospitalDataTiles()
        {
            List<DataTile> dataTiles = new();

            List<DataIndicator> dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), Property = "NewHospitalization" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, WithAverage = true, Property = "NewHospitalization" }
            };

            DataTile dataTile = SetDataTile("NewHospitalization", false, false, typeof(EpidemicIndicator), dataIndicators);

            dataTiles.Add(dataTile);

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "HospitalizedPatients" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "IntensiveCarePatients" }
            };

            dataTiles.Add(SetDataTile("HospitalizedPatients", false, true, false, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "NewHospitalization" }
            };

            dataTiles.Add(SetDataTile("NewHospitalization", true, true, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), Property = "NewIntensiveCarePatients" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, WithAverage = true, Property = "NewIntensiveCarePatients" }
            };

            dataTiles.Add(SetDataTile("NewIntensiveCarePatients", false, false, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "HospitalizedPatients" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "IntensiveCarePatients" }
            };

            dataTiles.Add(SetDataTile("IntensiveCarePatients", false, false, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "NewIntensiveCarePatients" }
            };

            dataTiles.Add(SetDataTile("NewIntensiveCarePatients", true, true, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), Property = "NewDeceasedPersons" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "NewDeceasedPersons", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewDeceasedPersons", false, false, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), Property = "DeceasedPersons" }
            };

            dataTiles.Add(SetDataTile("DeceasedPersons", false, true, false, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "NewDeceasedPersons" }
            };

            dataTiles.Add(SetDataTile("NewDeceasedPersons", true, true, typeof(EpidemicIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), Property = "NewReturnHome" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "NewReturnHome", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewReturnHome", false, false, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "NewReturnHome", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewReturnHome", true, true, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(EpidemicIndicator), IsAverage = true, Property = "OccupationRate", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("OccupationRate", false, true, 2, typeof(EpidemicIndicator), dataIndicators));

            return dataTiles;
        }

        private static IEnumerable<DataTile> GetVaccinacinationDataTiles()
        {
            List<DataTile> dataTiles = new();

            List<DataIndicator> dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(VaccinationIndicator), Property = "NewFirstDoses" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), IsAverage = true, Property = "NewFirstDoses", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewFirstDoses", false, false, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(VaccinationIndicator), IsAverage = true, Property = "NewFirstDoses" }
            };

            dataTiles.Add(SetDataTile("NewFirstDoses", true, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalFirstDoses" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalCompleteVaccinations" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalFirstBoosterDoses" }
            };

            dataTiles.Add(SetDataTile("TotalFirstDoses", false, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(VaccinationIndicator), Property = "NewCompleteVaccinations" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), IsAverage = true, Property = "NewCompleteVaccinations", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewCompleteVaccinations", false, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(VaccinationIndicator), IsAverage = true, Property = "NewCompleteVaccinations" }
            };

            dataTiles.Add(SetDataTile("NewCompleteVaccinations", true, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalFirstDoses" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalCompleteVaccinations" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalFirstBoosterDoses" }
            };

            dataTiles.Add(SetDataTile("TotalCompleteVaccinations", true, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(VaccinationIndicator), Property = "NewFirstBoosterDoses" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), IsAverage = true, Property = "NewFirstBoosterDoses", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewFirstBoosterDoses", false, false, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Area, IndicatorType = typeof(VaccinationIndicator), IsAverage = true, Property = "NewFirstBoosterDoses" }
            };

            dataTiles.Add(SetDataTile("NewFirstBoosterDoses", true, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalFirstDoses" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalCompleteVaccinations" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "TotalFirstBoosterDoses" }
            };

            dataTiles.Add(SetDataTile("TotalFirstBoosterDoses", true, true, typeof(VaccinationIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "FirstDosesCoverage" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "CompleteVaccinationsCoverage" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "FirstBoosterDosesCoverage" }
            };

            dataTiles.Add(SetDataTile("FirstDosesCoverage", false, true, 2, typeof(VaccinationIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "FirstDosesCoverage" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "CompleteVaccinationsCoverage" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "FirstBoosterDosesCoverage" }
            };

            dataTiles.Add(SetDataTile("CompleteVaccinationsCoverage", false, true, 2, typeof(VaccinationIndicator), dataIndicators, true));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "FirstDosesCoverage" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "CompleteVaccinationsCoverage" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(VaccinationIndicator), Property = "FirstBoosterDosesCoverage" }
            };

            dataTiles.Add(SetDataTile("FirstBoosterDosesCoverage", false, true, 2, typeof(VaccinationIndicator), dataIndicators, true));

            return dataTiles;
        }

        private static IEnumerable<DataTile> GetEvolutionIndicators()
        {
            List<DataTile> dataTiles = new();

            List<DataIndicator> dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, IsNationalIndicator = true, Property = "DailyConfirmedNewCases" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsNationalIndicator = true, Property = "DailyConfirmedNewCases", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("DailyConfirmedNewCases", false, true, true, 0, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, Property = "PositiveCases" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "PositiveCases", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("PositiveCases", false, true, 2, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, Property = "NewHospitalization" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "NewHospitalization", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewHospitalization", false, true, 2, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, Property = "HospitalizedPatients" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "HospitalizedPatients", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("HospitalizedPatients", false, true, 2, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, Property = "NewIntensiveCarePatients" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "NewIntensiveCarePatients", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("NewIntensiveCarePatients", false, true, 2, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, Property = "IntensiveCarePatients" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), Property = "IntensiveCarePatients", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("IntensiveCarePatients", false, true, 2, typeof(EpidemicIndicator), dataIndicators));

            dataIndicators = new()
            {
                new() { ChartType = ChartType.Bar, IndicatorType = typeof(EpidemicIndicator), IsEvolutionIndicator = true, IsNationalIndicator = true, Property = "DeceasedPersons" },
                new() { ChartType = ChartType.Line, IndicatorType = typeof(EpidemicIndicator), IsNationalIndicator = true, Property = "DeceasedPersons", WithAverage = true }
            };

            dataTiles.Add(SetDataTile("DeceasedPersons", false, true, true, 2, typeof(EpidemicIndicator), dataIndicators));

            return dataTiles;
        }
    }
}
