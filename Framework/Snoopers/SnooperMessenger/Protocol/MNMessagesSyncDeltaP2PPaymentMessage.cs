// Copyright (c) 2017 Jan Pluskal, Viliam Letavay
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

/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Text;
using Thrift.Protocol;

namespace Netfox.SnooperMessenger.Protocol
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class MNMessagesSyncDeltaP2PPaymentMessage : TBase
  {
    private MNMessagesSyncMessageMetadata _MessageMetadata;
    private long _TransferId;
    private int _MessageType;

    public MNMessagesSyncMessageMetadata MessageMetadata
    {
      get
      {
        return _MessageMetadata;
      }
      set
      {
        __isset.MessageMetadata = true;
        this._MessageMetadata = value;
      }
    }

    public long TransferId
    {
      get
      {
        return _TransferId;
      }
      set
      {
        __isset.TransferId = true;
        this._TransferId = value;
      }
    }

    public int MessageType
    {
      get
      {
        return _MessageType;
      }
      set
      {
        __isset.MessageType = true;
        this._MessageType = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool MessageMetadata;
      public bool TransferId;
      public bool MessageType;
    }

    public MNMessagesSyncDeltaP2PPaymentMessage() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                MessageMetadata = new MNMessagesSyncMessageMetadata();
                MessageMetadata.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I64) {
                TransferId = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I32) {
                MessageType = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("MNMessagesSyncDeltaP2PPaymentMessage");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (MessageMetadata != null && __isset.MessageMetadata) {
          field.Name = "MessageMetadata";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          MessageMetadata.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (__isset.TransferId) {
          field.Name = "TransferId";
          field.Type = TType.I64;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(TransferId);
          oprot.WriteFieldEnd();
        }
        if (__isset.MessageType) {
          field.Name = "MessageType";
          field.Type = TType.I32;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(MessageType);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaP2PPaymentMessage(");
      bool __first = true;
      if (MessageMetadata != null && __isset.MessageMetadata) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MessageMetadata: ");
        __sb.Append(MessageMetadata== null ? "<null>" : MessageMetadata.ToString());
      }
      if (__isset.TransferId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("TransferId: ");
        __sb.Append(TransferId);
      }
      if (__isset.MessageType) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MessageType: ");
        __sb.Append(MessageType);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
