//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;

namespace Apache.NMS.MQTT.Protocol
{
	public class Header
	{
		private byte value;

		public Header(int commandType, int qos, bool dup, bool retain)
		{
			Type = commandType;
			QoS = qos;
			Dup = dup;
			Retain = retain;
		}

		public Header(byte value)
		{
			this.value = value;
		}

		public byte RawValue
		{
			get { return this.value; }
			set { this.value = value; }
		}

		public int Type
		{
			get { return (this.value & 0xF0) >> 4; }
			set
			{
				this.value &= 0x0F;
				this.value |= (byte)((value << 4) & 0xF0);
			}
		}

		public int QoS
		{
			get { return (this.value & 0x06) >> 1; }
			set
			{
				this.value &= 0xF9;
				this.value |= (byte)((value << 1) & 0x06);
			}
		}

		public bool Dup
		{
			get { return (this.value & 0x08) > 0; }
			set
			{
				if (value)
				{
					this.value |= 0x08;
				}
				else
				{
					this.value &= 0xF7;
				}
			}
		}

		public bool Retain
		{
			get { return (this.value & 0x01) > 0; }
			set
			{
				if(value)
				{
					this.value |= 0x01;
				}
				else
				{
					this.value &= 0xFE;
				}
			}
		}
	}
}

