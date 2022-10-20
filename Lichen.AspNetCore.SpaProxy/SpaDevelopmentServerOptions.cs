namespace Lichen.AspNetCore.SpaProxy
{
    internal class SpaDevelopmentServerOptions
    {
        public string ServerUrl { get; set; } = "";

        public string LaunchCommand { get; set; } = "";

        public int MaxTimeoutInSeconds { get; set; }

        public TimeSpan MaxTimeout => TimeSpan.FromSeconds(MaxTimeoutInSeconds);

        public string WorkingDirectory { get; set; } = "";
    }
}