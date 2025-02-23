// Copyright (c) 2017 Jan Pluskal, Miroslav Slivka
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

using System.ComponentModel.DataAnnotations.Schema;

namespace Netfox.SnooperWebmails.Models.WebmailEvents
{
    /// <summary>
    /// Object representing email message.
    /// </summary>
    [ComplexType]
    public class MailMsg
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string SourceFolder { get; set; }
        [NotMapped]
        public bool IsEmpty => string.IsNullOrEmpty(this.From) 
                               && string.IsNullOrEmpty(this.To) 
                               && string.IsNullOrEmpty(this.Cc) 
                               && string.IsNullOrEmpty(this.Bcc)
                               && string.IsNullOrEmpty(this.Subject) 
                               && string.IsNullOrEmpty(this.Body);

        public override string ToString() { return "From=[ " + this.From + "] To=[" + this.To + "] Subject=[" + this.Subject +"]"; }
    }
}