/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*
 *
 *  Marshaler code for OpenWire format for MessageAck
 *
 *  NOTE!: This file is auto generated - do not modify!
 *         if you need to make a change, please see the Java Classes
 *         in the nms-activemq-openwire-generator module
 *
 */

using System;
using System.IO;

using Apache.NMS.ActiveMQ.Commands;

namespace Apache.NMS.ActiveMQ.OpenWire.V4
{
    /// <summary>
    ///  Marshalling code for Open Wire Format for MessageAck
    /// </summary>
    class MessageAckMarshaller : BaseCommandMarshaller
    {
        /// <summery>
        ///  Creates an instance of the Object that this marshaller handles.
        /// </summery>
        public override DataStructure CreateObject() 
        {
            return new MessageAck();
        }

        /// <summery>
        ///  Returns the type code for the Object that this Marshaller handles..
        /// </summery>
        public override byte GetDataStructureType() 
        {
            return MessageAck.ID_MESSAGEACK;
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void TightUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn, BooleanStream bs) 
        {
            base.TightUnmarshal(wireFormat, o, dataIn, bs);

            MessageAck info = (MessageAck)o;
            info.Destination = (ActiveMQDestination) TightUnmarshalCachedObject(wireFormat, dataIn, bs);
            info.TransactionId = (TransactionId) TightUnmarshalCachedObject(wireFormat, dataIn, bs);
            info.ConsumerId = (ConsumerId) TightUnmarshalCachedObject(wireFormat, dataIn, bs);
            info.AckType = dataIn.ReadByte();
            info.FirstMessageId = (MessageId) TightUnmarshalNestedObject(wireFormat, dataIn, bs);
            info.LastMessageId = (MessageId) TightUnmarshalNestedObject(wireFormat, dataIn, bs);
            info.MessageCount = dataIn.ReadInt32();
        }

        //
        // Write the booleans that this object uses to a BooleanStream
        //
        public override int TightMarshal1(OpenWireFormat wireFormat, Object o, BooleanStream bs)
        {
            MessageAck info = (MessageAck)o;

            int rc = base.TightMarshal1(wireFormat, o, bs);
            rc += TightMarshalCachedObject1(wireFormat, (DataStructure)info.Destination, bs);
            rc += TightMarshalCachedObject1(wireFormat, (DataStructure)info.TransactionId, bs);
            rc += TightMarshalCachedObject1(wireFormat, (DataStructure)info.ConsumerId, bs);
        rc += TightMarshalNestedObject1(wireFormat, (DataStructure)info.FirstMessageId, bs);
        rc += TightMarshalNestedObject1(wireFormat, (DataStructure)info.LastMessageId, bs);

            return rc + 5;
        }

        // 
        // Write a object instance to data output stream
        //
        public override void TightMarshal2(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut, BooleanStream bs)
        {
            base.TightMarshal2(wireFormat, o, dataOut, bs);

            MessageAck info = (MessageAck)o;
            TightMarshalCachedObject2(wireFormat, (DataStructure)info.Destination, dataOut, bs);
            TightMarshalCachedObject2(wireFormat, (DataStructure)info.TransactionId, dataOut, bs);
            TightMarshalCachedObject2(wireFormat, (DataStructure)info.ConsumerId, dataOut, bs);
            dataOut.Write(info.AckType);
            TightMarshalNestedObject2(wireFormat, (DataStructure)info.FirstMessageId, dataOut, bs);
            TightMarshalNestedObject2(wireFormat, (DataStructure)info.LastMessageId, dataOut, bs);
            dataOut.Write(info.MessageCount);
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void LooseUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn) 
        {
            base.LooseUnmarshal(wireFormat, o, dataIn);

            MessageAck info = (MessageAck)o;
            info.Destination = (ActiveMQDestination) LooseUnmarshalCachedObject(wireFormat, dataIn);
            info.TransactionId = (TransactionId) LooseUnmarshalCachedObject(wireFormat, dataIn);
            info.ConsumerId = (ConsumerId) LooseUnmarshalCachedObject(wireFormat, dataIn);
            info.AckType = dataIn.ReadByte();
            info.FirstMessageId = (MessageId) LooseUnmarshalNestedObject(wireFormat, dataIn);
            info.LastMessageId = (MessageId) LooseUnmarshalNestedObject(wireFormat, dataIn);
            info.MessageCount = dataIn.ReadInt32();
        }

        // 
        // Write a object instance to data output stream
        //
        public override void LooseMarshal(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut)
        {

            MessageAck info = (MessageAck)o;

            base.LooseMarshal(wireFormat, o, dataOut);
            LooseMarshalCachedObject(wireFormat, (DataStructure)info.Destination, dataOut);
            LooseMarshalCachedObject(wireFormat, (DataStructure)info.TransactionId, dataOut);
            LooseMarshalCachedObject(wireFormat, (DataStructure)info.ConsumerId, dataOut);
            dataOut.Write(info.AckType);
            LooseMarshalNestedObject(wireFormat, (DataStructure)info.FirstMessageId, dataOut);
            LooseMarshalNestedObject(wireFormat, (DataStructure)info.LastMessageId, dataOut);
            dataOut.Write(info.MessageCount);
        }
    }
}
