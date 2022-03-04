namespace ArgumentsParse.Exceptions;

public class ParserException : Exception
{
    public ParserException(string message) : base(message)
    {
    }
}

public class UndefinedArgumentException : ParserException
{
    public string Name { get; }

    public UndefinedArgumentException(string name) : base($"Undefined argument '{name}'")
        => Name = name;
}

public class RepeatedArgumentException : ParserException
{
    public string Name { get; }

    public RepeatedArgumentException(string name) : base($"Repeated argument '{name}'")
        => Name = name;
}

public class UnmatchedArgumentException : ParserException
{
    public int Index { get; }

    public UnmatchedArgumentException(int index) : base($"Unmatched argument at index {index}")
        => Index = index;
}

public class RequiredValueNotPresentException : ParserException
{
    public string Name { get; }

    public RequiredValueNotPresentException(string name) : base($"Required value for argument '{name}' not present")
        => Name = name;
}

public class RequiredArgumentsNotPresentException : ParserException
{
    public List<string> Names { get; }

    public RequiredArgumentsNotPresentException(List<string> names) : base($"Required argument '{string.Join("', '", names)}' not present")
        => Names = names;
}
