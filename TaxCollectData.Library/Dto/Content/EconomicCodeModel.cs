namespace TaxCollectData.Library.Dto.Content
{
    public record EconomicCodeModel
    {
        public EconomicCodeModel(string nameTrade, string taxpayerStatus, string taxpayerType, string postalcodeTaxpayer, string addressTaxpayer)
        {
            NameTrade = nameTrade;
            TaxpayerStatus = taxpayerStatus;
            TaxpayerType = taxpayerType;
            PostalcodeTaxpayer = postalcodeTaxpayer;
            AddressTaxpayer = addressTaxpayer;
        }

        public string NameTrade { get; }

        public string TaxpayerStatus { get; }

        public string TaxpayerType { get; }

        public string PostalcodeTaxpayer { get; }

        public string AddressTaxpayer { get; }
    }
}