using Microsoft.Extensions.DependencyInjection;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;
using TaxCollectData.Library.Extensions;

namespace TaxCollectData.Sample;

internal class TaxApiWithPkcs8SignatureSample
{
    private const string MEMORY_ID = "A11216";

    private static readonly string s_pemFilePath = @$"{AppContext.BaseDirectory}\private.pem";

    private readonly ITaxApis _api;

    private readonly SampleInvoiceCreator _sampleInvoiceCreator = new();

    private string uid;

    private string referenceNumber;

    public TaxApiWithPkcs8SignatureSample()
    {
        TaxApiService.Instance.Init(MEMORY_ID,
            new Pkcs8SignatoryConfig(s_pemFilePath, null),
            new NormalProperties(ClientType.SELF_TSP), "http://localhost:8055/requestsmanager/api/");
        _api = TaxApiService.Instance.TaxApis;
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

    public void GetServerInformation()
    {
        var serverInformation = _api.GetServerInformation();
        Console.WriteLine("success load server information: " + serverInformation);
    }

    private void GetToken()
    {
        var token = _api.RequestToken();
        if (token?.Token != null && token.Token.Length > 0)
        {
            Console.WriteLine("success login, token: " + token.Token);
        }
        else
        {
            Console.WriteLine("error in login");
        }
    }

    private async Task SendInvoicesAsync()
    {
        var invoices = _sampleInvoiceCreator.Create(MEMORY_ID);
        var responseModel = await _api.SendInvoicesAsync(invoices, null).ConfigureAwait(false);
        if (responseModel?.Body?.Result != null && responseModel.Body.Result.Any())
        {
            Console.WriteLine("success send invoice, response" + responseModel.Body.Result);
            var packetResponse = responseModel.Body.Result.First();
            uid = packetResponse.Uid;
            referenceNumber = packetResponse.ReferenceNumber;
        }
        else
        {
            Console.WriteLine(responseModel?.Body?.Errors);
        }
    }

    private void InquiryByUid()
    {
        var uidAndFiscalId = new UidAndFiscalId(uid, MEMORY_ID);
        var inquiryResultModels = _api.InquiryByUidAndFiscalId(new() { uidAndFiscalId });
        Console.WriteLine("inquiry By UID result: " + string.Join(", ", inquiryResultModels));
    }

    private void InquiryByTime()
    {
        var inquiryResultModels = _api.InquiryByTime("14010725");
        Console.WriteLine("inquiry By Time result: " + string.Join(", ", inquiryResultModels));
    }

    private void InquiryByTimeRange()
    {
        var inquiryResultModels = _api.InquiryByTimeRange("14010724", "14010725");
        Console.WriteLine("inquiry By Time Range result: " + string.Join(", ", inquiryResultModels));
    }

    private void InquiryByReferenceId()
    {
        var inquiryResultModels = _api.InquiryByReferenceId(new() { referenceNumber });
        Console.WriteLine("inquiry By Reference Id result: " + string.Join(", ", inquiryResultModels));
    }

    private void GetFiscalInformation()
    {
        var fiscalInformation = _api.GetFiscalInformation(MEMORY_ID);
        Console.WriteLine("Fiscal Information: " + fiscalInformation);
    }

    private void GetServiceStuffList()
    {
        var searchDto = new SearchDto(1, 10);
        var serviceStuffList = _api.GetServiceStuffList(searchDto);
        Console.WriteLine("Stuff List: " + string.Join(", ", serviceStuffList?.Result));
    }

    private void GetEconomicCodeInformation()
    {
        var economicCodeInformation = _api.GetEconomicCodeInformation("10660111880");
        Console.WriteLine("Economic Code Information: " + economicCodeInformation);
    }
}