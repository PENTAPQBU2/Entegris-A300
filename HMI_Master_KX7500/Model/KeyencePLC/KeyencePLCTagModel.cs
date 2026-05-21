using DATABUILDERAXLibLB;

namespace HMI_Master_KX7500.Models.KeyencePLC
{
    public class KeyencePLCTagModel
    {
        #region PublicProperties

        public string KeyencePLCTag { get; set; }
        public string KeyencePLCValue { get; set; }
        public string KeyencePLCStrType { get; set; }
        public DBPlcDevice KeyencePLCType { get; set; }

        #endregion PublicProperties
    }
}