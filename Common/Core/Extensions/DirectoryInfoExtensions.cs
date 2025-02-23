﻿// Copyright (c) 2017 Jan Pluskal
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System.IO;
using System.IO.Abstractions;

namespace Netfox.Core.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetSubdirectory(this DirectoryInfo directoryInfo, string directory)
        {
            return new DirectoryInfo(Path.Combine(directoryInfo.FullName,directory));
        }
        public static FileInfo GetSubFileInfo(this DirectoryInfo directoryInfo, string fileName)
        {
            return new FileInfo(Path.Combine(directoryInfo.FullName, fileName));
        }

        public static DirectoryInfoBase GetSubdirectory(this DirectoryInfoBase directoryInfo, string directory)
        {
            return new FileSystem().DirectoryInfo.FromDirectoryName(Path.Combine(directoryInfo.FullName, directory));
        }
    }
}
