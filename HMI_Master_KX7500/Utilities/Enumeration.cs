namespace HMI_Master_KX7500
{
    //public enum LotEntryModeEnum
    //{
    //    Production,
    //    GRR,
    //    IronMan
    //}

    #region PublicEnumerations

    public enum ComponentLifeCycleUpdate
    {
        Enable,
        Name
    }

    public enum CanonicalDataType
    {
        Unknown = 0,
        Int = 2,
        Long = 3,
        Real = 5,
        String = 8,
        Boolean = 11,
        Time = 19
    }

    public enum CycleTimeUpdate
    {
        Enable,
        Name,
        Timer
    }

    public enum DryRunUpdate
    {
        Enable,
        Name
    }

    public enum HeartbeatUpdate
    {
        Update
    }

    public enum TrackingStatusUpdate
    {
        Index
    }

    public enum ConveyorTrayUpdate
    {
        Index
    }

    public enum MachineLayoutUpdate
    {
        Index
    }

    public enum MachineModeUpdate
    {
        Index
    }

    public enum MachineStatusUpdate
    {
        Index
    }

    public enum MachineType
    {
        HMI_WLBI_SANAN
    }

    public enum MotorUpdate
    {
        ImportMotorAxisTeachingPosition
    }

    public enum OEEUpdate
    {
        Uph
    }

    public enum ShiftUpdate
    {
        Update
    }

    public enum LanguageUpdate
    {
        Update
    }

    public enum UserAccountGroupAccess
    {
        CloseWindow,
        ComponentLifeCycle,

        //ComponentLifeCycleEditor,
        Configuration,

        ConfigurationSettings,

        //ControlPanelEditor,
        CycleTime,

        //CycleTimeEditor,
        DryRun,

        //DryRunEditor,
        EngineeringToggleButton,

        //HeartbeatEditor,
        InitializeButton,

        //LanguageEditor,
        //LanguageTranslationEditor,
        LotEntry,

        //LotEntryEditor,
        //MachineModeEditor,
        //MachineSpeedEditor,
        //MachineStatusEditor,
        //MachineUphEditor,
        Main,

        OEE,

        //OEEEditor,
        //PopUpEditor,
        Recipe,

        Settings,
        SettingsGeneral,

        //SettingsGeneralEditor,
        UserAccountEditor,

        //UserAccountGroupEditor,
        //UserAccountSettingsEditor,
        Engineering,

        //EngineeringEditor,
        EngineeringPenta,

        IOList,
        IOLocation
        //IOEditor,
    }

    public enum AlarmType
    {
        Error,
        Warning
    }

    public enum PLCConfigUpdate
    {
        Save,
        Delete,
    }

    public enum OPCModules
    {
        Frame,
        Main,

        //Calibration,
        Recipe,

        OEE,
        PopUp,
        DryRun,
        Settings,
        CycleTime,
        ComponentLifeCycle,
        Motor,
        IO,
        Engineering,

        //AutoCleaning,
        //CassetteLog,
        VisionSetup,

        LotEntry,
        LFLegSettings,
        LFLegDrawingPos,

        //ModuleStatus,
        TrackingStatus,

        WelderSetup,
        MachineLayout,
        RecipeEditor
        // in case there is additional pages, please add on here.
    }

    public enum MachineTypeMPTC
    {
        MP,
        TC
    }

    public enum HMIAction
    {
        CloseHMI
    }

    #endregion PublicEnumerations
}