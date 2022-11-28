using Azure.ResourceManager.Resources;
using Projects.Azure.Models;

namespace Projects.Azure.Interfaces
{
    public interface IAzureService
    {
        public List<AzureResourceGroup> GetAllResourceGroups();
        public Task<List<AzureResource?>> GetAllResources(string resourceGroupName);
    }
}
