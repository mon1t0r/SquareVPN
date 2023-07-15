namespace WebAPI.Relays.ControlServer.Packet
{
    public interface IRelayPacket
    {
        void WriteToStream(BinaryWriter writer);
    }
}
