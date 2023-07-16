namespace VPNServer.RelayControl.Packet
{
    public interface IServerPacket
    {
        void WriteToStream(BinaryWriter writer);
    }
}
