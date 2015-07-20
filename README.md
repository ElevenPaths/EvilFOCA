### Evil FOCA

## Introduction
Evil Foca is a tool for security pentesters and auditors whose purpose it is to test security in IPv4 and IPv6 data networks.
The tool is capable of carrying out various attacks such as:

- MITM over IPv4 networks with ARP Spoofing and DHCP ACK Injection.
- MITM on IPv6 networks with Neighbor Advertisement Spoofing, SLAAC attack, fake DHCPv6.
- DoS (Denial of Service) on IPv4 networks with ARP Spoofing.
- DoS (Denial of Service) on IPv6 networks with SLAAC DoS.
- DNS Hijacking.

The software automatically scans the networks and identifies all devices and their respective network interfaces, specifying their IPv4 and IPv6 addresses as well as the physical addresses through a convenient and intuitive interface.

## Man In The Middle (MITM) attack
The well-known “Man In The Middle” is an attack in which the wrongdoer creates the possibility of reading, adding, or modifying information that is located in a channel between two terminals with neither of these noticing. Within the MITM attacks in IPv4 and IPv6 Evil Foca considers the following techniques:

- **ARP Spoofing**:
Consists in sending ARP messages to the Ethernet network. Normally the objective is to associate the MAC address of the attacker with the IP of another device. Any traffic directed to the IP address of the predetermined link gate will be erroneously sent to the attacker instead of its real destination.
- **DHCP ACK Injection**:
Consists in an attacker monitoring the DHCP exchanges and, at some point during the communication, sending a packet to modify its behavior. Evil Foca converts the machine in a fake DHCP server on the network.
- **Neighbor Advertisement Spoofing**:
The principle of this attack is identical to that of ARP Spoofing, with the difference being in that IPv6 doesn’t work with the ARP protocol, but that all information is sent through ICMPv6 packets. There are five types of ICMPv6 packets used in the discovery protocol and Evil Foca generates this type of packets, placing itself between the gateway and victim.
- **SLAAC attack**:
The objective of this type of attack is to be able to execute an MITM when a user connects to Internet and to a server that does not include support for IPv6 and to which it is therefore necessary to connect using IPv4. This attack is possible due to the fact that Evil Foca undertakes domain name resolution once it is in the communication media, and is capable of transforming IPv4 addresses in IPv6.
- **Fake DHCPv6 server**:
This attack involves the attacker posing as the DCHPv6 server, responding to all network requests, distributing IPv6 addresses and a false DNS to manipulate the user destination or deny the service.
- **Denial of Service (DoS) attack**:
The DoS attack is an attack to a system of machines or network that results in a service or resource being inaccessible for its users. Normally it provokes the loss of network connectivity due to consumption of the bandwidth of the victim’s network, or overloads the computing resources of the victim’s system.
- **DoS attack in IPv4 with ARP Spoofing**:
This type of DoS attack consists in associating a nonexistent MAC address in a victim’s ARP table. This results in rendering the machine whose ARP table has been modified incapable of connecting to the IP address associated to the nonexistent MAC.
- **DoS attack in IPv6 with SLAAC attack**:
In this type of attack a large quantity of “router advertisement” packets are generated, destined to one or several machines, announcing false routers and assigning a different IPv6 address and link gate for each router, collapsing the system and making machines unresponsive.
- **DNS Hijacking**:
The DNS Hijacking attack or DNS kidnapping consists in altering the resolution of the domain names system (DNS). This can be achieved using malware that invalidates the configuration of a TCP/IP machine so that it points to a pirate DNS server under the attacker’s control, or by way of an MITM attack, with the attacker being the party who receives the DNS requests, and responding himself or herself to a specific DNS request to direct the victim toward a specific destination selected by the attacker.

## License

EVil FOCA is developed by [ElevenPaths](https://www.elevenpaths.com) and released under the GNU Public License 3.0.
For more information, visit the Evil FOCA's webpage at [https://www.elevenpaths.com/labstools/evil-foca/index.html](https://www.elevenpaths.com/labstools/evil-foca/index.html)
