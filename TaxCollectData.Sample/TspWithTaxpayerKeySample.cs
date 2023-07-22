using Microsoft.Extensions.DependencyInjection;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Api;
using TaxCollectData.Library.Business.Normalize;
using TaxCollectData.Library.Business.Signatory;
using TaxCollectData.Library.Constants;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Dto.Transfer;
using TaxCollectData.Library.Enums;
using TaxCollectData.Library.Extensions;

namespace TaxCollectData.Sample;

public class TspWithTaxpayerKeySample
{
    private readonly ITaxApis _api;
    private readonly ITransferApi _transferApi;
    private readonly SampleInvoiceCreator _sampleInvoiceCreator = new();
    private readonly IProperties _properties = new NormalProperties(ClientType.SELF_TSP);
    
    private string token;
    private string referenceNumber;
    
    private const string TSP_PRIVATE_KEY = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCpwICuYxXmQouD7InTS6u6tYT/iA5gc+pROOqMQfwBs9NCqGTfN+cb4ExdQjwQY/J120TQUKDYgSd8LOhvwiTvcqCnwiWUYVX6bBwhW0wDgWYupfFKd0vE6WgqDp36Eg5X37MqsdBqzjtlTmUAxp+KRcMJP4yf1HilDcT98SHt5BuHE4COi20x9swEr40BKDiVApAh8lCinZMEmURMNKmOVmWeIrhO82BvGuQhn4RwGgdrJTymGDMfKuaT00jszP3r3IhHxELfRE4KNcesr2pdYgrB7otA6hW/YfYeLtrPPA2FoC+QM7fi9WVBTAUirWRgqHXjs9aql07VhUNA/fwRAgMBAAECggEBAIC37xvNGMsMhLxZfb1SMPsYL6yQX851tyU10mzekBg+YqC2Dh9RRZbWwzEoS2FmWHFT/l8z9HOXo/g+GVa9UcKcGgR7bIGSOV714XLNxtsVoQUYYdpEkZjUIF9bzCDW1jd98l8ajF6g2VsdUet/sXRpJ9Z4cMq1k3Ic+dZRpZe71N+P5mLTQo250/Eh8i3R3jbhVZ5mYABaHVObwsupgkL1XAQQ/on/gkR3GFCvQ4UGSUT3kvoACln5as7w8g7mBtsf8fYudTDi5ANSwhZKkkq+lN1XgPqibkK1KyZjgUUJPD/w62EJgo2+n4ELzjR4LQ8yAUtZH90OujpbSQoLugECgYEA7bUsxbRKNVXw6wGUhpsyzZ1GwlYwbyvAH+TVQY4e4NmGpF1LhgRuH58pIULTYWLwi1Qu9PJPiKQzJKiwQl1XClfCd5Ax/bz+TGM4LoGHT3J3RrCytbinwcRguEdARIxmOczr36aSRg2B1EaSZRw4f+W0VfwWe1NLa+EVhhKiUqECgYEAttCas0a287i+eg5UwrMQn1FZoB/xI0fCg7qCQtGjaZkqciKxxfzTlZg0QHPtmst2OAR5cNat9DkPyj3Jm3KYzVgINxWP5LE67nEJKUDu+QzxXz0oc+rPEhUv9JVdztYcoxSylbRPD1+SC8feX8KB4waSg09Yw7aLD+fdRt3bo3ECgYANFxINHR2NDFuFBYRBWWCV1BhmcUqfhKBC4V4hCwuGRFRWztBu4+WQljo0m7J6RXGLqqofUQyyMKCkXym9rdgyOJz76pPmLjcuy7P3U6i45kvNN8PPoAiU8hSFcV8Hp1elzTQcD9c0RmAk6XH8YW53FJY2ufge+HpJyY5e4L3RoQKBgGIhqdjv7Yb4NS65gKAelJ6ggValrr/8ZhplxERv2aL2d8VagEjBxSW41NuAHxwHeUbqh/GdGzVMaABsmErcAc/AIHDJHztoMzUI8hz14RFI9EvmPU+zzEFtpBdLElbTsf2MP6yCZCnhaDjwqZqHg0dFIOJhdklgNFduY+YFcz/RAoGBALk8jDW+7sZ7CUPBY+ddAty6XhOfbQSMuTu0tLlTzOI+TspV/0GLlyC6lumuJxBGqEXMdClp3i2T7YxMdtR+AKfkI4RideIkIb5Xv6Avyn9UWj3tY37xQv8M6lyIAeU7E+vVYlyLaSA0FNLe7V8ZXauT4IUSHudmUVrqEc1D97TV";
    private const string TAXPAYER_PRIVATE_KEY = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCpwICuYxXmQouD7InTS6u6tYT/iA5gc+pROOqMQfwBs9NCqGTfN+cb4ExdQjwQY/J120TQUKDYgSd8LOhvwiTvcqCnwiWUYVX6bBwhW0wDgWYupfFKd0vE6WgqDp36Eg5X37MqsdBqzjtlTmUAxp+KRcMJP4yf1HilDcT98SHt5BuHE4COi20x9swEr40BKDiVApAh8lCinZMEmURMNKmOVmWeIrhO82BvGuQhn4RwGgdrJTymGDMfKuaT00jszP3r3IhHxELfRE4KNcesr2pdYgrB7otA6hW/YfYeLtrPPA2FoC+QM7fi9WVBTAUirWRgqHXjs9aql07VhUNA/fwRAgMBAAECggEBAIC37xvNGMsMhLxZfb1SMPsYL6yQX851tyU10mzekBg+YqC2Dh9RRZbWwzEoS2FmWHFT/l8z9HOXo/g+GVa9UcKcGgR7bIGSOV714XLNxtsVoQUYYdpEkZjUIF9bzCDW1jd98l8ajF6g2VsdUet/sXRpJ9Z4cMq1k3Ic+dZRpZe71N+P5mLTQo250/Eh8i3R3jbhVZ5mYABaHVObwsupgkL1XAQQ/on/gkR3GFCvQ4UGSUT3kvoACln5as7w8g7mBtsf8fYudTDi5ANSwhZKkkq+lN1XgPqibkK1KyZjgUUJPD/w62EJgo2+n4ELzjR4LQ8yAUtZH90OujpbSQoLugECgYEA7bUsxbRKNVXw6wGUhpsyzZ1GwlYwbyvAH+TVQY4e4NmGpF1LhgRuH58pIULTYWLwi1Qu9PJPiKQzJKiwQl1XClfCd5Ax/bz+TGM4LoGHT3J3RrCytbinwcRguEdARIxmOczr36aSRg2B1EaSZRw4f+W0VfwWe1NLa+EVhhKiUqECgYEAttCas0a287i+eg5UwrMQn1FZoB/xI0fCg7qCQtGjaZkqciKxxfzTlZg0QHPtmst2OAR5cNat9DkPyj3Jm3KYzVgINxWP5LE67nEJKUDu+QzxXz0oc+rPEhUv9JVdztYcoxSylbRPD1+SC8feX8KB4waSg09Yw7aLD+fdRt3bo3ECgYANFxINHR2NDFuFBYRBWWCV1BhmcUqfhKBC4V4hCwuGRFRWztBu4+WQljo0m7J6RXGLqqofUQyyMKCkXym9rdgyOJz76pPmLjcuy7P3U6i45kvNN8PPoAiU8hSFcV8Hp1elzTQcD9c0RmAk6XH8YW53FJY2ufge+HpJyY5e4L3RoQKBgGIhqdjv7Yb4NS65gKAelJ6ggValrr/8ZhplxERv2aL2d8VagEjBxSW41NuAHxwHeUbqh/GdGzVMaABsmErcAc/AIHDJHztoMzUI8hz14RFI9EvmPU+zzEFtpBdLElbTsf2MP6yCZCnhaDjwqZqHg0dFIOJhdklgNFduY+YFcz/RAoGBALk8jDW+7sZ7CUPBY+ddAty6XhOfbQSMuTu0tLlTzOI+TspV/0GLlyC6lumuJxBGqEXMdClp3i2T7YxMdtR+AKfkI4RideIkIb5Xv6Avyn9UWj3tY37xQv8M6lyIAeU7E+vVYlyLaSA0FNLe7V8ZXauT4IUSHudmUVrqEc1D97TV";
    private const string CLIENT_ID = "102";
    private const string MEMORY_ID = "A111R1";

    public TspWithTaxpayerKeySample()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTaxApi(
            "https://wantolan.ir/requestsmanager/api",
            CLIENT_ID,
            _properties,
            new SignatoryConfig(TSP_PRIVATE_KEY, null),
            new SignatoryConfig(TAXPAYER_PRIVATE_KEY, null)
        );
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _api = serviceProvider.GetService<ITaxApis>() ?? throw new InvalidOperationException();
        _transferApi = serviceProvider.GetService<ITransferApi>() ?? throw new InvalidOperationException();
    }
    
    public async Task RunAsync()
    {
        _api.GetServerInformation();
        GetToken();
        await SendInvoicesAsync().ConfigureAwait(false);
        Thread.Sleep(6000);
        InquiryByReferenceId();

    }

    public void GetServerInformation() {
        var serverInformation = _api.GetServerInformation();
        Console.WriteLine("success load server information: " + serverInformation);
    }

    private void GetToken() {
        var token = _api.RequestToken();
        if (token?.Token != null && token.Token.Length > 0)
        {
            this.token = token.Token;
            Console.WriteLine("success login, token: " + token.Token);
        } else {
            Console.WriteLine("error in login");
        }
    }

    private async Task SendInvoicesAsync() {

        var invoiceDto = _sampleInvoiceCreator.Create(MEMORY_ID).First();
        
        var normalizer = new ObjectNormalizer(new());
        var normalize = normalizer.Normalize(invoiceDto, null);

        var signatory = new InMemorySignatory(new(TAXPAYER_PRIVATE_KEY, null));
        var sign = signatory.Sign(normalize);

        var packetDto = new PacketDto<InvoiceDto>(Guid.NewGuid().ToString(),
            PacketTypeConstants.InvoiceV01,
            MEMORY_ID,
            invoiceDto,
            false,
            null,
            null,
            null,
            sign,
            null
            );
        
        var headers = new Dictionary<string, string>();
        if (token != null) {
            headers.Add(TransferConstants.AuthorizationHeader, "Bearer " + token);
        }
        var responseModel = await _transferApi.SendPacketsAsync<InvoiceDto>(new(){packetDto},
            _properties.SendInvoiceApiAddress,
            headers,
            true,
            false);
        if (responseModel?.Body?.Result is not null && responseModel.Body.Result.Any()) {
            Console.WriteLine("success send invoice, response" + responseModel.Body.Result);
            var packetResponse = responseModel.Body.Result.First();
            referenceNumber = packetResponse.ReferenceNumber;
        } else {
            Console.WriteLine(responseModel?.Body?.Errors);
        }
    }
    
    private void InquiryByReferenceId() {
        var inquiryResultModels = _api.InquiryByReferenceId(new(){referenceNumber});
        Console.WriteLine("inquiry By Reference Id result: " + string.Join(", ", inquiryResultModels));
    }
}