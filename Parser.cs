using ArgumentsParse.Exceptions;

namespace ArgumentsParse;

public class Parser
{
    public record ParserConfiguration(
        string ArgumentPrefix = "-",
        bool AllowUndefined = false,
        bool AllowUnmatchedArguments = false,
        bool AllowRepeatedArguments = false
    );

    private List<Argument> _arguments;
    private ParserConfiguration _configuration;

    private Dictionary<string, string> _result = null!;

    public Parser(IEnumerable<Argument> arguments, ParserConfiguration? configuration = null)
    {
        _arguments = arguments.ToList();
        _configuration = configuration ?? new ParserConfiguration();
    }

    public Dictionary<string, string> Parse(string[] args)
    {
        _result = new Dictionary<string, string>();
        var seen = new List<string>();
        var required = _arguments
            .Where(i => i.Required)
            .Select(i => i.Name)
            .ToList();
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (IsArgument(arg))
            {
                var argument = FindArgument(arg);
                if (argument != null)
                {
                    var alreadySeen = seen.Contains(arg);
                    if (!alreadySeen)
                        seen.Add(arg);
                    else if (!_configuration.AllowRepeatedArguments)
                        throw new RepeatedArgumentException(argument.Name);
                    if (argument.Required && !alreadySeen)
                        required.Remove(argument.Name);
                    HandleParameter(args, ref i, argument);
                }
                else if (!_configuration.AllowUndefined)
                    throw new UndefinedArgumentException(arg);
            }
            else if (!_configuration.AllowUnmatchedArguments)
                throw new UnmatchedArgumentException(i);
        }

        if (required.Count > 0)
            throw new RequiredArgumentsNotPresentException(required);
        return _result;
    }

    private bool IsArgument(string value)
        => value.StartsWith(_configuration.ArgumentPrefix);

    private Argument? FindArgument(string value)
        => _arguments.FirstOrDefault(argument => argument.Name.Equals(value) || argument.Aliases.Contains(value));

    private void HandleParameter(string[] args, ref int i, Argument argument)
    {
        i++;
        if (argument.ValueRequired)
        {
            if (i == args.Length)
                throw new RequiredValueNotPresentException(argument.Name);
            var arg = args[i];
            if (IsArgument(arg))
                throw new RequiredValueNotPresentException(argument.Name);
            _result[argument.Name] = arg;
        }
        else
        {
            if (i != args.Length && !IsArgument(args[i]))
                throw new UnmatchedArgumentException(i);
            _result[argument.Name] = "true";
        }
    }
}