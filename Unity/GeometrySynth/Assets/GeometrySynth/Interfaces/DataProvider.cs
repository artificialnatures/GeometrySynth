namespace GeometrySynth.Interfaces
{
    public interface DataProvider
    {
        event ModuleDataChangedHandler ModuleCommandRecieved;
        bool Connect();
        bool Disconnect();
        bool RequestUpdate(int address);
        bool Receive();
        bool Send(string data);
    }
}