using System;
using System.Diagnostics.CodeAnalysis;

namespace ReportsLibrary.Tools;

public class ReportsException : Exception
{
    public ReportsException() { }

    public ReportsException(string message)
        : base(message)
    {
    }

    public ReportsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public static void ThrowIfNullOrWhiteSpace(string argument)
    {
        if (string.IsNullOrWhiteSpace(argument)) Throw(argument);
    }

    [DoesNotReturn]
    private static void Throw(string paramName) => throw new ArgumentNullException(paramName);
}