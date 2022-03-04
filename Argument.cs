namespace ArgumentsParse;

public record Argument(
    string Name,
    string[] Aliases,
    bool Required,
    bool ValueRequired
)
{
    public static Argument Flag(
        string name,
        string[]? aliases = null,
        bool required = false
    ) => new(name, aliases ?? Array.Empty<string>(), required, false);
    
    public static Argument Parameter(
        string name,
        string[]? aliases = null,
        bool required = false
    ) => new(name, aliases ?? Array.Empty<string>(), required, true);
}
