using System.Collections.Generic;

namespace HMI_Master_KX7500.Models.KeyencePLC
{
    public class KeyencePLCModel
    {
        #region PublicProperties

        public List<string> KeyencePLCTagList { get; set; }
        public KeyencePLCTagModel KeyencePLCTagModel { get; set; }

        #endregion PublicProperties
    }
}