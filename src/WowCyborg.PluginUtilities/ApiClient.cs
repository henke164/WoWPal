﻿using System;
using System.Collections.Generic;
using System.Net;
using WowCyborg.PluginUtilities.Models;

namespace WowCyborg.PluginUtilities
{
    public class ApiClient
    {
        private static string _baseUrl;

        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public ServerTextFile GetFile(string fileName)
            => new ServerTextFile
            {
                FileName = fileName,
                Content = DownloadString($"/file/?file={fileName}")
            };

        public IEnumerable<ServerTextFile> GetAddonFiles()
        {
            var str = DownloadString("/map");
            var fileNames = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var fileName in fileNames)
            {
                yield return GetFile(fileName);
            }
        }

        public Dictionary<string, string> GetFilesMap(string path)
        {
            var rotationDictionary = new Dictionary<string, string>();
            var str = DownloadString($"/{path}");
            var rows = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var row in rows)
            {
                var columns = row.Split('\t');
                rotationDictionary.Add(columns[0], columns[1]);
            }
            return rotationDictionary;
        }   

        protected string DownloadString(string path)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString($"{_baseUrl}{path}");
            }
        }
    }
}
