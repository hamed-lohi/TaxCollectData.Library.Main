using Microsoft.Extensions.DependencyInjection;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;
using TaxCollectData.Library.Extensions;

namespace TaxCollectData.Sample;

internal class TspWithOwnKeyV1Sample
{
    private const string ORG_PUBLIC_KEY = "MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAxdzREOEfk3vBQogDPGTMqdDQ7t0oDhuKMZkA+Wm1lhzjjhAGfSUOuDvOKRoUEQwP8oUcXRmYzcvCUgcfoRT5iz7HbovqH+bIeJwT4rmLmFcbfPke+E3DLUxOtIZifEXrKXWgSVPkRnhMgym6UiAtnzwA1rmKstJoWpk9Nv34CYgTk8DKQN5jQJqb9L/Ng0zOEEtI3zA424tsd9zv/kP4/SaSnbbnj0evqsZ29X6aBypvnTnwH9t3gbWM4I9eAVQhPYClawHTqvdaz/O/feqfm06QBFnCgL+CBdjLs30xQSLsPICjnlV1jMzoTZnAabWP6FRzzj6C2sxw9a/WwlXrKn3gldZ7Ctv6Jso72cEeCeUI1tzHMDJPU3Qy12RQzaXujpMhCz1DVa47RvqiumpTNyK9HfFIdhgoupFkxT14XLDl65S55MF6HuQvo/RHSbBJ93FQ+2/x/Q2MNGB3BXOjNwM2pj3ojbDv3pj9CHzvaYQUYM1yOcFmIJqJ72uvVf9Jx9iTObaNNF6pl52ADmh85GTAH1hz+4pR/E9IAXUIl/YiUneYu0G4tiDY4ZXykYNknNfhSgxmn/gPHT+7kL31nyxgjiEEhK0B0vagWvdRCNJSNGWpLtlq4FlCWTAnPI5ctiFgq925e+sySjNaORCoHraBXNEwyiHT2hu5ZipIW2cCAwEAAQ==";

    private const string ORG_KEY_ID = "6a2bcd88-a871-4245-a393-2843eafe6e02";

    private const string PRIVATE_KEY = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCpwICuYxXmQouD7InTS6u6tYT/iA5gc+pROOqMQfwBs9NCqGTfN+cb4ExdQjwQY/J120TQUKDYgSd8LOhvwiTvcqCnwiWUYVX6bBwhW0wDgWYupfFKd0vE6WgqDp36Eg5X37MqsdBqzjtlTmUAxp+KRcMJP4yf1HilDcT98SHt5BuHE4COi20x9swEr40BKDiVApAh8lCinZMEmURMNKmOVmWeIrhO82BvGuQhn4RwGgdrJTymGDMfKuaT00jszP3r3IhHxELfRE4KNcesr2pdYgrB7otA6hW/YfYeLtrPPA2FoC+QM7fi9WVBTAUirWRgqHXjs9aql07VhUNA/fwRAgMBAAECggEBAIC37xvNGMsMhLxZfb1SMPsYL6yQX851tyU10mzekBg+YqC2Dh9RRZbWwzEoS2FmWHFT/l8z9HOXo/g+GVa9UcKcGgR7bIGSOV714XLNxtsVoQUYYdpEkZjUIF9bzCDW1jd98l8ajF6g2VsdUet/sXRpJ9Z4cMq1k3Ic+dZRpZe71N+P5mLTQo250/Eh8i3R3jbhVZ5mYABaHVObwsupgkL1XAQQ/on/gkR3GFCvQ4UGSUT3kvoACln5as7w8g7mBtsf8fYudTDi5ANSwhZKkkq+lN1XgPqibkK1KyZjgUUJPD/w62EJgo2+n4ELzjR4LQ8yAUtZH90OujpbSQoLugECgYEA7bUsxbRKNVXw6wGUhpsyzZ1GwlYwbyvAH+TVQY4e4NmGpF1LhgRuH58pIULTYWLwi1Qu9PJPiKQzJKiwQl1XClfCd5Ax/bz+TGM4LoGHT3J3RrCytbinwcRguEdARIxmOczr36aSRg2B1EaSZRw4f+W0VfwWe1NLa+EVhhKiUqECgYEAttCas0a287i+eg5UwrMQn1FZoB/xI0fCg7qCQtGjaZkqciKxxfzTlZg0QHPtmst2OAR5cNat9DkPyj3Jm3KYzVgINxWP5LE67nEJKUDu+QzxXz0oc+rPEhUv9JVdztYcoxSylbRPD1+SC8feX8KB4waSg09Yw7aLD+fdRt3bo3ECgYANFxINHR2NDFuFBYRBWWCV1BhmcUqfhKBC4V4hCwuGRFRWztBu4+WQljo0m7J6RXGLqqofUQyyMKCkXym9rdgyOJz76pPmLjcuy7P3U6i45kvNN8PPoAiU8hSFcV8Hp1elzTQcD9c0RmAk6XH8YW53FJY2ufge+HpJyY5e4L3RoQKBgGIhqdjv7Yb4NS65gKAelJ6ggValrr/8ZhplxERv2aL2d8VagEjBxSW41NuAHxwHeUbqh/GdGzVMaABsmErcAc/AIHDJHztoMzUI8hz14RFI9EvmPU+zzEFtpBdLElbTsf2MP6yCZCnhaDjwqZqHg0dFIOJhdklgNFduY+YFcz/RAoGBALk8jDW+7sZ7CUPBY+ddAty6XhOfbQSMuTu0tLlTzOI+TspV/0GLlyC6lumuJxBGqEXMdClp3i2T7YxMdtR+AKfkI4RideIkIb5Xv6Avyn9UWj3tY37xQv8M6lyIAeU7E+vVYlyLaSA0FNLe7V8ZXauT4IUSHudmUVrqEc1D97TV";

    private const string CLIENT_ID = "102";

    private const string MEMORY_ID = "A111R1";

    private readonly ITaxApis _api;

    private readonly SampleInvoiceCreator _sampleInvoiceCreator = new();

    private string uid;
    
    private string referenceNumber;

    public TspWithOwnKeyV1Sample()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTaxApi(
            "https://wantolan.ir/requestsmanager/api",
            CLIENT_ID,
            new NormalProperties(ClientType.SELF_TSP, "v1"),
            new SignatoryConfig(PRIVATE_KEY, null)
            );
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _api = serviceProvider.GetService<ITaxApis>() ?? throw new InvalidOperationException();
    }

    public async Task RunAsync()
    {
        _api.GetServerInformation();
        GetToken();
        GetFiscalInformation();
        await SendInvoicesAsync().ConfigureAwait(false);
        Thread.Sleep(3000);
        InquiryByUid();
        InquiryByReferenceId();
        InquiryByTime();
        InquiryByTimeRange();
        Thread.Sleep(6000);
        GetFiscalInformation();
        InquiryByReferenceId();
        GetEconomicCodeInformation();

    }

    public void GetServerInformation() {
        var serverInformation = _api.GetServerInformation();
        Console.WriteLine("success load server information: " + serverInformation);
    }

    private void GetToken() {
        var token = _api.RequestToken();
        if (token?.Token != null && token.Token.Length > 0) {
            Console.WriteLine("success login, token: " + token.Token);
        } else {
            Console.WriteLine("error in login");
        }
    }

    private async Task SendInvoicesAsync()
    {
        var invoices = _sampleInvoiceCreator.Create(MEMORY_ID);
        var invoiceDtoWrappers = new List<InvoiceDtoWrapper>()
        {
            new()
            {
                FiscalId = MEMORY_ID,
                Invoice = invoices.First()
            }
        };
        var responseModel = await _api.SendTspInvoicesAsync(invoiceDtoWrappers, null).ConfigureAwait(false);
        if (responseModel?.Body?.Result != null && responseModel.Body.Result.Any()) {
            Console.WriteLine("success send invoice, response" + responseModel.Body.Result);
            var packetResponse = responseModel.Body.Result.First();
            uid = packetResponse.Uid;
            referenceNumber = packetResponse.ReferenceNumber;
        } else {
            Console.WriteLine(responseModel?.Body?.Errors);
        }
    }

    private void InquiryByUid() {
        var uidAndFiscalId = new UidAndFiscalId(uid, MEMORY_ID);
        var inquiryResultModels = _api.InquiryByUidAndFiscalId(new(){uidAndFiscalId});
        Console.WriteLine("inquiry By UID result: " + string.Join(", ", inquiryResultModels));
    }

    private void InquiryByTime() {
        var inquiryResultModels = _api.InquiryByTime("14010725");
        Console.WriteLine("inquiry By Time result: " + string.Join(", ", inquiryResultModels));
    }

    private void InquiryByTimeRange() {
        var inquiryResultModels = _api.InquiryByTimeRange("14010724", "14010725");
        Console.WriteLine("inquiry By Time Range result: " + string.Join(", ", inquiryResultModels));
    }

    private void InquiryByReferenceId() {
        var inquiryResultModels = _api.InquiryByReferenceId(new(){referenceNumber});
        Console.WriteLine("inquiry By Reference Id result: " + string.Join(", ", inquiryResultModels));
    }

    private void GetFiscalInformation() {
        var fiscalInformation = _api.GetFiscalInformation(MEMORY_ID);
        Console.WriteLine("Fiscal Information: " + fiscalInformation);
    }

    private void GetServiceStuffList()
    {
        var searchDto = new SearchDto(1, 10);
        var serviceStuffList = _api.GetServiceStuffList(searchDto);
        Console.WriteLine("Stuff List: " + string.Join(", ", serviceStuffList?.Result));
    }

    private void GetEconomicCodeInformation() {
        var economicCodeInformation = _api.GetEconomicCodeInformation("10660111880");
        Console.WriteLine("Economic Code Information: " + economicCodeInformation);
    }
}