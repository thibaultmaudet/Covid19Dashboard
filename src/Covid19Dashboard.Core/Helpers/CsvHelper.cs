using Covid19Dashboard.Core.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Covid19Dashboard.Helpers
{
    public static class CsvHelper
    {
        public static async Task<T> ToObject<T>(string filePath, char separator)
        {
            List<string[]> csv = new List<string[]>();

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
                csv.Add(line.Split(separator));

            string[] properties = lines[0].Split(separator);

            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                for (int j = 0; j < properties.Length; j++)
                    result.Add(properties[j], csv[i][j]);

                results.Add(result);
            }

            dynamic dynamic = JsonConvert.SerializeObject(results);

            return await Json.ToObjectAsync<T>(dynamic);
        }
    }
}
