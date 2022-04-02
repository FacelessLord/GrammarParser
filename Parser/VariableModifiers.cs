using System;

namespace Parser
{
    [Flags]
    public enum VariableModifiers
    {
        
        Const = 1,
        Let = 2,
        Compile = 4,
    }
}