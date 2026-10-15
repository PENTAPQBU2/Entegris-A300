using System.Collections.Generic;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.LotRecipe
{
    //One robot point set = the 33 point features (UR installation) one shell type uses.
    //Points[0..20] = P1-P21 screw 1-21, Points[21..32] = P22-P33 entry above screw 1,2,3,7,8,10,16-21.
    public class RobotPointSetModel
    {
        public int SetNo { get; set; }                  //= robot shell_int = recipe RobotPointSet
        public string Name { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty; //empty for the original sets with their own names
        public List<string> Points { get; set; } = new List<string>();

        [XmlIgnore]
        public string PendantNames => string.IsNullOrEmpty(Prefix) ? "original names" : $"{Prefix}_P01 ... {Prefix}_P33";
    }

    //Saved in <HMI folder>\MachineSettings\RobotPointSets.xml
    public class RobotPointSetListModel
    {
        public List<RobotPointSetModel> PointSets { get; set; } = new List<RobotPointSetModel>();
    }
}
