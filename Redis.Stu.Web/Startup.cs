using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Stu.Model;
using StackExchange.Redis;
using Redis.Stu.Repository;
using Redis.Stu.Repository.Redis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Redis.Stu.Common;


namespace Redis.Stu.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly ILoggerFactory _loggerFactory;
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            
            _env = env;
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //var logger = _loggerFactory.CreateLogger<Startup>();

            //logger.LogWarning(Configuration["mode"]);

            //if (_env.IsDevelopment())
            //{
            //    logger.LogInformation("这里是开发环境");
            //}
            //else
            //{
            //    logger.LogInformation("当前的环境是：" + _env.EnvironmentName);
            //}

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .AddJsonOptions(option => {
                    option.SerializerSettings.ReferenceLoopHandling = AppSetting.JsonSetting.ReferenceLoopHandling;
                    option.SerializerSettings.DateFormatString = AppSetting.JsonSetting.DateFormatString;
                    option.SerializerSettings.ContractResolver = AppSetting.JsonSetting.ContractResolver;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // 
            //services.AddDbContext<SchoolDbContext>(options => options.UseSqlServer(Configuration["connectionStrings:sqlConn"]));
            services.AddDbContext<SchoolDbContext>(options => options.UseMySQL(Configuration["connectionStrings:sqlConn"]));

            services.AddDistributedRedisCache(options =>
            {
                // 使用Redis记录Session
                options.Configuration = Configuration["connectionStrings:redisConn"];
                options.InstanceName = "RedisStu";
            });
            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
            });
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration["connectionStrings:redisConn"]));


            services.AddScoped<IStudentRepository, StudentRedisRepository>(a => new StudentRedisRepository(a.GetService<IConnectionMultiplexer>(), new StudentSqlRepository(a.GetService<SchoolDbContext>())));
            services.AddScoped<IGradeRepository, GradeRedisRepository>(a => new GradeRedisRepository(a.GetService<IConnectionMultiplexer>(), new GradeSqlRepository(a.GetService<SchoolDbContext>())));
            services.AddScoped<ICourseRepository, CourseRedisRepository>(a => new CourseRedisRepository(a.GetService<IConnectionMultiplexer>(), new CourseSqlRepository(a.GetService<SchoolDbContext>())));

            //services.AddScoped(typeof(IStudentRepository), a => new StudentRedisRepository(a.GetService<IConnectionMultiplexer>(), new StudentSqlRepository(a.GetService<SchoolDbContext>())));
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Index}/{id?}");
            });
        }
    }
}
