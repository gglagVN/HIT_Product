using System.Collections.Generic;

namespace Thnguyet.ScriptableObjectArchitecture
{
    public interface IStackTraceObject
    {
        List<StackTraceEntry> StackTraces { get; }

        void AddStackTrace();
        void AddStackTrace(object value);
    } 
}