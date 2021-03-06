/**
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
package org.apache.activemq.nms.openwire.tool.commands;

import java.io.PrintWriter;

public class SessionIdGenerator extends CommandClassGenerator {

    protected void generateConstructors( PrintWriter out ) {

        out.println("        public "+getClassName()+"()");
        out.println("        {");
        out.println("        }");
        out.println("");
        out.println("        public "+getClassName()+"( ConnectionId connectionId, long sessionId )");
        out.println("        {");
        out.println("            this.ConnectionId = connectionId.Value;");
        out.println("            this.value = sessionId;");
        out.println("        }");
        out.println("");
        out.println("        public "+getClassName()+"( ProducerId producerId )");
        out.println("        {");
        out.println("            this.ConnectionId = producerId.ConnectionId;");
        out.println("            this.value = producerId.SessionId;");
        out.println("        }");
        out.println("");
        out.println("        public "+getClassName()+"( ConsumerId consumerId )");
        out.println("        {");
        out.println("            this.ConnectionId = consumerId.ConnectionId;");
        out.println("            this.value = consumerId.SessionId;");
        out.println("        }");
        out.println("");

        super.generateConstructors(out);
    }

    protected void generateProperties( PrintWriter out ) {

        out.println("        private ConnectionId parentId;");
        out.println("");

        super.generateProperties(out);
    }

    protected void generateToStringBody( PrintWriter out ) {
        out.println("            return this.connectionId + \":\" + this.value;");
    }

    protected void generateAdditonalMembers( PrintWriter out ) {
        out.println("        public ConnectionId ParentId");
        out.println("        {");
        out.println("            get");
        out.println("            {");
        out.println("                 if( this.parentId == null ) {");
        out.println("                     this.parentId = new ConnectionId( this );");
        out.println("                 }");
        out.println("                 return this.parentId;");
        out.println("            }");
        out.println("        }");
        out.println("");

        super.generateAdditonalMembers( out );
    }

}
