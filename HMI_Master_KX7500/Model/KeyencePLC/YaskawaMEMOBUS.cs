namespace HMI_Master_KX7500.Models.KeyencePLC
{
    public static class YaskawaMEMOBUS
    {
        internal static string[] G_BList1 = new string[10001];
        internal static string[] G_WList1 = new string[4501]; // First 450 for bit
        internal static string[] G_WList2 = new string[1001];
        internal static string[] G_WList3 = new string[1001];
        internal static string[,] G_ScreenList = new string[1001, 2];   // ,0 store address ,1 store data

        // == For Yaskawa PLC communication ==========================
        private static short TransPort;

        private static byte SFC, CPUNum;
        private static short DataNum;
        private static long Adr;
        private static int RW;
        private static string Ssbuf, Srbuf, WData, RegisterType;

        private static byte[] sbuf = new byte[4097];
        private static byte[] rbuf = new byte[4097];
        private static short DATAi, MDATAi;
        private static short iSerial;
        // ===========================================================

        internal static int G_arrCounter;
        internal static bool G_Occupied;
        internal static bool G_WriteOn;

        public static void IniEngine()
        {
            G_arrCounter = 0;
            G_Occupied = false;
            G_WriteOn = false;

            // Initialize G_W/BList
            for(int k = 0; k <= 2500; k++)
                G_WList1[k] = "0";
            for(int k = 0; k <= 10000; k++)
                G_BList1[k] = "0";
            for(int k = 0; k <= 1000; k++)
            {
                G_ScreenList[k, 0] = "";
                G_ScreenList[k, 1] = "";
            }

            for(int k = 0; k <= 30; k++)
            {
                HMI_KeyenceGlobal.IN0[k] = "0";
                HMI_KeyenceGlobal.IN1[k] = "0";
                HMI_KeyenceGlobal.IN2[k] = "0";
                HMI_KeyenceGlobal.IN3[k] = "0";
                HMI_KeyenceGlobal.OUT0[k] = "0";
                HMI_KeyenceGlobal.OUT1[k] = "0";
                HMI_KeyenceGlobal.OUT2[k] = "0";
                HMI_KeyenceGlobal.OUT3[k] = "0";
            }
        }
    }
}