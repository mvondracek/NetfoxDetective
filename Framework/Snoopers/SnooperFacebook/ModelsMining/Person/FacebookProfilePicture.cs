﻿// Copyright (c) 2017 Jan Pluskal, Tomas Bruckner
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

namespace Netfox.SnooperFacebook.ModelsMining.Person
{
    /// <summary>
    /// Url to user's profile picture which is always public.
    /// </summary>
    public class FacebookProfilePicture
    {
        public ulong UserId { get; set; }
        public string PictureUrl { get; set; }
    }
}
