using Financial.Infrastructure.EFDataPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Services.EFImplementation.Shared
{
    public class BaseEFHandler
    {
        protected ChatContext _chatContext;

        protected BaseEFHandler(ChatContext chatContext)
        {
            this._chatContext = chatContext;
        }
    }
}
