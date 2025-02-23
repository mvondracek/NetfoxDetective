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

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Netfox.Core.Enums;

namespace Netfox.Framework.Models
{
    public class L7ConversationStatistics :  L4ConversationStatistics
    {
        private long? _extractedBytes;
        private long? _missingBytes;
        private long? _missingFrames;

        internal L7ConversationStatistics(L7ConversationStatistics upFlowStatistic, L7ConversationStatistics downFlowStatistic):base(upFlowStatistic.L7Conversation)
        {
            base.UPFlowStatistic = upFlowStatistic;
            base.DownFlowStatistic = downFlowStatistic;
            this.L7Conversation = upFlowStatistic.L7Conversation;
            this.L7ConversationRefId = upFlowStatistic.L7ConversationRefId;
        }
        
        public L7ConversationStatistics(FsUnidirectionalFlow flow, L7Conversation l7conversation, DaRFlowDirection flowDirection):base(flowDirection, l7conversation)
        {
            this.L7Conversation = l7conversation;
            this.L7ConversationRefId = l7conversation.Id;
            
            if(flow==null) return;
            
            foreach(var frame in flow.RealFrames)
            {
                base.ProcessFrame(frame);
                this.ExtractedBytes += frame.L7PayloadLength; 
            }
            foreach (var frame in flow.VirtualFrames)
            {
                base.ProcessFrame(frame);
                this.MissingFrames++;
                this.MissingBytes += frame.L7PayloadLength;
            }
        }
        protected L7ConversationStatistics() {  }

        public Guid? L7ConversationRefId { get; set; }

        [ForeignKey(nameof(L7ConversationRefId))]
        public virtual L7Conversation L7Conversation { get; set; }

        public Int64 ExtractedBytes
        {
            get { return (long) (this._extractedBytes ?? (this._extractedBytes = this.UPFlowStatistic?.ExtractedBytes + this.DownFlowStatistic?.ExtractedBytes ?? 0)); }
            set { this._extractedBytes = value; }
        }

        public Int64 MissingBytes
        {
            get { return (long)(this._missingBytes ?? (this._missingBytes = this.UPFlowStatistic?.MissingBytes + this.DownFlowStatistic?.MissingBytes ??0)); }
            set { this._missingBytes = value; }
        }

        public Int64 MissingFrames
        {
            get { return (long)(this._missingFrames ?? (this._missingFrames = this.UPFlowStatistic?.MissingFrames + this.DownFlowStatistic?.MissingFrames??0)); }
            set { this._missingFrames = value; }
        }

        #region Overrides of ConversationStatisticsBase
        public new L7ConversationStatistics DownFlowStatistic => base.DownFlowStatistic as L7ConversationStatistics;
        public new L7ConversationStatistics UPFlowStatistic => base.UPFlowStatistic as L7ConversationStatistics;
        #endregion
    }
}