using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Dto.Content
{
    public record FiscalInformationModel
    {
        public FiscalInformationModel(string nameTrade, FiscalStatus fiscalStatus, string economicCode)
        {
            NameTrade = nameTrade;
            FiscalStatus = fiscalStatus;
            EconomicCode = economicCode;
        }

        public string NameTrade { get; }

        public FiscalStatus FiscalStatus { get; }

        public string EconomicCode { get; }
    }
}