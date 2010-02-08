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
 *  Marshaler code for OpenWire format for JournalTopicAck
 *
 *  NOTE!: This file is auto generated - do not modify!
 *         if you need to make a change, please see the Java Classes
 *         in the nms-activemq-openwire-generator module
 *
 */

using System;
using System.Collections;
using System.IO;

using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.ActiveMQ.OpenWire;
using Apache.NMS.ActiveMQ.OpenWire.V5;

namespace Apache.NMS.ActiveMQ.OpenWire.V5
{
    /// <summary>
    ///  Marshalling code for Open Wire Format for JournalTopicAck
    /// </summary>
    class JournalTopicAckMarshaller : BaseDataStreamMarshaller
    {
        /// <summery>
        ///  Creates an instance of the Object that this marshaller handles.
        /// </summery>
        public override DataStructure CreateObject() 
        {
            return new JournalTopicAck();
        }

        /// <summery>
        ///  Returns the type code for the Object that this Marshaller handles..
        /// </summery>
        public override byte GetDataStructureType() 
        {
            return JournalTopicAck.ID_JOURNALTOPICACK;
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void TightUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn, BooleanStream bs) 
        {
            base.TightUnmarshal(wireFormat, o, dataIn, bs);

            JournalTopicAck info = (JournalTopicAck)o;
            info.Destination = (ActiveMQDestination) TightUnmarshalNestedObject(wireFormat, dataIn, bs);
            info.MessageId = (MessageId) TightUnmarshalNestedObject(wireFormat, dataIn, bs);
            info.MessageSequenceId = TightUnmarshalLong(wireFormat, dataIn, bs);
            info.SubscritionName = TightUnmarshalString(dataIn, bs);
            info.ClientId = TightUnmarshalString(dataIn, bs);
            info.TransactionId = (TransactionId) TightUnmarshalNestedObject(wireFormat, dataIn, bs);
        }

        //
        // Write the booleans that this object uses to a BooleanStream
        //
        public override int TightMarshal1(OpenWireFormat wireFormat, Object o, BooleanStream bs)
        {
            JournalTopicAck info = (JournalTopicAck)o;

            int rc = base.TightMarshal1(wireFormat, o, bs);
        rc += TightMarshalNestedObject1(wireFormat, (DataStructure)info.Destination, bs);
        rc += TightMarshalNestedObject1(wireFormat, (DataStructure)info.MessageId, bs);
            rc += TightMarshalLong1(wireFormat, info.MessageSequenceId, bs);
            rc += TightMarshalString1(info.SubscritionName, bs);
            rc += TightMarshalString1(info.ClientId, bs);
        rc += TightMarshalNestedObject1(wireFormat, (DataStructure)info.TransactionId, bs);

            return rc + 0;
        }

        // 
        // Write a object instance to data output stream
        //
        public override void TightMarshal2(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut, BooleanStream bs)
        {
            base.TightMarshal2(wireFormat, o, dataOut, bs);

            JournalTopicAck info = (JournalTopicAck)o;
            TightMarshalNestedObject2(wireFormat, (DataStructure)info.Destination, dataOut, bs);
            TightMarshalNestedObject2(wireFormat, (DataStructure)info.MessageId, dataOut, bs);
            TightMarshalLong2(wireFormat, info.MessageSequenceId, dataOut, bs);
            TightMarshalString2(info.SubscritionName, dataOut, bs);
            TightMarshalString2(info.ClientId, dataOut, bs);
            TightMarshalNestedObject2(wireFormat, (DataStructure)info.TransactionId, dataOut, bs);
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void LooseUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn) 
        {
            base.LooseUnmarshal(wireFormat, o, dataIn);

            JournalTopicAck info = (JournalTopicAck)o;
            info.Destination = (ActiveMQDestination) LooseUnmarshalNestedObject(wireFormat, dataIn);
            info.MessageId = (MessageId) LooseUnmarshalNestedObject(wireFormat, dataIn);
            info.MessageSequenceId = LooseUnmarshalLong(wireFormat, dataIn);
            info.SubscritionName = LooseUnmarshalString(dataIn);
            info.ClientId = LooseUnmarshalString(dataIn);
            info.TransactionId = (TransactionId) LooseUnmarshalNestedObject(wireFormat, dataIn);
        }

        // 
        // Write a object instance to data output stream
        //
        public override void LooseMarshal(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut)
        {

            JournalTopicAck info = (JournalTopicAck)o;

            base.LooseMarshal(wireFormat, o, dataOut);
            LooseMarshalNestedObject(wireFormat, (DataStructure)info.Destination, dataOut);
            LooseMarshalNestedObject(wireFormat, (DataStructure)info.MessageId, dataOut);
            LooseMarshalLong(wireFormat, info.MessageSequenceId, dataOut);
            LooseMarshalString(info.SubscritionName, dataOut);
            LooseMarshalString(info.ClientId, dataOut);
            LooseMarshalNestedObject(wireFormat, (DataStructure)info.TransactionId, dataOut);
        }
    }
}
