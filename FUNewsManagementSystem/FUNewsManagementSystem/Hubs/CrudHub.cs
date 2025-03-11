using Azure;
using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FUNewsManagementSystem.Hubs
{
    public class CrudHub : Hub
    {
        public async Task EntityCreated(string entityName, object entity)
        {
            await Clients.All.SendAsync("EntityCreated", entityName, entity);
        }

        public async Task EntityUpdated(string entityName, object entity)
        {
            await Clients.All.SendAsync("EntityUpdated", entityName, entity);
        }

        public async Task EntityDeleted(string entityName, object id)
        {
            await Clients.All.SendAsync("EntityDeleted", entityName, id);
        }
    }
}
