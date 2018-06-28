// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace ILOG.J2CsMapping.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class InetAddress
    {
        private IPAddress address;
        private IPHostEntry host;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public String GetHostAddress() 
        { 
            return host.AddressList[0].ToString(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public String GetHostName() 
        {
            return host.HostName; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public static InetAddress[] GetAllByName(String name) 
        {
            IPAddress[] ipa = System.Net.Dns.GetHostEntry(name).AddressList;

            InetAddress[] ina = new InetAddress[ipa.Length];

            for (int i = 0; i < ipa.Length; i++)
            {
                ina[i] = new InetAddress();
                ina[i].address = ipa[i];
            }

            return ina;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static InetAddress GetLocalHost() 
        {
            IPHostEntry iphe = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            InetAddress ina = new InetAddress();
            ina.host = iphe;

            return ina;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ina"></param>
        /// <returns></returns>
        public static implicit operator IPHostEntry(InetAddress ina)
        {
            return ina.host;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ina"></param>
        /// <returns></returns>
        public static implicit operator IPAddress(InetAddress ina)
        {
            return ina.address;
        }
    }
}
