using AuthProvider.Data.Models;
using Azure.Messaging.ServiceBus;
using System.Diagnostics;
using System.Text.Json;

namespace AuthProvider.Api.Handlers;

public class ServiceBusHandler(ServiceBusClient serviceBusClient)
{
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;

    public async Task SendMessageAsync<T>(string queueName, T messageContent)
    {
        try
        {
            var sender = _serviceBusClient.CreateSender(queueName);
            var message = new ServiceBusMessage(JsonSerializer.Serialize(messageContent));
            await sender.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
