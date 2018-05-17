using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public static class ConfigJson
    {
        /// <summary>
        /// The function serialzing the app config to string
        /// </summary>
        /// <returns></returns>
        public static string ConvertConfigToString()
        {
            AppParsing appPar = AppParsing.Instance;
            JObject jsonAppConfig = new JObject
            {
                ["Source Name"] = appPar.SourceName,
                ["Log Name"] = appPar.LogName,
                ["OutputDir"] = appPar.OutputDir,
                ["Thumbnail Size"] = appPar.ThubnailSized,
                ["Directories"] = JsonConvert.SerializeObject(appPar.PathHandlers),
            };
            return jsonAppConfig.ToString();
        }
    }
}