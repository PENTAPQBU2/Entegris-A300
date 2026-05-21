using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Models;
using HMI_Master_KX7500.OPCUA;
using PQ.HMI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HMI_Master_KX7500.Services
{
    public class ComponentLifeCycleService
    {
        #region PrivateFields
        //private static ComponentLifeCycleInitializeModel componentLifeCycleInitializeModel = new ComponentLifeCycleInitializeModel();
        private static string IsInDesignMode = "IsInDesignMode";
        #endregion

        #region Constructor
        //public ComponentLifeCycleService()
        //{
        //    initializeMessenger();
        //}
        #endregion

        #region PrivateInitializeMethods
        //private void initializeMessenger()
        //{
        //    Messenger.Default.Register<ComponentLifeCycleInitializeModel>(this, initializeComponentLifeCycleList);
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeComponentLifeCycleList(ComponentLifeCycleInitializeModel _componentLifeCycleInitializeModel)
        //{
        //    componentLifeCycleInitializeModel = _componentLifeCycleInitializeModel;
        //}
        #endregion

        #region PublicMethods
        public static bool SerializeComponentLifeCycleList(List<ComponentLifeCycleModel> componentLifeCycleList)
        {
            bool value = false;

            XmlTools.Serialize(FilePath.ComponentLifeCycle,
                   componentLifeCycleList);

            value = true;

            //Messenger.Default.Send(componentLifeCycleInitializeModel);

            return value;
        }

        public static List<ComponentLifeCycleModel> DeserializeComponentLifeCycleopcUaList()
        {
            List<ComponentLifeCycleModel> componentLifeCycleList = new List<ComponentLifeCycleModel>();
            //string clcNamespace = "Component_Life_Cycle_DB";
            string clcNamespace = "DB_Component_Life_Cycle";
            string moduleName = string.Empty;

            if (!File.Exists(FilePath.ComponentLifeCycle))
            {              
                for (int i = 1; i < 16; i++)
                {
                    //switch (i)
                    //{
                    //    case 1:
                    //        moduleName = "Rbt_LoadLF_Lifter";
                    //        break;
                    //    case 2:
                    //        moduleName = "Rbt_LoadLF_Gripper";
                    //        break;
                    //    case 3:
                    //        moduleName = "Rbt_LoadDBC_Lifter";
                    //        break;
                    //    case 4:
                    //        moduleName = "Rbt_LoadDBC_Gripper";
                    //        break;
                    //    case 5:
                    //        moduleName = "Rbt_Unload_Lifter";
                    //        break;
                    //    case 6:
                    //        moduleName = "Rbt_Unload_Gripper";
                    //        break;
                    //    case 7:
                    //        moduleName = "Left_Stage_Gripper";
                    //        break;
                    //    case 8:
                    //        moduleName = "Right_Stage_Gripper";
                    //        break;
                    //    case 9:
                    //        moduleName = "LFLoader_Clamper";
                    //        break;
                    //    case 10:
                    //        moduleName = "LFLoader_Precisor";
                    //        break;
                    //    case 11:
                    //        moduleName = "Unloader_Clamper";
                    //        break;
                    //    case 12:
                    //        moduleName = "Unloader_Precisor";
                    //        break;
                    //    case 13:
                    //        moduleName = "Clean_Station_Lifter";
                    //        break;
                    //    case 14:
                    //        moduleName = "Clean_Station_Rotator";
                    //        break;
                    //    case 15:
                    //        moduleName = "Clean_Station_Gripper";
                    //        break;
                    //    case 16:
                    //        moduleName = "DBC_Destacker_Holder";
                    //        break;
                    //    case 17:
                    //        moduleName = "DBC_Loader_Clamper";
                    //        break;
                    //    case 18:
                    //        moduleName = "DBC_Unloader_Clamper";
                    //        break;
                    //    case 19:
                    //        moduleName = "PnP_Lifter";
                    //        break;
                    //    case 20:
                    //        moduleName = "PnP_Gripper";
                    //        break;
                    //    default:
                    //        break;
                    //}

                    moduleName = $"CLC_{i}";

                    componentLifeCycleList.Add(new ComponentLifeCycleModel
                    {
                        ComponentLifeCycleTagModel = new ComponentLifeCycleTagModel
                        {
                            ComponentLifeCycleEnableTag = OPCUAHelper.CombinedNodeString(clcNamespace, moduleName, "EnableComponent"),

                            ComponentLifeCycleNameTag = OPCUAHelper.CombinedNodeString(clcNamespace, moduleName, "ComponentName"),

                            ComponentLifeCycleCountTag = OPCUAHelper.CombinedNodeString(clcNamespace, moduleName, "CycleCount"),
                            
                            ComponentLifeCycleCountResetTag = OPCUAHelper.CombinedNodeString(clcNamespace, moduleName, "CountReset"),
                            
                            ComponentLifeCycleLimitTag = OPCUAHelper.CombinedNodeString(clcNamespace, moduleName, "LifeCycle_Limit"),
                        }
                    });
                }

                SerializeComponentLifeCycleList(componentLifeCycleList);
            }

            componentLifeCycleList = XmlTools.Deserialize<List<ComponentLifeCycleModel>>(
                FilePath.ComponentLifeCycle);

            return componentLifeCycleList;
        }

        public static List<string> GetComponentLifeCycleTagList()
        {
            var componentLifeCycleList = DeserializeComponentLifeCycleopcUaList();
            List<string> componentLifeCycleTagList = new List<string>();

            componentLifeCycleTagList = componentLifeCycleList.Select(
                   x => x.ComponentLifeCycleTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                       y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString())).ToList();

            return componentLifeCycleTagList;
        }
        #endregion
    }
}