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
using System.Text;
using Apache.NMS.Stomp.Commands;
using Apache.NMS;

namespace Apache.NMS.Stomp.Protocol
{
    /// <summary>
    /// Some <a href="http://stomp.codehaus.org/">STOMP</a> protocol conversion helper methods.
    /// </summary>
    public class StompHelper
    {
        public static Destination ToDestination(string text)
        {
            if(text == null)
            {
                return null;
            }

            int type = Destination.STOMP_QUEUE;
            string lowertext = text.ToLower();
            if(lowertext.StartsWith("/queue/"))
            {
                text = text.Substring("/queue/".Length);
            }
            else if(lowertext.StartsWith("/topic/"))
            {
                text = text.Substring("/topic/".Length);
                type = Destination.STOMP_TOPIC;
            }
            else if(lowertext.StartsWith("/temp-topic/"))
            {
                text = text.Substring("/temp-topic/".Length);
                type = Destination.STOMP_TEMPORARY_TOPIC;
            }
            else if(lowertext.StartsWith("/temp-queue/"))
            {
                text = text.Substring("/temp-queue/".Length);
                type = Destination.STOMP_TEMPORARY_QUEUE;
            }
            else if(lowertext.StartsWith("/remote-temp-topic/"))
            {
                type = Destination.STOMP_TEMPORARY_TOPIC;
            }
            else if(lowertext.StartsWith("/remote-temp-queue/"))
            {
                type = Destination.STOMP_TEMPORARY_QUEUE;
            }

            return Destination.CreateDestination(type, text);
        }

        public static string ToStomp(Destination destination)
        {
            if(destination == null)
            {
                return null;
            }

            switch (destination.DestinationType)
            {
                case DestinationType.Topic:
                    return "/topic/" + destination.PhysicalName;

                case DestinationType.TemporaryTopic:
                    if (destination.PhysicalName.ToLower().StartsWith("/remote-temp-topic/"))
                    {
                        return destination.PhysicalName;
                    }

                    return "/temp-topic/" + destination.PhysicalName;

                case DestinationType.TemporaryQueue:
                    if (destination.PhysicalName.ToLower().StartsWith("/remote-temp-queue/"))
                    {
                        return destination.PhysicalName;
                    }

                    return "/temp-queue/" + destination.PhysicalName;

                default:
                    return "/queue/" + destination.PhysicalName;
            }
        }

        public static string ToStomp(AcknowledgementMode ackMode)
        {
            if(ackMode == AcknowledgementMode.IndividualAcknowledge)
            {
                return "client-individual";
            }
            else
            {
                return "client";
            }
        }

        public static bool ToBool(string text, bool defaultValue)
        {
            if(text == null)
            {
                return defaultValue;
            }

            return (0 == string.Compare("true", text, true));
        }
    }
}
