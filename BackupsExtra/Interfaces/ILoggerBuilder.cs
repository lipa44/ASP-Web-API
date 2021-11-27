using SerilogLogger = Serilog.Core.Logger;

namespace BackupsExtra.Interfaces
{
    public interface ILoggerBuilder
    {
        SerilogLogger CreateInstance();
    }
}