using Foundatio.Serializer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.BLL.Extensions
{
    public static class HttpRequestExtension
    {
        public static async Task<string> GetBodyAsync(this HttpRequest request)
        {
            //TODO: check if the body is seekable - cnseek prop -> enablebuffering method
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, Encoding.UTF8);

            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return body;
        }
    }
}
