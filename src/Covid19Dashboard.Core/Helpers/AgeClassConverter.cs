namespace Covid19Dashboard.Core.Helpers
{
    public class AgeClassConverter
    {
        public static string GetAgeLabelForVaccinationIndicator(int? ageClass)
        {
            switch (ageClass)
            {
                case 0:
                    return "Tous âges";
                case 4:
                    return "0-4 ans";
                case 9:
                    return "5-9 ans";
                case 11:
                    return "10-11 ans";
                case 17:
                    return "12-17 ans";
                case 24:
                    return "18-24 ans";
                case 29:
                    return "25-29 ans";
                case 39:
                    return "30-39 ans";
                case 49:
                    return "40-49 ans";
                case 59:
                    return "50-59 ans";
                case 64:
                    return "60-64 ans";
                case 69:
                    return "65-69 ans";
                case 74:
                    return "70-74 ans";
                case 79:
                    return "75-79 ans";
                case 80:
                    return "80 ans et plus";
                default:
                    return default;
            }
        }
    }
}
