﻿// <copyright file="HttpContextHelper.cs" company="WavePoint Co. Ltd.">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>

using Microsoft.Net.Http.Headers;
using OnlineSales.Interfaces;

namespace OnlineSales.Infrastructure
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public HttpRequest Request => this.httpContextAccessor?.HttpContext?.Request!;

        public string? IpAddress => this.httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        public string? UserAgent => this.httpContextAccessor?.HttpContext?.Request?.Headers[HeaderNames.UserAgent];

        public string? IpAddressV4 => this.httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();

        public string? IpAddressV6 => this.httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv6().ToString();
    }
}