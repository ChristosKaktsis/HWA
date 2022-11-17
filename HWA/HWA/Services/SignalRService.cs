using HWA.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HWA.Services
{
    public class SignalRService
    {
        private HubConnection hubConnection;
        public bool IsConnected =>
        hubConnection?.State == HubConnectionState.Connected;
        private string _url = string.Empty;
        public SignalRService(string url)
        {
            try
            {
                _url = url;
                hubConnection = new HubConnectionBuilder()
                    .WithUrl($"http://{_url}:7110/Chat")
                    .WithAutomaticReconnect()
                    .Build();
                hubConnection.Closed += HubConnection_Closed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            Console.WriteLine("ConnectionClosed!!!!");
            await Task.Delay(1000);
        }

        public async Task ConnectToHub()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("AddToGroup", App.CurrentConnectedUser);
        }
        public async Task DisconnectToHub()
        {
            await hubConnection.StopAsync();
        }
        public async Task SendMessage(User user, string message)
        {
            await hubConnection.InvokeAsync("SendPrivateMessage", user.ID, message);
        }
        public async Task CallUser(string cid)
        {
            await hubConnection.InvokeAsync("CallUser", cid);
        }
        public async Task AcceptCall(string cid)
        {
            await hubConnection.InvokeAsync("AcceptCall", cid);
        }
        public async Task RejectCall(string cid)
        {
            await hubConnection.InvokeAsync("RejectCall", cid);
        }
        public async Task EndCall(string cid)
        {
            await hubConnection.InvokeAsync("EndCall", cid);
        }
        public void ReceiveId(Action<string> action)
        {
            hubConnection.On<string>("ReceiveId", action);
        }
        public void ReceiveMessage(Action<string> action)
        {
            hubConnection.On<string>("ReceiveMessage", action);
        }
        public void ReceiveConnectedUsers(Action<User> action)
        {
            hubConnection.On<User>("AddUser", action);
        }
        public void ReceiveCall(Action<string> action)
        {
            hubConnection.On<string>("ReciveCall", action);
        }
        public void OnAcceptCall(Action<string> action)
        {
            hubConnection.On<string>("Accept", action);
        }
        public void OnRejectCall(Action action)
        {
            hubConnection.On("Reject", action);
        }
        public void ReceiveDisconnectedUser(Action<string> action)
        {
            hubConnection.On<string>("RemoveUser", action);
        }
        public void OnEndCall(Action action)
        {
            hubConnection.On("End", action);
        }
    }
}
