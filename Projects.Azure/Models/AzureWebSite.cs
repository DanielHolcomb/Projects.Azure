namespace Projects.Azure.Models
{
    public class AzureWebSite:AzureResource
    {
        public string RepoUri { get; set; }
        public string RepoPath { get; set; }
    }
}
