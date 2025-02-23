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
using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Netfox.SnooperMessenger.Protocol
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class MNMessagesSyncDeltaBroadcastMessage : TBase
  {
    private List<MNMessagesSyncMessageMetadata> _MessageMetadatas;
    private string _Body;
    private long _StickerId;
    private List<MNMessagesSyncAttachment> _Attachments;

    public List<MNMessagesSyncMessageMetadata> MessageMetadatas
    {
      get
      {
        return _MessageMetadatas;
      }
      set
      {
        __isset.MessageMetadatas = true;
        this._MessageMetadatas = value;
      }
    }

    public string Body
    {
      get
      {
        return _Body;
      }
      set
      {
        __isset.Body = true;
        this._Body = value;
      }
    }

    public long StickerId
    {
      get
      {
        return _StickerId;
      }
      set
      {
        __isset.StickerId = true;
        this._StickerId = value;
      }
    }

    public List<MNMessagesSyncAttachment> Attachments
    {
      get
      {
        return _Attachments;
      }
      set
      {
        __isset.Attachments = true;
        this._Attachments = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool MessageMetadatas;
      public bool Body;
      public bool StickerId;
      public bool Attachments;
    }

    public MNMessagesSyncDeltaBroadcastMessage() {
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
              if (field.Type == TType.List) {
                {
                  MessageMetadatas = new List<MNMessagesSyncMessageMetadata>();
                  TList _list87 = iprot.ReadListBegin();
                  for( int _i88 = 0; _i88 < _list87.Count; ++_i88)
                  {
                    MNMessagesSyncMessageMetadata _elem89;
                    _elem89 = new MNMessagesSyncMessageMetadata();
                    _elem89.Read(iprot);
                    MessageMetadatas.Add(_elem89);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Body = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I64) {
                StickerId = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.List) {
                {
                  Attachments = new List<MNMessagesSyncAttachment>();
                  TList _list90 = iprot.ReadListBegin();
                  for( int _i91 = 0; _i91 < _list90.Count; ++_i91)
                  {
                    MNMessagesSyncAttachment _elem92;
                    _elem92 = new MNMessagesSyncAttachment();
                    _elem92.Read(iprot);
                    Attachments.Add(_elem92);
                  }
                  iprot.ReadListEnd();
                }
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
        TStruct struc = new TStruct("MNMessagesSyncDeltaBroadcastMessage");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (MessageMetadatas != null && __isset.MessageMetadatas) {
          field.Name = "MessageMetadatas";
          field.Type = TType.List;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, MessageMetadatas.Count));
            foreach (MNMessagesSyncMessageMetadata _iter93 in MessageMetadatas)
            {
              _iter93.Write(oprot);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (Body != null && __isset.Body) {
          field.Name = "Body";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Body);
          oprot.WriteFieldEnd();
        }
        if (__isset.StickerId) {
          field.Name = "StickerId";
          field.Type = TType.I64;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(StickerId);
          oprot.WriteFieldEnd();
        }
        if (Attachments != null && __isset.Attachments) {
          field.Name = "Attachments";
          field.Type = TType.List;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.Struct, Attachments.Count));
            foreach (MNMessagesSyncAttachment _iter94 in Attachments)
            {
              _iter94.Write(oprot);
            }
            oprot.WriteListEnd();
          }
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
      StringBuilder __sb = new StringBuilder("MNMessagesSyncDeltaBroadcastMessage(");
      bool __first = true;
      if (MessageMetadatas != null && __isset.MessageMetadatas) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MessageMetadatas: ");
        __sb.Append(MessageMetadatas);
      }
      if (Body != null && __isset.Body) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Body: ");
        __sb.Append(Body);
      }
      if (__isset.StickerId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("StickerId: ");
        __sb.Append(StickerId);
      }
      if (Attachments != null && __isset.Attachments) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Attachments: ");
        __sb.Append(Attachments);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
