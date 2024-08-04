using AuthProvider.Data.Models;
using Azure.Messaging.ServiceBus;
using System.Diagnostics;
using System.Text.Json;

namespace AuthProvider.Api.Handlers;

public class ServiceBusHandler
{
    private readonly ServiceBusClient _serviceBusClient;

    public ServiceBusHandler(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }

    public async Task SendUserCreatedMessageAsync(SignUp user)
    {
        var sender = _serviceBusClient.CreateSender("newuser-queue");

        var message = new ServiceBusMessage(JsonSerializer.Serialize(new { user.Email, user.FirstName, user.LastName }));
        try
        {
            await sender.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
