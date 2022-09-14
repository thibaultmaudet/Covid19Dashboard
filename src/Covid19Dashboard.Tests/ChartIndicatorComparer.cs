using System.Collections.Generic;

using Covid19Dashboard.Core.Models;

namespace Covid19Dashboard.Tests
{
    public class ChartIndicatorComparer : IEqualityComparer<ChartIndicator>
    {
        public bool Equals(ChartIndicator x, ChartIndicator y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.Date.Equals(y.Date) && x.Value.Equals(y.Value);
        }

        public int GetHashCode(ChartIndicator obj)
        {
            if (ReferenceEquals(obj, null)) return 0;

            int hashCodeName = obj.Date == null ? 0 : obj.Date.GetHashCode();
            int hasCodeAge = obj.Value.GetHashCode();

            return hashCodeName ^ hasCodeAge;
        }
    }
}
