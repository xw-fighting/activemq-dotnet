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
using System.Collections.Specialized;
using Apache.NMS.Util;
using Apache.NMS.Policies;
using Apache.NMS.MQTT.Transport;
using Apache.NMS.MQTT.Util;

namespace Apache.NMS.MQTT
{
    /// <summary>
    /// Represents a connection with a message broker
    /// </summary>
    public class ConnectionFactory : IConnectionFactory
    {
        public const string DEFAULT_BROKER_URL = "tcp://localhost:61613";
        public const string ENV_BROKER_URL = "ACTIVEMQ_BROKER_URL";

        private static event ExceptionListener onException;
        private Uri brokerUri;
        private string connectionUserName;
        private string connectionPassword;
        private string clientId;
        private string clientIdPrefix;
        private IdGenerator clientIdGenerator;

        private bool copyMessageOnSend = true;
        private bool asyncSend;
        private bool alwaysSyncSend;
        private bool sendAcksAsync=true;
        private bool dispatchAsync=true;
        private TimeSpan requestTimeout = NMSConstants.defaultRequestTimeout;
        private AcknowledgementMode acknowledgementMode = AcknowledgementMode.AutoAcknowledge;

        private IRedeliveryPolicy redeliveryPolicy = new RedeliveryPolicy();

        static ConnectionFactory()
        {
            TransportFactory.OnException += ConnectionFactory.ExceptionHandler;
        }

        public static string GetDefaultBrokerUrl()
        {
            return Environment.GetEnvironmentVariable(ENV_BROKER_URL) ?? DEFAULT_BROKER_URL;
        }

        public ConnectionFactory() : this(GetDefaultBrokerUrl())
        {
        }

        public ConnectionFactory(string brokerUri) : this(brokerUri, null)
        {
        }

        public ConnectionFactory(string brokerUri, string clientID)
            : this(URISupport.CreateCompatibleUri(brokerUri), clientID)
        {
        }

        public ConnectionFactory(Uri brokerUri) : this(brokerUri, null)
        {
        }

        public ConnectionFactory(Uri brokerUri, string clientID)
        {
            this.BrokerUri = brokerUri;
            this.ClientId = clientID;
        }

        public IConnection CreateConnection()
        {
            return CreateConnection(connectionUserName, connectionPassword);
        }

        public IConnection CreateConnection(string userName, string password)
        {
            Connection connection = null;

            try
            {
                Tracer.InfoFormat("Connecting to: {0}", brokerUri.ToString());

                ITransport transport = TransportFactory.CreateTransport(brokerUri);

                connection = new Connection(brokerUri, transport, this.ClientIdGenerator);

                connection.UserName = userName;
                connection.Password = password;

                ConfigureConnection(connection);

                if(this.clientId != null)
                {
					// Set the connection factory version as the default, the user can
					// still override this via a call to Connection.ClientId = XXX
                    connection.DefaultClientId = this.clientId;
                }

                connection.ITransport.Start();

                return connection;
            }
            catch(NMSException e)
            {
                try
                {
                    connection.Close();
                }
                catch
                {
                }

                throw e;
            }
            catch(Exception e)
            {
                try
                {
                    connection.Close();
                }
                catch
                {
                }

                throw NMSExceptionSupport.Create("Could not connect to broker URL: " + this.brokerUri + ". Reason: " + e.Message, e);
            }
        }

        #region ConnectionFactory Properties

        /// <summary>
        /// Get/or set the broker Uri.
        /// </summary>
        public Uri BrokerUri
        {
            get { return brokerUri; }
            set
            {
                brokerUri = new Uri(URISupport.StripPrefix(value.OriginalString, "mqtt:"));

                if(!String.IsNullOrEmpty(brokerUri.Query) && !brokerUri.OriginalString.EndsWith(")"))
                {
                    // Since the Uri class will return the end of a Query string found in a Composite
                    // URI we must ensure that we trim that off before we proceed.
                    string query = brokerUri.Query.Substring(brokerUri.Query.LastIndexOf(")") + 1);

                    StringDictionary properties = URISupport.ParseQuery(query);
                
                    StringDictionary connection = URISupport.ExtractProperties(properties, "connection.");
                    
                    if(connection != null)
                    {
                        URISupport.SetProperties(this, connection, "connection.");
                    }

                    brokerUri = URISupport.CreateRemainingUri(brokerUri, properties);
                }
            }
        }

        public string UserName
        {
            get { return connectionUserName; }
            set { connectionUserName = value; }
        }

        public string Password
        {
            get { return connectionPassword; }
            set { connectionPassword = value; }
        }

        public string ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }

        public string ClientIdPrefix
        {
            get { return clientIdPrefix; }
            set { clientIdPrefix = value; }
        }

        public bool CopyMessageOnSend
        {
            get { return copyMessageOnSend; }
            set { copyMessageOnSend = value; }
        }

        public bool AlwaysSyncSend
        {
            get { return alwaysSyncSend; }
            set { alwaysSyncSend = value; }
        }

        public bool SendAcksAsync
        {
            get { return sendAcksAsync; }
            set { sendAcksAsync = value; }
        }

        public bool AsyncSend
        {
            get { return asyncSend; }
            set { asyncSend = value; }
        }

        public string AckMode
        {
            set { this.acknowledgementMode = NMSConvert.ToAcknowledgementMode(value); }
        }

        public bool DispatchAsync
        {
            get { return this.dispatchAsync; }
            set { this.dispatchAsync = value; }
        }

        public int RequestTimeout
        {
            get { return (int) this.requestTimeout.TotalMilliseconds; }
            set { this.requestTimeout = TimeSpan.FromMilliseconds(value); }
        }

        public AcknowledgementMode AcknowledgementMode
        {
            get { return acknowledgementMode; }
            set { this.acknowledgementMode = value; }
        }

        public IRedeliveryPolicy RedeliveryPolicy
        {
            get { return this.redeliveryPolicy; }
            set
            {
                if(value != null)
                {
                    this.redeliveryPolicy = value;
                }
            }
        }

        public IdGenerator ClientIdGenerator
        {
            set { this.clientIdGenerator = value; }
            get
            {
                lock(this)
                {
                    if(this.clientIdGenerator == null)
                    {
                        if(this.clientIdPrefix != null)
                        {
                            this.clientIdGenerator = new IdGenerator(this.clientIdPrefix);
                        }
                        else
                        {
                            this.clientIdGenerator = new IdGenerator();
                        }
                    }

                    return this.clientIdGenerator;
                }
            }
        }

        public event ExceptionListener OnException
        {
            add { onException += value; }
            remove
            {
                if(onException != null)
                {
                    onException -= value;
                }
            }
        }

        private ConsumerTransformerDelegate consumerTransformer;
        public ConsumerTransformerDelegate ConsumerTransformer
        {
            get { return this.consumerTransformer; }
            set { this.consumerTransformer = value; }
        }

        private ProducerTransformerDelegate producerTransformer;
        public ProducerTransformerDelegate ProducerTransformer
        {
            get { return this.producerTransformer; }
            set { this.producerTransformer = value; }
        }

        #endregion

        protected virtual void ConfigureConnection(Connection connection)
        {
            connection.AcknowledgementMode = this.acknowledgementMode;
            connection.RequestTimeout = this.requestTimeout;
            connection.ConsumerTransformer = this.consumerTransformer;
            connection.ProducerTransformer = this.producerTransformer;
        }

        protected static void ExceptionHandler(Exception ex)
        {
            if(ConnectionFactory.onException != null)
            {
                ConnectionFactory.onException(ex);
            }
        }
    }
}
