using Microsoft.AspNetCore.Builder;

namespace ConceptArchitect.Web
{
    public static class MiddlewareExtensions
    {
        public static WebApplication  UseOnUrl(this WebApplication app, string url, RequestDelegate urlAction, bool exactMatch=false)
        {
            app.Use(async (context, next) =>
            {
                if ((exactMatch && context.Request.Path == url) || context.Request.Path.ToString().Contains(url))
                {
                    await urlAction(context);
                }
                else
                    await next(context);
            });

            return app;
        }

        public static WebApplication UseUrlResponse<T>(this WebApplication app, string url, Func<HttpContext,T> responseDataProvider, bool exactMatch=false)
        {
            return app.UseOnUrl(url, async context =>
            {
                var data = responseDataProvider(context);
                await context.Response.WriteAsync(data.ToString());
            },exactMatch);
        }

        public static WebApplication UseSimpleError(this WebApplication app,bool showExceptionMessage=false)
        {
            app
               .Use(async (context, next) =>
               {
                   try
                   {
                       await next(context);
                       Console.WriteLine("Thank God!!! There was no problem.");
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine("Thank God!!! we managed the error");
                       context.Response.StatusCode = 500; //internal server error
                       if(showExceptionMessage)
                       {
                           var message = ex.Message
                                                .Replace("/error/", "")
                                                .Replace("/", " ")
                                                .Replace("-", " ");

                           await context.Response.WriteAsync($"error: {message}");
                       }
                       else
                            await context.Response.WriteAsync($"Some Error occured. We are working on it");
                   }
               });

            return app;
        }
 
        public static WebApplication UseReferer(this WebApplication app, string baseUrl,Func<HttpContext,RequestDelegate,Task> middleware, string expectedReferer=null)
        {   
            app.Use( async (context,next) =>
            {
                if(!context.Request.Path.ToString().StartsWith(baseUrl))
                {
                    await next(context);
                    return;
                }    

                bool allowRequest = false;

                if(context.Request.Headers.ContainsKey("REFERER"))
                {   
                      allowRequest=expectedReferer==null ||  context.Request.Headers["REFERER"]==expectedReferer;
                }
                
                if(allowRequest)
                    await middleware(context,next);
                else
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync($"You are NOT permitted to access the resource");
                }

            });

            return app;
        }
    

    public static WebApplication UseStaticResource(this WebApplication app)
        {
            
            var appPath= app.Environment.ContentRootPath;

            
            
            app.Use(async (context,next)=>{
                var resourcePath = $"{appPath}{context.Request.Path.ToString()}";
                resourcePath = resourcePath.Replace("\\", "/");
                System.Console.WriteLine( resourcePath  );

                if(File.Exists(resourcePath))
                {
                    var reader= new StreamReader(resourcePath);
                    string str = "";
                    while(!reader.EndOfStream)
                    {
                        str += (char)reader.Read();
                    }
                    reader.Close();
                    await context.Response.WriteAsync(str);
                }
                else
                    await next(context);
            });

            return app;
        }


        public static WebApplication UseDeveloperMode(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    await context.Response.WriteAsync($"{ex.GetType().Name}:{ex.Message}\n");
                    await context.Response.WriteAsync(ex.StackTrace);
                }
            });

            return app;
        }



    }


 
}