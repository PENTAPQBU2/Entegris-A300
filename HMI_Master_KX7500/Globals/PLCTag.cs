using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Globals
{
    public static class PLCTag
    {
        #region Main Page
        public const string MachineStatus = "EM103";
        public const string ProductionUPH = "EM1254";
        public const string Yield = "EM1260";
        public const string MachineMode = "EM104";
        public const string MachineName = "EM770";
        public const string Project_No = "EM950";
        public const string TotalPassFoup = "EM1759";
        public const string TotalFailFoup = "EM1761";
        public const string LotQuantity = "EM1561";
        public const string AssemblyFoup = "EM1634";
        public const string LoadedShell = "R20000";
        public const string LoadedSideColumn = "EM5.12";
        public const string LoadedShimORing = "EM21.13";

        public const string HMIStart = "MR000";
        public const string HMIStop = "MR001";
        public const string HMIReset = "MR002";


        public const string ErrorString = "EM201";
        public const string TmpErrorString = "EM340";
        public const string WarningString = "EM480";
        public const string TmpWarningString = "EM630";

        public const string PLCProgramVersion = "EM910";
        public const string HMIProgramVersion = "EM930";

        public const string EngineeringModeBit = "MR1000";

        public const string IOPageBit = "MR1300";
        #endregion

        #region Lot OEE
        public const string Lot_OEE_Start_Date_Time = "EM1782";
        public const string Lot_OEE_New_Lot_ID_String = "EM1501";
        public const string Lot_OEE_Operator_ID = "EM1553";

        public const string Lot_OEE_Total_Quantity_Out = "EM1763";
        public const string Lot_OEE_Total_Quantity_In = "EM1767";
        public const string Lot_OEE_Total_Passed = "EM1759";
        public const string Lot_OEE_Total_Failed = "EM1761";
        public const string Lot_OEE_Total_Yield = "EM1765";
        public const string Lot_OEE_Soft_Jam = "EM1756";
        public const string Lot_OEE_Hard_Jam = "EM1757";

        public const string Lot_OEE_System_Up_Time = "EM1736";
        public const string Lot_OEE_Operation_Time = "EM1738";
        public const string Lot_OEE_Down_Time = "EM1740";
        public const string Lot_OEE_Idle_Time = "EM1742";
        public const string Lot_OEE_MTBF = "EM1750";
        public const string Lot_OEE_MTBA = "EM1748";
        #endregion

        #region Shift OEE
        public const string Shift_OEE_Start_Date_Time = "EM1076";

        public const string Shift_OEE_Total_Quantity_Out = "EM1054";
        public const string Shift_OEE_Total_Quantity_In = "EM1058";
        public const string Shift_OEE_Total_Passed = "EM1050";
        public const string Shift_OEE_Total_Failed = "EM1052";
        public const string Shift_OEE_Total_Yield = "EM1056";
        public const string Shift_OEE_Soft_Jam = "EM1061";
        public const string Shift_OEE_Hard_Jam = "EM1060";

        public const string Shift_OEE_System_Up_Time = "EM1030";
        public const string Shift_OEE_Operation_Time = "EM1032";
        public const string Shift_OEE_Down_Time = "EM1034";
        public const string Shift_OEE_Idle_Time = "EM1036";
        public const string Shift_OEE_MTBF = "EM1044";
        public const string Shift_OEE_MTBA = "EM1042";
        #endregion

        #region Hourly UPH
        public const string UPH_Hourly_0 = "EM1200";
        public const string UPH_Hourly_1 = "EM1202";
        public const string UPH_Hourly_2 = "EM1204";
        public const string UPH_Hourly_3 = "EM1206";
        public const string UPH_Hourly_4 = "EM1208";
        public const string UPH_Hourly_5 = "EM1210";
        public const string UPH_Hourly_6 = "EM1212";
        public const string UPH_Hourly_7 = "EM1214";
        public const string UPH_Hourly_8 = "EM1216";
        public const string UPH_Hourly_9 = "EM1218";
        public const string UPH_Hourly_10 = "EM1220";
        public const string UPH_Hourly_11 = "EM1222";
        public const string UPH_Hourly_12 = "EM1224";
        public const string UPH_Hourly_13 = "EM1226";
        public const string UPH_Hourly_14 = "EM1228";
        public const string UPH_Hourly_15 = "EM1230";
        public const string UPH_Hourly_16 = "EM1232";
        public const string UPH_Hourly_17 = "EM1234";
        public const string UPH_Hourly_18 = "EM1236";
        public const string UPH_Hourly_19 = "EM1238";
        public const string UPH_Hourly_20 = "EM1240";
        public const string UPH_Hourly_21 = "EM1242"; 
        public const string UPH_Hourly_22 = "EM1244";
        public const string UPH_Hourly_23 = "EM1246";
        #endregion

        #region Input
        public const string Touch_Button_Signal = "R10000";
        public const string Start_Button = "R10001";
        public const string Stop_Button = "R10002";
        public const string Reset_Button = "R10003";
        public const string E_Stop_Button = "R10004";
        public const string E_Stop_Reset_Button = "R10005";
        public const string Safety_Relay_Signal = "R10006";
        public const string Incoming_Air_Pressure_Switch = "R10007";
        public const string Curtain_Sensor_Signal = "R10008";
        public const string E_Stop_Button2 = "R10009";
        public const string Safety_Door_Lock_Signal = "R100010";
        public const string Contactot_Cut_Off_Signal = "R10011";
        #endregion

        #region Camline
        public const string Trigger_HMI_Send_Start_Job = "MR49603"; //"EM3000.3"
        public const string Lot_ID = "EM1501";
        public const string Foup_Barcode = "EM3050";
        public const string Equipment_ID = "EM3070";
        public const string Start_Job_OK = "MR49604"; //"EM3000.3"
        public const string Start_Job_NOK = "MR49605"; //"EM3000.5"
        public const string Trigger_HMI_Send_Job_Complete = "MR49606"; //"EM3000.6"
        public const string Overall_Pass = "MR49600"; //EM3000.0
        public const string Overall_Fail = "MR49601"; //EM3000.1
        public const string Error_During_Testing = "MR49607"; //EM3000.7

        //ScrewNumber
        public const string Screw_Number = "EM10";
        #endregion

        #region Lot recipe (existing PLC labels)
        public const string Lot_Recipe_ID = "EM1559";        //Lot_Info_DB_Recipe_ID, 2W
        public const string Lot_Recipe_Name = "EM1623";      //Lot_Info_DB_Curr_Recipe_Name, string[20]

        //Robot point set of the lot recipe. PLC moves it to W0102 (robot int input 1, shell_int)
        public const string HMI_Robot_PointSet = "EM4200";   //2W
        #endregion

    }
}
