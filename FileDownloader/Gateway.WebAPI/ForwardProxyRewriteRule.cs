using System;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace Gateway.WebAPI
{
    public class ForwardProxyRewriteRule : Microsoft.AspNetCore.Rewrite.IRule
    {
        public int StatusCode { get; } = (int) HttpStatusCode.RedirectKeepVerb;
        public bool ExcludeLocalhost { get; set; } = true;

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
//            var host    = request.Host;
//            var newPath = string.Empty;

            if (request.Path.StartsWithSegments("/explore", StringComparison.OrdinalIgnoreCase))
            {
                var newPath =  request.Scheme + "://" + request.Path.Value.Substring("/explore".Length) + request.QueryString;
                var response = context.HttpContext.Response;
                response.StatusCode                    = StatusCode;
                response.Headers[HeaderNames.Location] = newPath;
                context.Result = RuleResult.EndResponse; // Do not continue processing the request

                return;
            }

            context.Result = RuleResult.ContinueRules;

//            if (host.Host.StartsWith("www", StringComparison.OrdinalIgnoreCase))
//            {
//                context.Result = RuleResult.ContinueRules;
//                return;
//            }
//
//            if (ExcludeLocalhost && string.Equals(host.Host, "localhost", StringComparison.OrdinalIgnoreCase))
//            {
//                context.Result = RuleResult.ContinueRules;
//                return;
//            }
//
//            string newPath = request.Scheme + "://www." + host.Value + request.PathBase + request.Path + request.QueryString;
//
//            var response = context.HttpContext.Response;
//            response.StatusCode                    = StatusCode;
//            response.Headers[HeaderNames.Location] = newPath;
//            context.Result                         = RuleResult.EndResponse; // Do not continue processing the request
        }
    }
}
