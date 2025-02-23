/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *
 * Contains some contributions under the Thrift Software License.
 * Please see doc/old-thrift-license.txt in the Thrift distribution for
 * details.
 */

namespace Thrift.Transport
{
    public class TTransportException : TException
    {
        protected ExceptionType type;

        public TTransportException()
            : base()
        {
        }

        public TTransportException(ExceptionType type)
            : this()
        {
            this.type = type;
        }

        public TTransportException(ExceptionType type, string message)
            : base(message)
        {
            this.type = type;
        }

        public TTransportException(string message)
            : base(message)
        {
        }

        public ExceptionType Type
        {
            get { return type; }
        }

        public enum ExceptionType
        {
            Unknown,
            NotOpen,
            AlreadyOpen,
            TimedOut,
            EndOfFile,
            Interrupted
        }
    }
}
