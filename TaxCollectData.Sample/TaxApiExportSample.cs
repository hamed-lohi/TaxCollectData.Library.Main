using Microsoft.Extensions.DependencyInjection;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Dto.Properties;
using TaxCollectData.Library.Enums;
using TaxCollectData.Library.Extensions;

namespace TaxCollectData.Sample;

internal class TaxApiExportSample
{
    private const string MEMORY_ID = "A11216";

    private const string PRIVATE_KEY = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC16RdUCnytCHb6tPJHz0nUozX3sjag8AOKiYXLzqePgGfzau6OOvnCRzBB2uOMxpxMyG5fcbxblHTx+EgcC9zktqREgN47Ka81CTHiS9TpfDC/32ccqHviBCT+bjhCFdi3XmI86hSNoM6ZcPZbDyhqNJ+8jEk9RwHN9Z0Ca6ruMHSOFWisl/h1l5gsjfNGIDUjsZyJ4c6xtXDtJJ6EW+mZsXqWavvLUH2p4lDIxe8Nste8/WCfuNosbbbZtGTv9k5zjXXO4RaelM3QDtKQHKJ+Y9sh5ZXwmAgNrLyXUv0CLtkwlqXjhRtgL35me7E/4SlysAafXHXh6bXBWKDXvf3lAgMBAAECggEAKZ43H9tZXFYSMHgK8Spn1A32GCIN9QKMcNrXQUJZnA9u2OY3R1OPVicmz35Gbqv20OtPMydohlQ/8CcTSlVdd3cgvGp2TXdTNjFRGBqszrFCzvcL1mbmltZHZ21skhQA9azSkWilhKSMd9b8Cee9IZVCEWdQD0SqUUZW4lmjLOSFjQA0ExqCd28Q8yOkoELsCF9zzi6UDEg3NaKvCTETnrw04yIIE2zgBCLCSZJiDks2r5E0rgVefDAetxaXBvYwy83O8PTh70hmjAXGPVz065edFLvglpITxCtF7DEAWjtQvAoiejWCcMou1PvaJQdgAOeYjCUBOn6QZlwx4qZNiQKBgQD94tMPSCKtSBpZPOd/ZCQZWGsevuABiiGU+OLLAIUjjl+n9RFM39iPiLAooq1YeKsjxjWKscY3L9eKz4UU1uRNkahWz8+Ho7zq6R4S5D+idp3QDRRSPAK7OWZOGH/By+WvypOUyyZG8aEM/GxqbdE+cIitH3VncNU9ZC7uUqTDvQKBgQC3bNiM2yhuOhOKL3DG1lxqr9wA4QEbIPCd7pCaCuHRvRuLCus86z0jO7PZv7f/6Vtu4yBTKWtRfP8pDXZISBIbEtxe7/p2vI1sBMi/uGAAnUM0Ndw1ISpU1jzpUyW9Vf0quWPG0plA9MwCTccwXt1znX9wNlFk4uOHrRaAkacxSQKBgQC9bHvuvfJpeQ93n1JVoOCyLF8X/G/jli0CGkQUFBpB8hr6lIVI5waL6P8OAnn1NWry8RLHnWX3jPFzduujJcYG/fMsejYrzIXKew3eKIA19ew+61NLG80p5WSoRe0kKb4AT9OWw4+WhPeVWcyGB9ODk7DWAk+1UuE2wcWmOPEHvQKBgCBCTluHd7bbE/Cro0P1E0/YGfM1n0IsKuU7vca5vzlp7twnUXPnU1tM9raHF080tVXtMBQwJqWwPBf2PCU+N6D3UjaIMh6Lzrt+o+fD/25cOiOGjXHyoUVGYHQQoYSJCPtom3muNDHOW8rT7wI8zOm2e2E6zFnX5XhJIrn948+xAoGAFEJrNWzE5z8BRItE8Hak3oyHCwjEWs9+/FgkI3oBDbdMagTjzArSwmOWyFX3GvBy4bzIK+ML+v+q4hYgjJzraFUgHMIxDwbxMSW6Sb4B8EkGDBTMgliTqwY4WJBMs/Yu+aCnbGh74zW6ii/qahTcFpQjiZDYGvdp/q01Az/J+N8=";
    
    private readonly ITaxApis _api;

    private readonly SampleExportInvoiceCreator _sampleInvoiceCreator = new();

    private string uid;

    private string referenceNumber;

    public TaxApiExportSample()
    {
        TaxApiService.Instance.Init(MEMORY_ID,
            new SignatoryConfig(PRIVATE_KEY, null),
            new NormalProperties(ClientType.SELF_TSP, ""));
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