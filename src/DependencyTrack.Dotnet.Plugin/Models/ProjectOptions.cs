namespace WeMicroIt.DependencyTrack.Dotnet.Models {
    public class ProjectOptions{
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string? ProjectUUID { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectVersion { get; set; }
        public string? ParentUUID { get; set; }
        public string? ParentName { get; set; }
        public string? ParentVersion { get; set; }
        public bool AutoCreate { get; set; } = false;
        public string BomFile { get; set; }
    }
}