using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.OPCUA
{
    class OPCUAHelper
    {
        /*NOTE 1
         * CombinedNodeString and CombinedNodeInt is not returning data type of
         * node address such as siemens address (e.g DB450,STRING34)
         * its only for returning address in string since some of the opcUA 
         * node been renamed by PLC using 0-15 (e.g {DB_Unit_Info}/2:Num_of_Leg_Per_Quadrant/2:1)
         * 2:1 <- the 1 here is the address. so instead of using "1", we can just put 1 as int and 
         * return string node. Simplified Job = A glimpse of sunshine in our dark life as programmer
         * 
         * NOTE 2 :
         * we put Global.NodeNameSpace at front just in for case we do need to use it in the future
         * for now Global.NodeNameSpace is string.Empty
         * 
         * NOTE 3
         * please do note that if the address is in array, the node format at the end will be using this symbol []
         * refer to IONodeCombineInt
         */

        public static string IONodeCombineInt(string folderNode, string folderNode2, int controlNode)
        {
            string ioNode = $"{Global.NodeNameSpace}{folderNode}.{folderNode2}[{controlNode}]";
            return ioNode;
        }

        public static string IONodeCombineInt(string folderNode, int controlNode)
        {
            string ioNode = $"{Global.NodeNameSpace}{folderNode}.[{controlNode}]";
            return ioNode;
        }

        public static string CombinedNodeString(string folderNode, string controlNode)
        {
            string combinedNode = $"{Global.NodeNameSpace}{folderNode}.{controlNode}";
            return combinedNode;
        }

        public static string CombinedNodeString(string folderNode, string folderNode2, string controlNode)
        {
            string combinedNode = $"{Global.NodeNameSpace}{folderNode}.{folderNode2}.{controlNode}";
            return combinedNode;
        }
        public static string CombinedNodeString(string folderNode, string folderNode2, string folderNode3, string controlNode)
        {
            string combinedNode = $"{Global.NodeNameSpace}{folderNode}.{folderNode2}.{folderNode3}.{controlNode}";
            return combinedNode;
        }
        public static string CombinedNodeInt(string folderNode, string folderNode2, int controlNode)
        {
            string combinedNode = $"{Global.NodeNameSpace}{folderNode}.{folderNode2}.{controlNode}";
            return combinedNode;
        }

        public static string CombinedNodeArray(string folderNode, string folderNode2, string folderNode3, int arrayNode)
        {
            string arrayNode_ = $"{Global.NodeNameSpace}{folderNode}.{folderNode2}.{folderNode3}[{arrayNode}]";
            return arrayNode_;
        }
        public static string CombinedNodeArray(string folderNode, string folderNode2, int arrayNode)
        {
            string arrayNode_ = $"{Global.NodeNameSpace}{folderNode}.{folderNode2}[{arrayNode}]";
            return arrayNode_;
        }
    }
}
