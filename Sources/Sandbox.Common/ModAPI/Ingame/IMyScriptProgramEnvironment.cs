namespace Sandbox.ModAPI.Ingame
{
    internal interface IMyScriptProgramEnvironment
    {
        IMyProgrammableBlock ProgrammableBlock { get; }
        void WriteEcho(string message);
        bool SetTimeout(int timeout, MyScriptTimeoutFunction callback);
    }
}