using System;

namespace Backups.Tools
{
    public class BackupException : Exception
    {
        public BackupException()
        {
        }

        public BackupException(string message)
            : base(message)
        {
        }

        public BackupException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}