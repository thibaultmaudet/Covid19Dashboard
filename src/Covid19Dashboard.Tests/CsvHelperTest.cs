using System.Collections.Generic;
using System.Threading.Tasks;

using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Covid19Dashboard.Tests
{
    [TestClass]
    public class CsvHelperTest
    {
        [TestMethod]
        [DeploymentItem("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", "Assets")]
        public async Task GetEpidemicIndicators()
        {
            Assert.IsNotNull(await CsvHelper.ToObject<List<EpidemicIndicator>>("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", ','));
        }
    }
}
