﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary
{
    internal abstract class AbstractPathLibrary
    {
        #region Save Locations


        protected internal static string LibrariesLocation => Locations.Libraries;

        #endregion

        #region Paths

        protected abstract List<string> Paths { get; set; }

        #endregion

        #region Load Save Synchronize

        protected void BaseSave(FileStream libraryConfigFile)
            => new XmlSerializer(Paths.GetType()).Serialize(libraryConfigFile, Paths);

        protected void BaseLoad(FileStream libraryConfigFile)
            => Paths = (List<string>) new XmlSerializer(typeof (List<string>)).Deserialize(libraryConfigFile);

        private static void EnumerateDirectory()
        {
        }

        protected void BaseSynchronize(Dictionary<string, Action<List<string>, string>> onSynchronizedFile)
        {
            foreach (var action in onSynchronizedFile)
            {
                foreach (string path in Paths)
                {
                    var localPath = new DirectoryInfo(path);
                    foreach (string filePath in Directory.GetFiles(path, action.Key, SearchOption.AllDirectories).AsEnumerable())
                    {
                        var fileDirectoryPath = new FileInfo(filePath).Directory?.FullName;
                        var dividedPath = fileDirectoryPath?.Substring(localPath.FullName.Length)
                            .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
                        dividedPath[0] = localPath.Name;
                        onSynchronizedFile[action.Key](dividedPath, filePath);
                    }
                }
            }
        }

        #endregion
    }
}