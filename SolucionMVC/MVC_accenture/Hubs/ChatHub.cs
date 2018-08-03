﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MVC_accenture.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Invoca al metodosaddNewMessageToPage para actualizar los clientes.
            Clients.All.addNewMessageToPage(name, message);
        }

    }

}