namespace DnpTcpDemux
{
    class Program
    {
        static void Main(string[] args)
        {
            DemuxManager lm = new DemuxManager();
            Demux l = new Demux(5000);
            l.AddEndPoint(10, new System.Net.Sockets.TcpClient("localhost", 20001));
            lm.AddDemultiplexer(l);
            
        }
    }
}
