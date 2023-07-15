using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNServer.RelayControl.Packet
{
    public interface IServerPacket
    {
        void WriteToStream(BinaryWriter writer);
    }
}
