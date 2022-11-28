using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Projects.Azure.Interfaces;
using Projects.Azure.Models;
using Azure.ResourceManager.AppService;

namespace Projects.Azure.Services
{
    public class AzureService: IAzureService
    {
        private readonly ArmClient _armClient;
        public AzureService(IConfiguration configuration)
        {
            _armClient = new ArmClient(new DefaultAzureCredential(), configuration.GetSection("Azure:SubscriptionId").Value);
        }

        public List<AzureResourceGroup> GetAllResourceGroups()
        {
            var resourceGroupCollection = _armClient.GetDefaultSubscription().GetResourceGroups();

            var resourceGroups = new List<AzureResourceGroup>();

            foreach(var resourceGroup in resourceGroupCollection)
            {
                var azureResourceGroup = new AzureResourceGroup()
                {
                    Name = resourceGroup.Data.Name
                };
                resourceGroups.Add(azureResourceGroup);
            }

            return resourceGroups;
        }

        public async Task<List<AzureResource?>> GetAllResources(string resourceGroupName)
        {
            ResourceGroupResource resourceGroupResource =
                _armClient.GetDefaultSubscription().GetResourceGroup(resourceGroupName);

            var resourcesPageable = resourceGroupResource.GetGenericResourcesAsync();

            List<AzureResource?> resources = new List<AzureResource?>();
            await foreach(var resource in resourcesPageable)
            {
                var azureResource = new AzureResource()
                {
                    Name = resource.Data.Name,
                    Type = resource.Data.ResourceType.Type,
                    NameSpace = resource.Data.ResourceType.Namespace
                };
                resources.Add(azureResource);
            }

            return resources;
        }

        public async Task<List<AzureWebSite?>> GetAllWebSites(string resourceGroupName)
        {
            ResourceGroupResource resourceGroupResource =
                _armClient.GetDefaultSubscription().GetResourceGroup(resourceGroupName);

            var websites = resourceGroupResource.GetWebSites();

            List<AzureWebSite?> resources = new List<AzureWebSite?>();
            await foreach (var site in websites)
            {
                var repo = site.GetWebSiteSourceControl()?.Get()?.Value?.Data;
                if (repo?.RepoUri?.AbsolutePath != null)
                {
                    var azureResource = new AzureWebSite()
                    {
                        Name = site.Data.Name,
                        Type = site.Data.ResourceType.Type,
                        NameSpace = site.Data.ResourceType.Namespace,
                        RepoUri = repo.RepoUri.ToString(),
                        RepoPath = repo.RepoUri.AbsolutePath
                    };
                    resources.Add(azureResource);
                }
            }

            return resources;
        }

    }
}
