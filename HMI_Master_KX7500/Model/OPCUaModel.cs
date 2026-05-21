using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Model
{
    public class OPCUaModel
    {
        #region PublicProperties
        public List<string> OPCUA_TagList { get; set; }
        public OPCUaTagModel OPCUA_TagModel { get; set; }
        #endregion
    }
}
