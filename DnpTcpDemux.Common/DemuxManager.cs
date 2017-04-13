using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnpTcpDemux
{
    public class DemuxManager
    {
        public List<Demux> Demultiplexers { get; private set; }

        public DemuxManager(string configFile)
        {
            Demultiplexers = new List<Demux>();
        }

        public DemuxManager()
        {
            Demultiplexers = new List<Demux>();
        }

        public void AddDemultiplexer(Demux demux)
        {
            Demultiplexers.Add(demux);
        }
    }
}
