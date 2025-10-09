namespace MBA.Educacao.Online.API.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite"))
                );

                builder.Services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite"))
                );
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

                builder.Services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
            }
        }
    }
}

