namespace ImmersingPicker.Services.Services;

public class VersionServices
{
    public static VersionInfo CurrentVersion = new VersionInfo()
    { Major = 1, Minor = 0, Patch = 0, Step = VersionStep.ALPHA,
        Codename = "Ellen"
    };

    public static string VersionString(VersionInfo info) => $"{info.Major}.{info.Minor}.{info.Patch}" +
                                                            $"{info.Step switch { VersionStep.ALPHA => "-Alpha", VersionStep.BETA => "-Beta", _ => "" }} " +
                                                            $"codename {info.Codename}";

    public enum VersionStep
    {
        ALPHA, BETA, GA
    }

    public struct VersionInfo
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public VersionStep Step { get; set; }
        public string Codename { get; set; }
    }
}