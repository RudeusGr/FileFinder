namespace FileFinder;

public class ConfigurationJson
{
    public required IList<string> Extensions { get; set; }
    public required Dictionary<string, string> Months { get; set; }
    public required Dictionary<string, string> Urls { get; set; }
}
