using System.Reflection;

namespace ImmersingPicker.Services.Services;

public class VersionServices
{
    public static VersionInfo CurrentVersion = new VersionInfo(Assembly.GetExecutingAssembly().GetName().Version);

    public static string VersionString(VersionInfo info) => $"{info.Major}.{info.Minor}.{info.Patch}" +
                                                            $"{info.Step switch { VersionStep.ALPHA => "-Alpha", VersionStep.BETA => "-Beta", _ => "" }} " +
                                                            $"codename {info.Codename}";

    public enum VersionStep
    {
        ALPHA, BETA, GA
    }

    public struct VersionInfo
    {
        public static Dictionary<Tuple<int, int>, string> Codenames = new()
        {
            { new(0, 1), "Ellen" }
        };

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public VersionStep Step { get; set; }
        public string Codename { get; set; }

        public VersionInfo(Version version)
        {
            Major = version.Major;
            Minor = version.Minor;
            Patch = version.Build;
            Step = version.Revision switch
            {
                1 => VersionStep.ALPHA,
                2 => VersionStep.BETA,
                _ => VersionStep.GA
            };
            Codename = Codenames.TryGetValue(new(Major, Minor), out var codename) ? codename : "Belle?";
        }
    }
}