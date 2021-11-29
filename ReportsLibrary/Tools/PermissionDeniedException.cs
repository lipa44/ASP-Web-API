using System;
namespace ReportsLibrary.Tools
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException() { }

        public PermissionDeniedException(string message)
            : base(message) { }

        public PermissionDeniedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}