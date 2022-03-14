using DataAcccessLayer.InmemoryDataStore;
using DataAcccessLayer.Models;
using DataAcccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAcccessLayer.Repositories.Implementations
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly IDateStore dateStore;

        public ChatSessionRepository(IDateStore dateStore)
        {
            this.dateStore = dateStore;
        }
        public ChatSession InsertNewSession(ChatSession chatSession)
        {
            this.dateStore.AddSession(chatSession);
            return chatSession;
        }

        public bool UpdateSession(ChatSession session)
        {
            this.dateStore.UpdateChatSession(session);
            return true;
        }

        public int GetCurrentActiveSessionCount() 
        {
            return this.dateStore.GetActiveChatCount();
        }
    }

}
