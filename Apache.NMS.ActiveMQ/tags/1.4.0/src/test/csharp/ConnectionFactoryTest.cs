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

using Apache.NMS.Test;
using Apache.NMS.ActiveMQ;

using NUnit.Framework;

namespace Apache.NMS.ActiveMQ.Test
{
	[TestFixture]
	public class ConnectionFactoryTest : NMSTestSupport
	{
		[Test, Sequential]
		public void TestConnectionFactorySetParams(
			[Values("tcp://${activemqhost}:61616", "activemq:tcp://${activemqhost}:61616")]
			string connectionURI,
			[Values(AcknowledgementMode.ClientAcknowledge, AcknowledgementMode.AutoAcknowledge)]
			AcknowledgementMode ackMode,
			[Values(true, false)]
			bool asyncSend,
			[Values(true, false)]
			bool alwaysSyncSend,
			[Values(true, false)]
			bool asyncClose,
			[Values(true, false)]
			bool copyMessageOnSend,
			[Values(3000, 1000)]
			int requestTimeout,
			[Values(true, false)]
			bool sendAcksAsync,
			[Values(true, false)]
			bool dispatchAsync)
		{
			ConnectionFactory factory = new ConnectionFactory(NMSTestSupport.ReplaceEnvVar(connectionURI));

			factory.AcknowledgementMode = ackMode;
			factory.AsyncSend = asyncSend;
			factory.AlwaysSyncSend = alwaysSyncSend;
			factory.AsyncClose = asyncClose;
			factory.CopyMessageOnSend = copyMessageOnSend;
			factory.RequestTimeout = requestTimeout;
			factory.SendAcksAsync = sendAcksAsync;
			factory.DispatchAsync = dispatchAsync;

			using(Connection connection = factory.CreateConnection() as Connection)
			{
				Assert.AreEqual(ackMode, connection.AcknowledgementMode);
				Assert.AreEqual(asyncSend, connection.AsyncSend);
				Assert.AreEqual(alwaysSyncSend, connection.AlwaysSyncSend);
				Assert.AreEqual(asyncClose, connection.AsyncClose);
				Assert.AreEqual(copyMessageOnSend, connection.CopyMessageOnSend);
				Assert.AreEqual(requestTimeout, connection.RequestTimeout.TotalMilliseconds);
				Assert.AreEqual(sendAcksAsync, connection.SendAcksAsync);
				Assert.AreEqual(dispatchAsync, connection.DispatchAsync);
			}
		}

		[Test, Sequential]
		public void TestConnectionFactoryParseParams(
			[Values("tcp://${activemqhost}:61616", "activemq:tcp://${activemqhost}:61616")]
			string baseConnectionURI,
			[Values(AcknowledgementMode.ClientAcknowledge, AcknowledgementMode.AutoAcknowledge)]
			AcknowledgementMode ackMode,
			[Values(true, false)]
			bool asyncSend,
			[Values(true, false)]
			bool alwaysSyncSend,
			[Values(true, false)]
			bool asyncClose,
			[Values(true, false)]
			bool copyMessageOnSend,
			[Values(3000, 1000)]
			int requestTimeout,
			[Values(true, false)]
			bool sendAcksAsync,
			[Values(true, false)]
			bool dispatchAsync)
		{
			string connectionURI = string.Format("{0}?" +
								   "connection.AckMode={1}&" +
								   "connection.AsyncSend={2}&" +
								   "connection.AlwaysSyncSend={3}&" +
								   "connection.AsyncClose={4}&" +
								   "connection.CopyMessageOnSend={5}&" +
								   "connection.RequestTimeout={6}&" +
								   "connection.SendAcksAsync={7}&" +
								   "connection.DispatchAsync={8}",
								   baseConnectionURI, ackMode, asyncSend, alwaysSyncSend, asyncClose, copyMessageOnSend, requestTimeout, sendAcksAsync, dispatchAsync);

			ConnectionFactory factory = new ConnectionFactory(NMSTestSupport.ReplaceEnvVar(connectionURI));

			using(Connection connection = factory.CreateConnection() as Connection)
			{
				Assert.AreEqual(ackMode, connection.AcknowledgementMode);
				Assert.AreEqual(asyncSend, connection.AsyncSend);
				Assert.AreEqual(alwaysSyncSend, connection.AlwaysSyncSend);
				Assert.AreEqual(asyncClose, connection.AsyncClose);
				Assert.AreEqual(copyMessageOnSend, connection.CopyMessageOnSend);
				Assert.AreEqual(requestTimeout, connection.RequestTimeout.TotalMilliseconds);
				Assert.AreEqual(sendAcksAsync, connection.SendAcksAsync);
				Assert.AreEqual(dispatchAsync, connection.DispatchAsync);
			}
		}
	}
}

