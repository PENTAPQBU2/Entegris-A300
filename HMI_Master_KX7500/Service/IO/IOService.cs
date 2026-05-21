using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500
{
    public class IOService
    {
        #region PrivateFields
        //private static IOInitializeModel iOInitializeModel = new IOInitializeModel();
        //private static OPCUA_ opcUa = new OPCUA_();
        public const string IsInDesignMode = "IsInDesignMode";
        #endregion

        #region Constructor
        //public IOService()
        //{
        //    initializeMessenger();
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeMessenger()
        //{
        //    Messenger.Default.Register<IOInitializeModel>(this, initializeIO);
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeIO(IOInitializeModel _iOInitializeModel)
        //{
            
        //    iOInitializeModel = _iOInitializeModel;
        //}
        #endregion

        #region PublicMethod
        //public static bool SerializeIO(IOModel iOModel)
        //{
        //    bool value = false;

        //    XmlTools.Serialize(iOInitializeModel.IOFilePath, iOModel);
        //    value = true;
        //    Messenger.Default.Send(iOInitializeModel);

        //    return value;
        //}

        public static IOModel DeserializeIO()
        {
            IOModel ioModel = new IOModel();

            if (File.Exists(FilePath.IO))
            {
                ioModel = XmlTools.Deserialize<IOModel>(FilePath.IO);
            }
            return ioModel;
        }

        //public static List<string> GetIOTagList(IOModel iOModel)
        //{
        //    List<string> ioTagList = new List<string>();

        //    ioTagList = ioTagList.Union(iOModel.IOItemsModels.SelectMany(
        //             x => x.IOInputList).Select(
        //             x => x.IOInputTagModel).SelectMany(
        //             x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //                 y => y.GetValue(x).ToString()))).ToList();

        //    ioTagList = ioTagList.Union(iOModel.IOItemsModels.SelectMany(
        //        x => x.IOOutputList).Select(
        //        x => x.IOOutputTagModel).SelectMany(
        //        x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
        //            y => y.GetValue(x).ToString()))).ToList();

        //    return ioTagList;
        //}

        public static List<string> GetIOTagList()
        {
            IOModel iOModel = DeserializeIO();
            List<string> ioTagList = new List<string>();

            ioTagList = ioTagList.Union(iOModel.IOItemsModels.SelectMany(
                     x => x.IOInputList).Select(
                     x => x.IOInputTagModel).SelectMany(
                     x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                         y => y.GetValue(x).ToString()))).ToList();

            ioTagList = ioTagList.Union(iOModel.IOItemsModels.SelectMany(
                x => x.IOOutputList).Select(
                x => x.IOOutputTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                    y => y.GetValue(x).ToString()))).ToList();

            return ioTagList;
        }
        #endregion
    }
}
