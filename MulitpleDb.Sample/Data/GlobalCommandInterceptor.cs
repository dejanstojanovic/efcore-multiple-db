using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;

namespace MulitpleDb.Sample.Data
{
    public class CommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;
                command.CommandText = command.CommandText.Replace(
                    "[Database2.dbo].",
                    "[Database2].[dbo].");
            }
        }
    }
}
