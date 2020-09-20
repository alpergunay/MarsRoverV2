﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Hb.MarsRover.Infrastructure.Core.Http.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetClientIpAddress(this HttpRequest request)
        {
            return request?.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
        }
    }
}