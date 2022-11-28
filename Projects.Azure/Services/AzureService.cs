using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Projects.Azure.Interfaces;
using Projects.Azure.Models;

namespace Projects.Azure.Services
{
    public class AzureService: IAzureService
    {
        private readonly ArmClient _armClient;

        public AzureService()
        {
            _armClient = new ArmClient(new DefaultAzureCredential());
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

            var resourecesPageable = resourceGroupResource.GetGenericResourcesAsync();

            List<AzureResource?> resources = new List<AzureResource?>();
            await foreach(var resource in resourecesPageable)
            {
                var azureResource = new AzureResource()
                {
                    Name = resource.Data.Name
                };
                resources.Add(azureResource);
            }

            return resources;
        }
    }
}
