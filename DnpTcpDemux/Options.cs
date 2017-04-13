using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnpTcpDemux
{
    public class Options
    {
            [Option('p', "port", Required = true, HelpText = "Port Number")]
            public UInt16 PortNumber { get; set; }
            
            [HelpOption]
            public string GetUsage()
            {
                HelpText ht = HelpText.AutoBuild(new Options());
                ht.Copyright = "Copyright 2016";
                ht.Heading = "DNP-TCP Demultiplexer" + Environment.NewLine + "Tool splitting one TCP connection into many remote endpoints based on the DNP address";

                return ht.ToString();
            }
        
    }
}
