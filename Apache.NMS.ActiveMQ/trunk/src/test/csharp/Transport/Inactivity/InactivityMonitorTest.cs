// /*
//  * Licensed to the Apache Software Foundation (ASF) under one or more
//  * contributor license agreements.  See the NOTICE file distributed with
//  * this work for additional information regarding copyright ownership.
//  * The ASF licenses this file to You under the Apache License, Version 2.0
//  * (the "License"); you may not use this file except in compliance with
//  * the License.  You may obtain a copy of the License at
//  *
//  *     http://www.apache.org/licenses/LICENSE-2.0
//  *
//  * Unless required by applicable law or agreed to in writing, software
//  * distributed under the License is distributed on an "AS IS" BASIS,
//  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  * See the License for the specific language governing permissions and
//  * limitations under the License.
//  */
//

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Transport;
using Apache.NMS.ActiveMQ.Transport.Mock;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.ActiveMQ.OpenWire;
using NUnit.Framework;
using NUnit.Framework.Extensions;

namespace Apache.NMS.ActiveMQ.Test
{
    //[TestFixture]
    public class InactivityMonitorTest
    {
        private List<Command> received;
        private List<Exception> exceptions;
        private MockTransport transport = null;
        private WireFormatInfo localWireFormatInfo = null;

        public void OnException(ITransport transport, Exception exception)
        {
            Tracer.Debug("Test: Received Exception from Transport: " + exception );
            exceptions.Add( exception );
        }

        public void OnCommand(ITransport transport, Command command)
        {
            Tracer.Debug("Test: Received Command from Transport: " + command );
            received.Add( command );
        }

        //[SetUp]
        public void SetUp()
        {
            this.received = new List<Command>();
            this.exceptions = new List<Exception>();

            Uri uri = new Uri("mock://mock?wireformat=openwire");
            MockTransportFactory factory = new MockTransportFactory();

            this.transport = factory.CompositeConnect( uri ) as MockTransport;

            this.localWireFormatInfo = new WireFormatInfo();

            this.localWireFormatInfo.Version = 5;
            this.localWireFormatInfo.MaxInactivityDuration = 3000;
            this.localWireFormatInfo.TightEncodingEnabled = false;
        }

        //[Test]
        public void TestCreate()
        {
            InactivityMonitor monitor = new InactivityMonitor( this.transport );

            Assert.IsTrue( monitor.InitialDelayTime == 0 );
            Assert.IsTrue( monitor.ReadCheckTime == 0 );
            Assert.IsTrue( monitor.WriteCheckTime == 0 );
            Assert.IsTrue( monitor.KeepAliveResponseRequired == false );
            Assert.IsTrue( monitor.IsDisposed == false );
        }

        //[Test]
        public void TestReadTimeout()
        {
            InactivityMonitor monitor = new InactivityMonitor( this.transport );

            monitor.Exception += new ExceptionHandler(OnException);

            // Send the local one for the monitor to record.
            monitor.Oneway( this.localWireFormatInfo );

            Thread.Sleep( 2000 );

            // Should not have timed out on Read yet.
            Assert.IsTrue( this.exceptions.Count == 0 );

            Thread.Sleep( 5000 );

            // Channel should have been inactive for to long.
            Assert.IsTrue( this.exceptions.Count > 0 );
        }

        //[Test]
        public void TestWriteMessageFail()
        {
            this.transport.FailOnKeepAliveInfoSends = true ;
            this.transport.NumSentKeepAliveInfosBeforeFail = 4;

            InactivityMonitor monitor = new InactivityMonitor( this.transport );

            monitor.Exception += new ExceptionHandler(OnException);

            // Send the local one for the monitor to record.
            monitor.Oneway( this.localWireFormatInfo );

            Thread.Sleep( 2000 );

            ActiveMQMessage message = new ActiveMQMessage();
            this.transport.InjectCommand( message );

            // Should not have timed out on Read yet.
            Assert.IsTrue( this.exceptions.Count == 0 );

            Thread.Sleep( 8000 );

            // Channel should have been inactive for to long.
            Assert.IsTrue( this.exceptions.Count > 0 );
        }

        //[Test]
        public void TestNonFailureSendCase()
        {
            InactivityMonitor monitor = new InactivityMonitor( this.transport );

            monitor.Exception += new ExceptionHandler(OnException);

            // Send the local one for the monitor to record.
            monitor.Oneway( this.localWireFormatInfo );

            ActiveMQMessage message = new ActiveMQMessage();
            for( int ix = 0; ix < 20; ++ix )
            {
                monitor.Oneway( message );
                Thread.Sleep( 500 );
                this.transport.InjectCommand( message );
                Thread.Sleep( 500 );
            }

            // Channel should have been inactive for to long.
            Assert.IsTrue( this.exceptions.Count == 0 );
        }

    }
}