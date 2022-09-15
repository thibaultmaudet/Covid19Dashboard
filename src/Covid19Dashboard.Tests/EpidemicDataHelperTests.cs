using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Covid19Dashboard.Tests
{
    [TestClass]
    public class EpidemicDataHelperTests
    {
        public Data Data => Data.Instance;

        [TestInitialize]
        public async Task Initialize()
        {
            Data.EpidemicIndicators = null;
            Data.VaccinationIndicators = null;

            Common.EpidemicIndicators = await Common.GetEpidemicIndicatorsAsync();
            Common.VaccinationIndicators = await Common.GetVaccinationIndicatorsAsync();
        }

        [TestMethod]
        [DeploymentItem("Assets\\4ecd4b41-333f-4bb2-ad0b-f757ba61b440.csv", "Assets")]
        [DeploymentItem("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", "Assets")]
        public void GetEvolutionForSpecifiedField()
        {
            Assert.AreEqual(default, EpidemicDataHelper.GetEvolution("DailyConfirmedNewCases", false, true, typeof(EpidemicIndicator)));

            Assert.AreEqual(default, EpidemicDataHelper.GetEvolution("DailyConfirmedNewCases", false, true, new DateTime(2022, 08, 26), typeof(EpidemicIndicator)));

            Assert.AreEqual(default, EpidemicDataHelper.GetEvolution("NewCompleteVaccinations", false, true, typeof(VaccinationIndicator)));

            Data.EpidemicIndicators = Common.EpidemicIndicators;

            Data.VaccinationIndicators = Common.VaccinationIndicators;

            Assert.AreEqual(-5.13, EpidemicDataHelper.GetEvolution("DailyConfirmedNewCases", false, true,
                typeof(EpidemicIndicator)));

            Assert.AreEqual(-37.64, EpidemicDataHelper.GetEvolution("IncidenceRate", false, true, new DateTime(2022, 08, 15), typeof(EpidemicIndicator)));

            Assert.AreEqual(-0.83, EpidemicDataHelper.GetEvolution("NewCompleteVaccinations", false, false, typeof(VaccinationIndicator)));

            Assert.AreEqual(-81.12, EpidemicDataHelper.GetEvolution("NewCompleteVaccinations", false, false, new DateTime(2022, 08, 15), typeof(VaccinationIndicator)));
        }

        [TestMethod]
        [DeploymentItem("Assets\\4ecd4b41-333f-4bb2-ad0b-f757ba61b440.csv", "Assets")]
        [DeploymentItem("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", "Assets")]
        public void GetEvolutionForChartForsSpecifiedField()
        {
            Assert.AreEqual(default, EpidemicDataHelper.GetEvolutionForChart("DailyConfirmedNewCases", false, true, typeof(EpidemicIndicator)));

            Assert.AreEqual(default, EpidemicDataHelper.GetEvolutionForChart("NewCompleteVaccinations", false, true, typeof(VaccinationIndicator)));

            Data.EpidemicIndicators = Common.EpidemicIndicators;

            Data.VaccinationIndicators = Common.VaccinationIndicators;

            List<ChartIndicator> expectedDailyConfirmedNewCasesChartIndicators = new()
            {
                new ChartIndicator() { Date = new DateTime(2022, 08, 29), Value = -5.13 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 28), Value = -4.79 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 27), Value = -10.25 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 26), Value = -10.48 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 25), Value = -15.96 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 24), Value = -40.44 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 23), Value = 357.87 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 22), Value = -12.09 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 21), Value = -9.34 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 20), Value = -6.16 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 19), Value = -10.61 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 18), Value = -2.91 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 17), Value = 31.99 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 16), Value = -83.44 }
            };

            List<ChartIndicator> expectedNewCompleteVaccinationsChartIndicators = new()
            {
                new ChartIndicator() { Date = new DateTime(2022, 08, 29), Value = -0.83 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 28), Value = 16.13 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 27), Value = -11.26},
                new ChartIndicator() { Date = new DateTime(2022, 08, 26), Value = 9.94 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 25), Value = -8.84 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 24), Value = -1.07 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 23), Value = 3.59},
                new ChartIndicator() { Date = new DateTime(2022, 08, 22), Value = 495.04 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 21), Value = -7.92 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 20), Value = 26.44 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 19), Value = 3.64 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 18), Value = 6.06 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 17), Value = -2.73 },
                new ChartIndicator() { Date = new DateTime(2022, 08, 16), Value = -2.38 }
            };

            List<ChartIndicator> actualDailyConfirmedNewCasesChartIndicators = EpidemicDataHelper.GetEvolutionForChart("DailyConfirmedNewCases", false, true, typeof(EpidemicIndicator)).OrderByDescending(x => x.Date).Take(14).ToList();

            Assert.IsNotNull(actualDailyConfirmedNewCasesChartIndicators);

            Assert.AreEqual(expectedDailyConfirmedNewCasesChartIndicators.Count, actualDailyConfirmedNewCasesChartIndicators.Count);

            Assert.IsTrue(expectedDailyConfirmedNewCasesChartIndicators.SequenceEqual(actualDailyConfirmedNewCasesChartIndicators, new ChartIndicatorComparer()));

            List<ChartIndicator> actualNewCompleteVaccinationsChartIndicators = EpidemicDataHelper.GetEvolutionForChart("NewCompleteVaccinations", false, true, typeof(VaccinationIndicator)).OrderByDescending(x => x.Date).Take(14).ToList();

            Assert.IsNotNull(actualNewCompleteVaccinationsChartIndicators);

            Assert.AreEqual(expectedNewCompleteVaccinationsChartIndicators.Count, actualNewCompleteVaccinationsChartIndicators.Count);

            Assert.IsTrue(expectedNewCompleteVaccinationsChartIndicators.SequenceEqual(actualNewCompleteVaccinationsChartIndicators, new ChartIndicatorComparer()));
        }

        [TestMethod]
        [DeploymentItem("Assets\\4ecd4b41-333f-4bb2-ad0b-f757ba61b440.csv", "Assets")]
        [DeploymentItem("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", "Assets")]
        public void GetFirstDateAvailableForSpecifiedField()
        {
            Assert.AreEqual(default, EpidemicDataHelper.GetLastUpdate("DailyConfirmedNewCases", true, typeof(EpidemicIndicator)));

            Assert.AreEqual(default, EpidemicDataHelper.GetEvolutionForChart("NewCompleteVaccinations", false, true, typeof(VaccinationIndicator)));

            Data.EpidemicIndicators = Common.EpidemicIndicators;

            Data.VaccinationIndicators = Common.VaccinationIndicators;

            Assert.AreEqual(new DateTime(2022, 08, 29).ToShortDateString(), EpidemicDataHelper.GetLastUpdate("DailyConfirmedNewCases", true, typeof(EpidemicIndicator)));

            Assert.AreEqual(new DateTime(2022, 08, 26).ToShortDateString(), EpidemicDataHelper.GetLastUpdate("IncidenceRate", false, typeof(EpidemicIndicator)));

            Assert.AreEqual(new DateTime(2022, 08, 29).ToShortDateString(), EpidemicDataHelper.GetLastUpdate("NewCompleteVaccinations", true, typeof(VaccinationIndicator)));
        }

        [TestMethod]
        [DeploymentItem("Assets\\4ecd4b41-333f-4bb2-ad0b-f757ba61b440.csv", "Assets")]
        [DeploymentItem("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", "Assets")]
        public void GetFirstValueAvailableForSpecifiedField()
        {
            Assert.AreEqual(default, EpidemicDataHelper.GetValue("DailyConfirmedNewCases", false, true, 0, typeof(EpidemicIndicator)));

            Assert.AreEqual(default, EpidemicDataHelper.GetEvolutionForChart("NewCompleteVaccinations", false, true, typeof(VaccinationIndicator)));

            Data.EpidemicIndicators = Common.EpidemicIndicators;

            Data.VaccinationIndicators = Common.VaccinationIndicators;

            Assert.AreEqual("3806", EpidemicDataHelper.GetValue("DailyConfirmedNewCases", false, true, 0, typeof(EpidemicIndicator)));

            float.TryParse("182,39", System.Globalization.NumberStyles.AllowDecimalPoint, null, out float expected);

            Assert.AreEqual(expected.ToString(), EpidemicDataHelper.GetValue("IncidenceRate", false, false, 2, typeof(EpidemicIndicator)));

            Assert.AreEqual("714", EpidemicDataHelper.GetValue("NewCompleteVaccinations", false, false, 0, typeof(VaccinationIndicator)));
        }
    }
}
