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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace evilfoca.Attacks
{
    public static class DHCP
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        unsafe public struct bootp
        {
            public byte bp_op;              /* packet opcode type */
            public byte bp_htype;           /* hardware addr type */
            public byte bp_hlen;            /* hardware addr length */
            public byte bp_hops;            /* gateway hops */
            public uint bp_xid;             /* transaction ID */
            public ushort bp_secs;          /* seconds since boot began */
            public ushort bp_flags;         /* flags - see bootp_flag_values[] in print-bootp.c */
            public uint bp_ciaddr;          /* client IP address */
            public uint bp_yiaddr;          /* 'your' IP address */
            public uint bp_siaddr;          /* server IP address */
            public uint bp_giaddr;          /* gateway IP address */
            public fixed byte bp_chaddr[16];/* client hardware address */
            public fixed byte bp_sname[64]; /* server host name */
            public fixed byte bp_file[128]; /* boot file name */
            public fixed byte bp_vend[4];   /* vendor-specific area dinamic size*/
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct optionHeader
        {
            public byte option;
            public byte lenght;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //DHCP Message Type, 53
        public struct MessageType
        {
            public optionHeader optionHeader;
            public byte type;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //DHCP Server ID, 54
        public struct ServerID
        {
            public optionHeader optionHeader;
            public uint id;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //Requested IP Address, 50
        public struct RequestedIP
        {
            public optionHeader optionHeader;
            public uint id;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //IP AddressLease Time, 51
        public struct AddressLeaseTime
        {
            public optionHeader optionHeader;
            public uint time;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //Subnet Mask, 1
        public struct SubnetMask
        {
            public optionHeader optionHeader;
            public uint mask;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //Router, 3
        public struct Router
        {
            public optionHeader optionHeader;
            public uint ip;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //Domain Name server, 6
        public struct DomainNameServer
        {
            public optionHeader optionHeader;
            public uint ip;
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //Domain Name server, 17
        unsafe public struct Hostname
        {
            public optionHeader optionHeader;
            public fixed byte name[1];
        };
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        //The typical DHCP ACK response options
        public struct optionsResponse
        {
            public MessageType messageType;
            public ServerID serverID;
            public AddressLeaseTime addressLeaseTime;
            public SubnetMask subnetMask;
            public Router route;
            public DomainNameServer domainNameServer;
            public byte endOptions; //0xFF
        };

        /*
         * UDP port numbers, server and client.
         */
        public const byte IPPORT_BOOTPS = 67;
        public const byte IPPORT_BOOTPC = 68;

        public const byte BOOTPREPLY = 2;
        public const byte BOOTPREQUEST = 1;

        /*
         * Vendor magic cookie (v_magic; for RFC1048 (inverted to int comparison in little endian;
         */
        public const uint VM_RFC1048 = 0x63538263;

        /*
         * RFC1048 tag values used to specify what information is being supplied in
         * the vendor field of the packet.
         */

        public const byte TAG_PAD = 0;
        public const byte TAG_SUBNET_MASK = 1;
        public const byte TAG_TIME_OFFSET = 2;
        public const byte TAG_GATEWAY = 3;
        public const byte TAG_TIME_SERVER = 4;
        public const byte TAG_NAME_SERVER = 5;
        public const byte TAG_DOMAIN_SERVER = 6;
        public const byte TAG_LOG_SERVER = 7;
        public const byte TAG_COOKIE_SERVER = 8;
        public const byte TAG_LPR_SERVER = 9;
        public const byte TAG_IMPRESS_SERVER = 10;
        public const byte TAG_RLP_SERVER = 11;
        public const byte TAG_HOSTNAME = 12;
        public const byte TAG_BOOTSIZE = 13;
        public const byte TAG_END = 255;
        /* RFC1497 tags */
        public const byte TAG_DUMPPATH = 14;
        public const byte TAG_DOMAINNAME = 15;
        public const byte TAG_SWAP_SERVER = 16;
        public const byte TAG_ROOTPATH = 17;
        public const byte TAG_EXTPATH = 18;
        /* RFC2132 */
        public const byte TAG_IP_FORWARD = 19;
        public const byte TAG_NL_SRCRT = 20;
        public const byte TAG_PFILTERS = 21;
        public const byte TAG_REASS_SIZE = 22;
        public const byte TAG_DEF_TTL = 23;
        public const byte TAG_MTU_TIMEOUT = 24;
        public const byte TAG_MTU_TABLE = 25;
        public const byte TAG_INT_MTU = 26;
        public const byte TAG_LOCAL_SUBNETS = 27;
        public const byte TAG_BROAD_ADDR = 28;
        public const byte TAG_DO_MASK_DISC = 29;
        public const byte TAG_SUPPLY_MASK = 30;
        public const byte TAG_DO_RDISC = 31;
        public const byte TAG_RTR_SOL_ADDR = 32;
        public const byte TAG_STATIC_ROUTE = 33;
        public const byte TAG_USE_TRAILERS = 34;
        public const byte TAG_ARP_TIMEOUT = 35;
        public const byte TAG_ETH_ENCAP = 36;
        public const byte TAG_TCP_TTL = 37;
        public const byte TAG_TCP_KEEPALIVE = 38;
        public const byte TAG_KEEPALIVE_GO = 39;
        public const byte TAG_NIS_DOMAIN = 40;
        public const byte TAG_NIS_SERVERS = 41;
        public const byte TAG_NTP_SERVERS = 42;
        public const byte TAG_VENDOR_OPTS = 43;
        public const byte TAG_NETBIOS_NS = 44;
        public const byte TAG_NETBIOS_DDS = 45;
        public const byte TAG_NETBIOS_NODE = 46;
        public const byte TAG_NETBIOS_SCOPE = 47;
        public const byte TAG_XWIN_FS = 48;
        public const byte TAG_XWIN_DM = 49;
        public const byte TAG_NIS_P_DOMAIN = 64;
        public const byte TAG_NIS_P_SERVERS = 65;
        public const byte TAG_MOBILE_HOME = 68;
        public const byte TAG_SMPT_SERVER = 69;
        public const byte TAG_POP3_SERVER = 70;
        public const byte TAG_NNTP_SERVER = 71;
        public const byte TAG_WWW_SERVER = 72;
        public const byte TAG_FINGER_SERVER = 73;
        public const byte TAG_IRC_SERVER = 74;
        public const byte TAG_STREETTALK_SRVR = 75;
        public const byte TAG_STREETTALK_STDA = 76;
        /* DHCP options */
        public const byte TAG_REQUESTED_IP = 50;
        public const byte TAG_IP_LEASE = 51;
        public const byte TAG_OPT_OVERLOAD = 52;
        public const byte TAG_TFTP_SERVER = 66;
        public const byte TAG_BOOTFILENAME = 67;
        public const byte TAG_DHCP_MESSAGE = 53;
        public const byte TAG_SERVER_ID = 54;
        public const byte TAG_PARM_REQUEST = 55;
        public const byte TAG_MESSAGE = 56;
        public const byte TAG_MAX_MSG_SIZE = 57;
        public const byte TAG_RENEWAL_TIME = 58;
        public const byte TAG_REBIND_TIME = 59;
        public const byte TAG_VENDOR_CLASS = 60;
        public const byte TAG_CLIENT_ID = 61;
        /* RFC 2241 */
        public const byte TAG_NDS_SERVERS = 85;
        public const byte TAG_NDS_TREE_NAME = 86;
        public const byte TAG_NDS_CONTEXT = 87;
        /* RFC 2242 */
        public const byte TAG_NDS_IPDOMAIN = 62;
        public const byte TAG_NDS_IPINFO = 63;
        /* RFC 2485 */
        public const byte TAG_OPEN_GROUP_UAP = 98;
        /* RFC 2563 */
        public const byte TAG_DISABLE_AUTOCONF = 116;
        /* RFC 2610 */
        public const byte TAG_SLP_DA = 78;
        public const byte TAG_SLP_SCOPE = 79;
        /* RFC 2937 */
        public const byte TAG_NS_SEARCH = 117;
        /* RFC 3011 */
        public const byte TAG_IP4_SUBNET_SELECT = 118;
        /* RFC 3442 */
        public const byte TAG_CLASSLESS_STATIC_RT = 121;
        public const byte TAG_CLASSLESS_STA_RT_MS = 249;
        /* ftp://ftp.isi.edu/.../assignments/bootp-dhcp-extensions */
        public const byte TAG_USER_CLASS = 77;
        public const byte TAG_SLP_NAMING_AUTH = 80;
        public const byte TAG_CLIENT_FQDN = 81;
        public const byte TAG_AGENT_CIRCUIT = 82;
        public const byte TAG_AGENT_REMOTE = 83;
        public const byte TAG_AGENT_MASK = 84;
        public const byte TAG_TZ_STRING = 88;
        public const byte TAG_FQDN_OPTION = 89;
        public const byte TAG_AUTH = 90;
        public const byte TAG_VINES_SERVERS = 91;
        public const byte TAG_SERVER_RANK = 92;
        public const byte TAG_CLIENT_ARCH = 93;
        public const byte TAG_CLIENT_NDI = 94;
        public const byte TAG_CLIENT_GUID = 97;
        public const byte TAG_LDAP_URL = 95;
        public const byte TAG_6OVER4 = 96;
        public const byte TAG_PRINTER_NAME = 100;
        public const byte TAG_MDHCP_SERVER = 101;
        public const byte TAG_IPX_COMPAT = 110;
        public const byte TAG_NETINFO_PARENT = 112;
        public const byte TAG_NETINFO_PARENT_TAG = 113;
        public const byte TAG_URL = 114;
        public const byte TAG_FAILOVER = 115;
        public const byte TAG_EXTENDED_REQUEST = 126;
        public const byte TAG_EXTENDED_OPTION = 127;


        /* DHCP Message types (values for TAG_DHCP_MESSAGE option; */
        public const byte DHCPDISCOVER = 1;
        public const byte DHCPOFFER = 2;
        public const byte DHCPREQUEST = 3;
        public const byte DHCPDECLINE = 4;
        public const byte DHCPACK = 5;
        public const byte DHCPNAK = 6;
        public const byte DHCPRELEASE = 7;
        public const byte DHCPINFORM = 8;

        /* v_flags values */
        public const byte VF_SMASK = 1;       /* Subnet mask field contains valid data */

        /* RFC 4702 DHCP Client FQDN Option */

        public const byte CLIENT_FQDN_FLAGS_S = 0x01;
        public const byte CLIENT_FQDN_FLAGS_O = 0x02;
        public const byte CLIENT_FQDN_FLAGS_E = 0x04;
        public const byte CLIENT_FQDN_FLAGS_N = 0x08;
    }
}
