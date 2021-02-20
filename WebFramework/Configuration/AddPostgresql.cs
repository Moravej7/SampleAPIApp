using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration
{
    public class AddPostgresql : IConfigDatabase
    {
        public void AddCustomDb(IServiceCollection services, string connectionString)
        {
            //add postgresql
            throw new NotImplementedException();
        }
    }
}
