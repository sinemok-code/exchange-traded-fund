using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Configuration;
using ETF.Web.Common;
using ETF.Web.Common.Exceptions;
using ETF.Web.Repository.Interfaces;
using Newtonsoft.Json;
using HttpMethod = ETF.Web.Repository.Interfaces.HttpMethod;

namespace ETF.Web.Repository.DataAccess
{
    public class HttpHelper : IApiHelper
    {
        private const string PayLoadHeaderKey = "api-entity-payload";
        private const string JsonContentType = "application/json";

        private readonly string webPath;

        private readonly string baseAddress;

        private readonly HttpMethod httpMethod;

        private readonly bool multiPart;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">API path.</param>
        /// <param name="httpMethod">Type of HTTP request to send.</param>
        /// <param name="multiPart">True if sending files to API.</param>
        public HttpHelper(string path, HttpMethod httpMethod = HttpMethod.Get, bool multiPart = false)
        {
            this.webPath = path;
            this.baseAddress = Reference.Dictionary.ApiUri;
            this.httpMethod = httpMethod;
            this.multiPart = multiPart;
        }

        /// <summary>
        /// Sends a HTTP request, waits for the response and deserialises it to the specified type.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="args">Query string arguments (use Encode() extension method for any argument that needs to be encoded). (Optional)</param>
        /// <param name="content">Request content. (Optional)</param>
        /// <returns>Response.</returns>
        public T GetResponse<T>(object[] args = null, object content = null)
        {
            HttpClient client = null;
            HttpContent httpContent = null;

            try
            {
                HttpResponseMessage response = null;

                client = this.GetClient();

                // GetDateTimeRoundTripValue called to convert parameters with type of DateTime to round-trip date/time string
                var url = args == null ? this.webPath : string.Format(this.webPath, args.Select(GetDateTimeRoundTripValue).ToArray());

                // add app_id and app_key to end of url for all requests
                var append = url.IndexOf("?", StringComparison.Ordinal) != -1 ? "&" : "?";
                url += append;

                if (content != null)
                {
                    httpContent = this.multiPart ? (HttpContent)BuildMultiPartFormContent((IEnumerable<HttpPostedFileBase>)content) : JsonSerialise(content);
                }

                switch (this.httpMethod)
                {
                    case HttpMethod.Get:
                        response = client.GetAsync(url).Result;

                        break;

                    case HttpMethod.Post:
                        response = client.PostAsync(url, httpContent).Result;

                        break;

                    case HttpMethod.Put:
                        response = client.PutAsync(url, httpContent).Result;

                        break;

                    case HttpMethod.Delete:
                        var message = new HttpRequestMessage(new System.Net.Http.HttpMethod("Delete"), url) { Content = httpContent };

                        response = client.SendAsync(message).Result;

                        break;
                }

                if (response == null)
                {
                    throw new AppException(message: "HTTP Helper received no response.");
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;

                    case HttpStatusCode.NotFound:
                        throw new AppException(Reference.StatusCode.NotFound);

                    case HttpStatusCode.Forbidden:
                        throw new AppException(Reference.StatusCode.Forbidden);

                    default:
                        throw new AppException(message: "API response: " + response.ReasonPhrase);
                }

                if (response.Content.Headers.ContentType.MediaType != JsonContentType)
                {
                    throw new AppException(message: "Content type of response received by HTTP Helper was not JSON.");
                }

                string responseContent;

                SetPayloadHeader(response);

                if (response.Content.Headers.ContentEncoding.Contains("gzip")
                    || response.Content.Headers.ContentEncoding.Contains("deflate"))
                {
                    using (var responseStream = response.Content.ReadAsStreamAsync())
                    {
                        var decompressedStream = new GZipStream(responseStream.Result, CompressionMode.Decompress);

                        using (var streamReader = new StreamReader(decompressedStream))
                        {
                            responseContent = streamReader.ReadToEnd();
                        }
                    }
                }
                else
                {
                    // response is not compressed
                    responseContent = response.Content.ReadAsStringAsync().Result;
                }

                var val = (T)
                    JsonConvert.DeserializeObject(
                        responseContent,
                        typeof(T),
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            Binder = new TypeNameSerializationBinder()
                        });

                return val;
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }

                if (httpContent != null)
                {
                    httpContent.Dispose();
                }
            }
        }

        private static void SetPayloadHeader(HttpResponseMessage response)
        {
            var payload = response.Headers.Where(x => x.Key.ToLower() == PayLoadHeaderKey);
            var payloadValues = string.Join(", ", payload.SelectMany(pair => pair.Value).ToArray());

            if (!string.IsNullOrEmpty(payloadValues))
            {
                HttpContext.Current.Response.AddHeader(PayLoadHeaderKey, payloadValues);
            }
        }

        private static StringContent JsonSerialise(object content)
        {
            var json = JsonConvert.SerializeObject(
                content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None,
                    Binder = new TypeNameSerializationBinder()
                });

            return new StringContent(json, new UTF8Encoding(), JsonContentType);
        }

        private static MultipartFormDataContent BuildMultiPartFormContent(IEnumerable<HttpPostedFileBase> files)
        {
            var multiPartContent = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.InputStream);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = file.FileName };
                multiPartContent.Add(fileContent);
            }

            return multiPartContent;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient {BaseAddress = new Uri(this.baseAddress)};

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));

            return client;
        }

        /// <summary>
        /// Used to convert the variables type of DateTime to round trip string
        /// </summary>
        /// <param name="value">Parameter to be converted</param>
        /// <returns>The round-trip date/time string if argument type is DateTime, else default string value</returns>
        private static string GetDateTimeRoundTripValue(object value)
        {
            if (value == null) return string.Empty;

            var date = value as DateTime?;

            return date != null ? HttpUtility.UrlEncode(date.Value.ToString("o")) : value.ToString();
        }
    }
}
