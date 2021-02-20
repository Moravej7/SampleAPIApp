using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public interface IConfigDatabase
    {
        public void AddCustomDb(IServiceCollection services, string connectionString);
    }
}
