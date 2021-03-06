/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Threading;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.ActiveMQ.Transport;
using Apache.NMS.ActiveMQ.Transport.Mock;
using NUnit.Framework;

namespace Apache.NMS.ActiveMQ.Test
{
    [TestFixture]
    public class MockTransportTest
    {
        private List<Command> sent = new List<Command>();
        private List<Command> received = new List<Command>();
        private List<Exception> exceptions = new List<Exception>();
        
        private MockTransport transport;
        
        public void OnException(ITransport transport, Exception exception)
        {
            Tracer.DebugFormat("MockTransportTest::onException - " + exception );            
            exceptions.Add( exception );
        }
        
        public void OnCommand(ITransport transport, Command command)
        {
            Tracer.DebugFormat("MockTransportTest::OnCommand - " + command );            
            received.Add( command );
        }
        
        public void OnOutgoingCommand(ITransport transport, Command command)
        {
            Tracer.DebugFormat("MockTransportTest::OnOutgoingCommand - " + command );            
            sent.Add( command );
        }

        [SetUp]
        public void Init()
        {
            this.transport = new MockTransport();

            transport.Command = new CommandHandler(OnCommand);
            transport.Exception = new ExceptionHandler(OnException);
            transport.OutgoingCommand = new CommandHandler(OnOutgoingCommand);
        }
        
        [Test]
        public void CreateMockTransportTest()
        {
            MockTransport transport = new MockTransport();
            
            Assert.IsNotNull(transport);
            Assert.IsFalse(transport.IsStarted);
            Assert.IsFalse(transport.IsDisposed);
        }

        [Test]
        public void StartItializedTransportTest()
        {
            MockTransport transport = new MockTransport();

            transport.Command = new CommandHandler(OnCommand);
            transport.Exception = new ExceptionHandler(OnException);

            transport.Start();
        }
        
        [Test]
        [ExpectedException( "System.InvalidOperationException" )]
        public void StartUnitializedTransportTest()
        {
            MockTransport transport = new MockTransport();
            transport.Start();
        }
        
        [Test]
        public void OneWaySendMessageTest()
        {
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            transport.Oneway( message );
            Assert.IsTrue(transport.NumSentMessages == 1);
            Assert.Contains(message, sent);
        }

        [Test]
        public void RequestMessageTest()
        {
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            transport.Request( message );
            Assert.IsTrue(transport.NumSentMessages == 1);
            Assert.Contains(message, sent);
        }
        
        [Test]
        [ExpectedException( "Apache.NMS.ActiveMQ.IOException" )]
        public void OneWayFailOnSendMessageTest()
        {
            transport.FailOnSendMessage = true;
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            transport.Oneway( message );
        }

        [Test]
        [ExpectedException( "Apache.NMS.ActiveMQ.IOException" )]
        public void RequestFailOnSendMessageTest()
        {
            transport.FailOnSendMessage = true;
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            Assert.IsNotNull( transport.Request( message ) );
        }
        
        [Test]
        [ExpectedException( "Apache.NMS.ActiveMQ.IOException" )]
        public void AsyncRequestFailOnSendMessageTest()
        {
            transport.FailOnSendMessage = true;
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            Assert.IsNotNull( transport.AsyncRequest( message ) );
        }
        
        [Test]
        [ExpectedException( "Apache.NMS.ActiveMQ.IOException" )]
        public void OnewayFailOnSendTwoMessagesTest()
        {
            transport.FailOnSendMessage = true;
            transport.NumSentMessagesBeforeFail = 2;
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            transport.Oneway( message );
            transport.Oneway( message );
            transport.Oneway( message );
        }        

        [Test]
        [ExpectedException( "Apache.NMS.ActiveMQ.IOException" )]
        public void RequestFailOnSendTwoMessagesTest()
        {
            transport.FailOnSendMessage = true;
            transport.NumSentMessagesBeforeFail = 2;
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            transport.Request( message );
            transport.Request( message );
            transport.Request( message );
        }        
        
        [Test]
        [ExpectedException( "Apache.NMS.ActiveMQ.IOException" )]
        public void AsyncRequestFailOnSendTwoMessagesTest()
        {
            transport.FailOnSendMessage = true;
            transport.NumSentMessagesBeforeFail = 2;
            transport.Start();
            ActiveMQTextMessage message = new ActiveMQTextMessage();
            transport.AsyncRequest( message );
            transport.AsyncRequest( message );
            transport.AsyncRequest( message );
        }
        
        [Test]
        public void InjectCommandTest()
        {
            ActiveMQMessage message = new ActiveMQMessage();
            
            transport.Start();            
            transport.InjectCommand(message);
            
            Thread.Sleep( 1000 );
            
            Assert.IsTrue(this.received.Count > 0 );
            Assert.IsTrue(transport.NumReceivedMessages == 1);
        }
        
        [Test]
        public void FailOnReceiveMessageTest()
        {
            ActiveMQMessage message = new ActiveMQMessage();
            
            transport.FailOnReceiveMessage = true;
            transport.Start();
            transport.InjectCommand(message);
            
            Thread.Sleep( 1000 );
            
            Assert.IsTrue(this.exceptions.Count > 0 );
            Assert.IsTrue(transport.NumReceivedMessages == 1);
        }        
    }
}
