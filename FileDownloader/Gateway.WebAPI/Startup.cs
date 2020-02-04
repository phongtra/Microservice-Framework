using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace Gateway.WebAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public IWebHostEnvironment Environment { get; private set; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true; //Add this line
            //Rewrite Rules
            services.Configure<RewriteOptions>(options =>
            {
                
                //Example here: https://github.com/aspnet/AspNetCore/blob/master/src/Middleware/Rewrite/sample
                options
//                        .AddRedirect("(.*)/$", "$1")
//                        .AddRewrite(@"app/(\d+)", "app?id=$1", skipRemainingRules: false)
//                        .AddRedirectToHttps(302, 5001)
//                        .AddIISUrlRewrite(Environment.ContentRootFileProvider, "RewriteRules/UrlRewrite.xml")
//                        .AddApacheModRewrite(Environment.ContentRootFileProvider, "Rewrite.txt")
                    .Add(new ForwardProxyRewriteRule());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsPolicyBuilder => corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var key = Encoding.ASCII.GetBytes("THIS_IS_A_RANDOM_SECRET_2e7a1e80-16ee-4e52-b5c6-5e8892459259");
            services.AddAuthentication()
                .AddJwtBearer("ApiSecurity", x =>
                {
                    x.Authority = "http://localhost:2100";
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                });
            //var authenticationProviderKey = "TestKey";
            //Action<IdentityServerAuthenticationOptions> opt = o =>
            //{
            //    o.Authority = "http://localhost:2100";
            //    o.ApiName = "IdentityServer";
            //    o.SupportedTokens = SupportedTokens.Both;
            //    o.RequireHttpsMetadata = false;
            //};
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer("ApiSecurity", x =>
            //    {
            //        x.RequireHttpsMetadata = false;
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = false,
            //            ValidateAudience = false
            //        };

            //    }).AddIdentityServerAuthentication(authenticationProviderKey, opt);
            services.AddOcelot().AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Attention: Order impacts

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseWebSockets();

            app.UseRewriter();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapGet("/", async context =>
//                {
//                    await context.Response.WriteAsync("");//Hide root
//                });
//            });

            var configuration = new OcelotPipelineConfiguration
            {
                PreQueryStringBuilderMiddleware = async (ctx, next) =>
                {
                    await next.Invoke();
                    //add code here because all middleware as run at this point and the request is going to go back up the pipeline.
                                    
                }
            };
            app.UseOcelot(configuration).Wait();

        }
    }
}
