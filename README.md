# DnpTcpSplitter
A TCP-DNP demultiplexer

This tool listens on a TCP port for DNP packets, and forwards the packets to destinations based on a lookup table based on the DNP address.  Intended to use with the SSE DNP Communications Server, as SSE does not support IP polling groups.

Usage:  DnpTcpSplitter.exe [-tcp|-udp] portnumber configfile
