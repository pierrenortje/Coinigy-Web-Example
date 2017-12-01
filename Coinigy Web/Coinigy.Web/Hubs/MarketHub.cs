using Coinigy.API;
using Microsoft.AspNet.SignalR;
using System.Threading;

namespace Coinigy.Web.Hubs
{
    public class MarketHub : Hub
    {
        #region Private Fields
        private readonly Websocket socket = null;
        private readonly object obj = new object();

        private bool wait = false;
        #endregion

        #region Constructor
        public MarketHub()
        {
            socket = new Websocket(new API.Models.ApiCredentials
            {
                ApiKey = "[YOUR-API-KEY]",
                ApiSecret = "[YOUR-API-SECRET]"
            });

            socket.OnClientReady += Socket_OnClientReady;
            socket.OnTradeMessage += Socket_OnTradeMessage;
            socket.OnError += Socket_OnError;

            socket.Connect();
        }
        #endregion

        #region Private Methods
        private void Socket_OnClientReady()
        {
            socket.SubscribeToTradeChannel("BMEX", "XBT", "USD");
        }

        private void Socket_OnTradeMessage(string exchange, string primaryCurrency, string secondaryCurrency, API.Models.TradeItem trade)
        {
            lock (obj)
            {
                if (this.wait)
                {
                    // Only send updates every second
                    Thread.Sleep(1000);
                    this.wait = false;
                }

                Clients.All.broadcastPrice(exchange, primaryCurrency, trade.Price);
                this.wait = true;
            }
        }

        private void Socket_OnError(System.Exception ex)
        {
            Clients.All.onError(ex.Message);
        }
        #endregion
    }
}
