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
//
//  NOTE!: This file is autogenerated - do not modify!
//         if you need to make a change, please see the Groovy scripts in the
//         activemq-core module
//



namespace Apache.NMS.ActiveMQ.Commands
{
	/// <summary>
	///  The ActiveMQ SessionId Command
	/// </summary>
	public class SessionId : BaseDataStructure, DataStructure
	{
		public const byte ID_SessionId = 121;

		string connectionId;
		long value;
		ConnectionId parentId;

		public override int GetHashCode()
		{
			int answer = 0;
			answer = (answer * 37) + HashCode(ConnectionId);
			answer = (answer * 37) + HashCode(Value);
			return answer;
		}

		public override bool Equals(object that)
		{
			if(that is SessionId)
			{
				return Equals((SessionId) that);
			}
			return false;
		}

		public virtual bool Equals(SessionId that)
		{
			if(!Equals(this.ConnectionId, that.ConnectionId))
				return false;
			if(!Equals(this.Value, that.Value))
				return false;
			return true;
		}

		public override string ToString()
		{
			return GetType().Name + "["
				+ " ConnectionId=" + ConnectionId
				+ " Value=" + Value
				+ " ]";
		}

		public override byte GetDataStructureType()
		{
			return ID_SessionId;
		}

		// Properties

		public string ConnectionId
		{
			get { return connectionId; }
			set { this.connectionId = value; }
		}

		public long Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public ConnectionId ParentId
		{
			get
			{
				if(parentId == null)
				{
					parentId = new ConnectionId(this);
				}
				return parentId;
			}
		}

		public SessionId()
			: base()
		{
		}

		public SessionId(ConnectionId connectionId, long sessionId)
		{
			this.connectionId = connectionId.Value;
			this.value = sessionId;
		}

		public SessionId(SessionId id)
		{
			this.connectionId = id.ConnectionId;
			this.value = id.Value;
		}

		public SessionId(ProducerId id)
		{
			this.connectionId = id.ConnectionId;
            this.value = id.SessionId;
		}

		public SessionId(ConsumerId id)
		{
			this.connectionId = id.ConnectionId;
			this.value = id.SessionId;
		}
	}
}
