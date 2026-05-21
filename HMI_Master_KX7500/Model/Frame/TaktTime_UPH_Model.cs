using GalaSoft.MvvmLight;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.Frame
{
    public class TaktTime_UPH_Model : ViewModelBase
    {
        private string taktUPH_Key = string.Empty;

        public string TaktUPH_Key
        {
            get { return taktUPH_Key; }
            set
            {
                if(taktUPH_Key != value)
                {
                    taktUPH_Key = value;
                    RaisePropertyChanged(nameof(TaktUPH_Key));
                }
            }
        }

        private string taktUPH_Name = string.Empty;

        [XmlIgnore]
        public string TaktUPH_Name
        {
            get { return taktUPH_Name; }
            set
            {
                if(taktUPH_Name != value)
                {
                    taktUPH_Name = value;
                    RaisePropertyChanged(nameof(TaktUPH_Name));
                }
            }
        }

        private string node = string.Empty;

        public string Node
        {
            get { return node; }
            set
            {
                if(node != value)
                {
                    node = value;
                    RaisePropertyChanged(nameof(Node));
                }
            }
        }

        private string nodetype = string.Empty;

        public string NodeType
        {
            get { return nodetype; }
            set
            {
                if(nodetype != value)
                {
                    nodetype = value;
                    RaisePropertyChanged(nameof(NodeType));
                }
            }
        }

        private string nodeValue = string.Empty;

        [XmlIgnore]
        public string NodeValue
        {
            get { return nodeValue; }
            set
            {
                if(nodeValue != value)
                {
                    nodeValue = value;
                    RaisePropertyChanged(nameof(NodeValue));
                }
            }
        }
    }
}