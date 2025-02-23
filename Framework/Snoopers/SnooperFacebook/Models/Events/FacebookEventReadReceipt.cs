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

using Netfox.Framework.Models.Snoopers;

namespace Netfox.SnooperFacebook.Models.Events
{
    /// <summary>
    /// Metadata message. Happens when other user reads  your facebook personal message.
    /// </summary>
	public class FacebookEventReadReceipt : FacebookEventBase
	{
        private FacebookEventReadReceipt() : base() { } //EF
        public FacebookEventReadReceipt(SnooperExportBase exportBase) : base(exportBase) { }
		public ulong ReaderId { get; set; }
		public ulong ThreadId { get; set; }
		public ulong Timestamp { get; set; }
	}
}
