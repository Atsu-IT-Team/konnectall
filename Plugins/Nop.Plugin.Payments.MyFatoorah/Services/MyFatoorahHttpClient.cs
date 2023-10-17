// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.Payments.MyFatoorah.Services.MyFatoorahHttpClient
// Assembly: Nop.Plugin.Payments.MyFatoorah, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null
// MVID: 74EEC5F1-0E85-4A72-B69A-76B24BEA288E
// Assembly location: C:\Users\USER\Downloads\Nop.Plugin.Payments.MyFatoorah_main_4.5\Payments.MyFatoorah\Nop.Plugin.Payments.MyFatoorah.dll

using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Nop.Core.Caching;
using Nop.Core.Configuration;
using Nop.Plugin.Payments.MyFatoorah.Models;
using Nop.Plugin.Payments.MyFatoorah.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


#nullable enable
namespace Nop.Plugin.Payments.MyFatoorah.Services
{
    public class MyFatoorahHttpClient
    {
        private readonly
#nullable disable
        MyFatoorahPaymentSettings _myFatoorahPaymentSettings;
        private HttpClient _httpClient;
        private static readonly object padlock = new object();
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly AppSettings _appSettings;
        private CacheKey _mfConfigCatchKey;

        public HttpClient httpClient
        {
            get
            {
                if (this._httpClient == null)
                {
                    lock (MyFatoorahHttpClient.padlock)
                    {
                        if (this._httpClient == null)
                            this._httpClient = new HttpClient();
                    }
                }
                return this._httpClient;
            }
        }

        public MyFatoorahHttpClient(
          HttpClient client,
          MyFatoorahPaymentSettings myFatoorahPaymentSettings,
          AppSettings appSettings,
          IStaticCacheManager staticCacheManager)
        {
            this._myFatoorahPaymentSettings = myFatoorahPaymentSettings;
            this._appSettings = appSettings;
            this._staticCacheManager = staticCacheManager;
            client.Timeout = TimeSpan.FromSeconds(20.0);
            client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "nopCommerce-4.50");
            if (!string.IsNullOrEmpty(this._myFatoorahPaymentSettings.APIToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._myFatoorahPaymentSettings.APIToken);
            this._httpClient = client;
            this._mfConfigCatchKey = new CacheKey("MFConfig", Array.Empty<string>())
            {
                CacheTime = this._appSettings.Get<CacheConfig>().DefaultCacheTime
            };
        }

        private async Task SetBaseAddress(HttpClient client)
        {
            MyFatoorahHttpClient fatoorahHttpClient = this;
            Dictionary<string, ConfigData> mfConfigs = await _staticCacheManager.GetAsync<Dictionary<string, ConfigData>>(fatoorahHttpClient._mfConfigCatchKey, (Func<Dictionary<string, ConfigData>>)(() => (Dictionary<string, ConfigData>)null));
            if (mfConfigs == null)
            {
                using (HttpClient clnt = new HttpClient())
                {
                    HttpResponseMessage async = await clnt.GetAsync("https://portal.myfatoorah.com/Files/API/mf-config.json");
                    if (async.IsSuccessStatusCode)
                    {
                        mfConfigs = JsonConvert.DeserializeObject<Dictionary<string, ConfigData>>(await async.Content.ReadAsStringAsync());
                        await fatoorahHttpClient._staticCacheManager.SetAsync(_mfConfigCatchKey, (object)mfConfigs);
                    }
                }
            }
            if (mfConfigs == null)
            {
                mfConfigs = (Dictionary<string, ConfigData>)null;
            }
            else
            {
                // ISSUE: reference to a compiler-generated method
                //KeyValuePair<string, ConfigData> keyValuePair = mfConfigs.FirstOrDefault<KeyValuePair<string, ConfigData>>(new Func<KeyValuePair<string, ConfigData>, bool>(fatoorahHttpClient.SetBaseAddress(_httpClient)));
                KeyValuePair<string, ConfigData> keyValuePair = mfConfigs.FirstOrDefault<KeyValuePair<string, ConfigData>>();
                client.BaseAddress = new Uri(fatoorahHttpClient._myFatoorahPaymentSettings.UseSandbox ? keyValuePair.Value?.testv2 : keyValuePair.Value?.v2);
                mfConfigs = (Dictionary<string, ConfigData>)null;
            }
        }

        public async Task<SendPaymentResponse> CreateInvoiceAsync(SendPaymentRequest request)
        {
            string content = JsonConvert.SerializeObject((object)request);
            string url = "v2/" + APICallEnum.SendPayment.ToString();
            StringContent requestContent = new StringContent(content, Encoding.UTF8, "application/json");
            if (ServicePointManager.SecurityProtocol == (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            await this.SetBaseAddress(this._httpClient);
            HttpResponseMessage response = await this._httpClient.PostAsync(url, (HttpContent)requestContent);
            if (!response.IsSuccessStatusCode)
            {
                SendPaymentResponse invoiceAsync = new SendPaymentResponse();
                SendPaymentResponse sendPaymentResponse = invoiceAsync;
                sendPaymentResponse.Message = await response.Content.ReadAsStringAsync();
                return invoiceAsync;
            }
            response.EnsureSuccessStatusCode();
            SendPaymentResponse invoiceAsync1 = JsonConvert.DeserializeObject<SendPaymentResponse>(await response.Content.ReadAsStringAsync());
            if (invoiceAsync1 == null)
                invoiceAsync1 = new SendPaymentResponse()
                {
                    Message = response.ReasonPhrase
                };
            return invoiceAsync1;
        }

        public async Task<GetPaymentStatusResponse> GetPaymentStatusAsync(string paymentId)
        {
            string url = "v2/" + APICallEnum.GetPaymentStatus.ToString();
            StringContent requestContent = new StringContent(JsonConvert.SerializeObject((object)new
            {
                Key = paymentId,
                KeyType = "PaymentId"
            }), Encoding.UTF8, "application/json");
            if (ServicePointManager.SecurityProtocol == (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            await this.SetBaseAddress(this._httpClient);
            HttpResponseMessage response = await this._httpClient.PostAsync(url, (HttpContent)requestContent);
            if (!response.IsSuccessStatusCode)
            {
                GetPaymentStatusResponse paymentStatusAsync = new GetPaymentStatusResponse();
                GetPaymentStatusResponse paymentStatusResponse = paymentStatusAsync;
                paymentStatusResponse.Message = await response.Content.ReadAsStringAsync();
                return paymentStatusAsync;
            }
            response.EnsureSuccessStatusCode();
            GetPaymentStatusResponse paymentStatusAsync1 = JsonConvert.DeserializeObject<GetPaymentStatusResponse>(await response.Content.ReadAsStringAsync());
            if (paymentStatusAsync1 == null)
                paymentStatusAsync1 = new GetPaymentStatusResponse()
                {
                    Message = response.ReasonPhrase
                };
            return paymentStatusAsync1;
        }
    }
}
