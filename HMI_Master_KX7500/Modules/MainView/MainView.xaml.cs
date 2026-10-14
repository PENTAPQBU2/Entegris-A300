using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.ModulesView.Main;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HMI_Master_KX7500.Modules.MainView
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());
            InitializeComponent();
            Event.UpdateEvent += Update;

            DataContext = new MainViewModel();
        }

        private KeyenceDll keyenceDll;
        ~MainView()
        {
            Event.UpdateEvent -= Update;
        }
        private void Update()
        {
            if (Global.PageList["Main"])
            {
                Dispatcher.Invoke(() =>
                {
                    MachineNameTextBlock.Text = keyenceDll.Read<string>(PLCTag.MachineName, 40);
                    ProjectNoTextBlock.Text = keyenceDll.Read<string>(PLCTag.Project_No, 20);
                    SoftwareProgramVerTextBlock.Text = keyenceDll.Read<string>(PLCTag.PLCProgramVersion, 26);
                    //BypassStatusTextBlock.Text=keyenceDll.Read<>(PLCTag.);

                    TotalPassTextBlock.Text = keyenceDll.Read<short>(PLCTag.TotalPassFoup).ToString();
                    TotalFailTextBlock.Text = keyenceDll.Read<short>(PLCTag.TotalFailFoup).ToString();

                    LotQtyTextBlock.Text = keyenceDll.Read<int>(PLCTag.LotQuantity).ToString();
                    CurrentFoupTextBlock.Text=keyenceDll.Read<int>(PLCTag.AssemblyFoup).ToString();
                    LoadedShellTextBlock.Text = keyenceDll.Read<bool>(PLCTag.LoadedShell).ToString();
                    LoadedSideColumnTextBlock.Text = keyenceDll.Read<bool>(PLCTag.LoadedSideColumn).ToString();
                    LoadedShimOringTextBlock.Text = keyenceDll.Read<bool>(PLCTag.LoadedShimORing).ToString();
                });               
            }
        }
    }
}