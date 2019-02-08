using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace HWService
{
    class JsonDeserialization
    {

        public string jsonPathText = @"C:\JsonFiles\PostTexts.json";
        public string jsonPathLinks = @"C:\JsonFiles\PostLinks.json";
        public string jsonPathImgLinks = @"C:\JsonFiles\PostImgLinks.json";

        public string lockPathText = @"C:\JsonFiles\PostTexts.lock";
        public string lockPathLinks = @"C:\JsonFiles\PostLinks.lock";
        public string lockPathImgLinks = @"C:\JsonFiles\PostImgLinks.lock";

        public Dictionary<string, List<string>> dictionaryOfText;
        public Dictionary<string, List<string>> dictionaryOfLinks;
        public Dictionary<string, List<string>> dictionaryOfLinksImg;

        public Dictionary<string, List<string>> textFileJson()
        {
            while (true)
            {
                Thread.Sleep(50);

                if (File.Exists(lockPathText))
                    continue;

                dictionaryOfText = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(jsonPathText));
                EventLog.WriteEntry("HWService", $"Deserialized {dictionaryOfText.Count} elements", EventLogEntryType.Information);
                return dictionaryOfText;
            }
        }

        public Dictionary<string, List<string>> linksFileJson()
        {
            while (true)
            {
                Thread.Sleep(50);

                if (File.Exists(lockPathLinks))
                    continue;

                dictionaryOfLinks = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(jsonPathLinks));

                EventLog.WriteEntry("HWService", $"Deserialized {dictionaryOfLinks.Count} elements", EventLogEntryType.Information);
                return dictionaryOfLinks;
            }
        }

        public Dictionary<string, List<string>> linkImgFileJson()
        {
            while (true)
            {
                Thread.Sleep(50);

                if (File.Exists(lockPathImgLinks))
                    continue;

                dictionaryOfLinksImg = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(jsonPathImgLinks));

                EventLog.WriteEntry("HWService", $"Deserialized {dictionaryOfLinksImg.Count} elements", EventLogEntryType.Information);
                return dictionaryOfLinksImg;
            }
        }
    }
}
