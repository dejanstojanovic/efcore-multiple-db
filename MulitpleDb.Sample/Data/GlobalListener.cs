using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace MulitpleDb.Sample.Data
{
    public class GlobalListener : IObserver<DiagnosticListener>
    {
        private readonly GlobalCommandInterceptor _commandInterceptor = new GlobalCommandInterceptor();
        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == DbLoggerCategory.Name)
                value.Subscribe(_commandInterceptor);
        }
    }
}
