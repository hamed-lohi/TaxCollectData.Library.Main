namespace TaxCollectData.Sample;
internal class Program
{
    public static async Task Main(string[] args)
    {
        await new TaxApiSample().RunAsync().ConfigureAwait(false);
        await new TaxApiExportSample().RunAsync().ConfigureAwait(false);
        await new TspWithOwnKeySample().RunAsync().ConfigureAwait(false);
        await new TspWithOwnKeyV1Sample().RunAsync().ConfigureAwait(false);
        await new TspWithTaxpayerKeySample().RunAsync().ConfigureAwait(false);
        await new TspWithTaxpayerKeyV1Sample().RunAsync().ConfigureAwait(false);
        await new TaxApiWithPkcs8SignatureSample().RunAsync().ConfigureAwait(false);
        await new TaxApiWithPkcs11SignatureSample().RunAsync().ConfigureAwait(false);
    }
}