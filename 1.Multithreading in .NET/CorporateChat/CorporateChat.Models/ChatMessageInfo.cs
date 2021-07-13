using System;

namespace CorporateChat.Models
{
    [Serializable]
    public class ChatMessageInfo
    {
        public string ClientName { get; set; }

        public string Message { get; set; }
    }
}
