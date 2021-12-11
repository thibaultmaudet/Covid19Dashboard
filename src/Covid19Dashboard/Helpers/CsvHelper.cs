using Covid19Dashboard.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Covid19Dashboard.Helpers
{
    public static class CsvHelper
    {
        public static async Task<T> ToObject<T>(StorageFile storageFile)
        {
            List<string[]> csv = new List<string[]>();

            IList<string> lines = await FileIO.ReadLinesAsync(storageFile);

            foreach (string line in lines)
                csv.Add(line.Split(','));

            string[] properties = lines[0].Split(',');

            List<Dictionary<string, string>> listObjResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Count; i++)
            {
                Dictionary<string, string> objResult = new Dictionary<string, string>();

                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }

            dynamic dynamic = JsonConvert.SerializeObject(listObjResult);

            return await Json.ToObjectAsync<T>(dynamic);
        }
    }
}
