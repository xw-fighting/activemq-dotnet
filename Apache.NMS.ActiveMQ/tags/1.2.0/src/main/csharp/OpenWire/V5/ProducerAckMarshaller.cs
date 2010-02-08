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
 *  Marshaler code for OpenWire format for ProducerAck
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
    ///  Marshalling code for Open Wire Format for ProducerAck
    /// </summary>
    class ProducerAckMarshaller : BaseCommandMarshaller
    {
        /// <summery>
        ///  Creates an instance of the Object that this marshaller handles.
        /// </summery>
        public override DataStructure CreateObject() 
        {
            return new ProducerAck();
        }

        /// <summery>
        ///  Returns the type code for the Object that this Marshaller handles..
        /// </summery>
        public override byte GetDataStructureType() 
        {
            return ProducerAck.ID_PRODUCERACK;
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void TightUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn, BooleanStream bs) 
        {
            base.TightUnmarshal(wireFormat, o, dataIn, bs);

            ProducerAck info = (ProducerAck)o;
            info.ProducerId = (ProducerId) TightUnmarshalNestedObject(wireFormat, dataIn, bs);
            info.Size = dataIn.ReadInt32();
        }

        //
        // Write the booleans that this object uses to a BooleanStream
        //
        public override int TightMarshal1(OpenWireFormat wireFormat, Object o, BooleanStream bs)
        {
            ProducerAck info = (ProducerAck)o;

            int rc = base.TightMarshal1(wireFormat, o, bs);
        rc += TightMarshalNestedObject1(wireFormat, (DataStructure)info.ProducerId, bs);

            return rc + 4;
        }

        // 
        // Write a object instance to data output stream
        //
        public override void TightMarshal2(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut, BooleanStream bs)
        {
            base.TightMarshal2(wireFormat, o, dataOut, bs);

            ProducerAck info = (ProducerAck)o;
            TightMarshalNestedObject2(wireFormat, (DataStructure)info.ProducerId, dataOut, bs);
            dataOut.Write(info.Size);
        }

        // 
        // Un-marshal an object instance from the data input stream
        // 
        public override void LooseUnmarshal(OpenWireFormat wireFormat, Object o, BinaryReader dataIn) 
        {
            base.LooseUnmarshal(wireFormat, o, dataIn);

            ProducerAck info = (ProducerAck)o;
            info.ProducerId = (ProducerId) LooseUnmarshalNestedObject(wireFormat, dataIn);
            info.Size = dataIn.ReadInt32();
        }

        // 
        // Write a object instance to data output stream
        //
        public override void LooseMarshal(OpenWireFormat wireFormat, Object o, BinaryWriter dataOut)
        {

            ProducerAck info = (ProducerAck)o;

            base.LooseMarshal(wireFormat, o, dataOut);
            LooseMarshalNestedObject(wireFormat, (DataStructure)info.ProducerId, dataOut);
            dataOut.Write(info.Size);
        }
    }
}
