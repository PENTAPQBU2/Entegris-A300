using GalaSoft.MvvmLight;
using System.Windows.Input;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Model.LotEntry
{
    public class LotEntryModel : ViewModelBase
    {
        #region PublicCommandProperties

        private ICommand tSOColumnCommand = null;
        [XmlIgnore]
        public ICommand TSOColumnCommand
        {
            get { return tSOColumnCommand; }
            set
            {
                if (tSOColumnCommand != value)
                {
                    tSOColumnCommand = value;
                    RaisePropertyChanged(nameof(TSOColumnCommand));
                }
            }
        }

        private ICommand newLotCommand = null;
        [XmlIgnore]
        public ICommand NewLotCommand
        {
            get { return newLotCommand; }
            set
            {
                if (newLotCommand != value)
                {
                    newLotCommand = value;
                    RaisePropertyChanged(nameof(NewLotCommand));
                }
            }
        }

        private ICommand endLotCommand = null;
        [XmlIgnore]
        public ICommand EndLotCommand
        {
            get { return endLotCommand; }
            set
            {
                if (endLotCommand != value)
                {
                    endLotCommand = value;
                    RaisePropertyChanged(nameof(EndLotCommand));
                }
            }
        }

        //
        private ICommand shellColourBlackCommand = null;
        [XmlIgnore]
        public ICommand ShellColourBlackCommand
        {
            get { return shellColourBlackCommand; }
            set
            {
                if (shellColourBlackCommand != value)
                {
                    shellColourBlackCommand = value;
                    RaisePropertyChanged(nameof(ShellColourBlackCommand));
                }
            }
        }
        private ICommand transparentCommand = null;
        [XmlIgnore]
        public ICommand TransparentCommand
        {
            get { return transparentCommand; }
            set
            {
                if (transparentCommand != value)
                {
                    transparentCommand = value;
                    RaisePropertyChanged(nameof(TransparentCommand));
                }
            }
        }
        #endregion

        #region PublicProperties

        private bool work_Order_Started = false;
        [XmlIgnore]
        public bool Work_Order_Started
        {
            get { return work_Order_Started; }
            set
            {
                if (work_Order_Started != value)
                {
                    work_Order_Started = value;
                    RaisePropertyChanged(nameof(Work_Order_Started));
                }
            }
        }

        private string work_Order_ID = string.Empty;
        [XmlIgnore]
        public string Work_Order_ID
        {
            get { return work_Order_ID; }
            set
            {
                if (work_Order_ID != value)
                {
                    work_Order_ID = value;
                    RaisePropertyChanged(nameof(Work_Order_ID));
                }
            }
        }

        private bool set_Work_Order_Info_Button = false;
        [XmlIgnore]
        public bool Set_Work_Order_Info_Button
        {
            get { return set_Work_Order_Info_Button; }
            set
            {
                if (set_Work_Order_Info_Button != value)
                {
                    set_Work_Order_Info_Button = value;
                    RaisePropertyChanged(nameof(Set_Work_Order_Info_Button));
                }
            }
        }

        private bool end_Work_Order_Button = false;
        [XmlIgnore]
        public bool End_Work_Order_Button
        {
            get { return end_Work_Order_Button; }
            set
            {
                if (end_Work_Order_Button != value)
                {
                    end_Work_Order_Button = value;
                    RaisePropertyChanged(nameof(End_Work_Order_Button));
                }
            }
        }

        private string operatorID = string.Empty;
        [XmlIgnore]
        public string OperatorID
        {
            get { return operatorID; }
            set
            {
                if (operatorID != value)
                {
                    operatorID = value;
                    RaisePropertyChanged(nameof(OperatorID));
                }
            }
        }

        private int recipe = 0;
        [XmlIgnore]
        public int Recipe
        {
            get { return recipe; }
            set
            {
                if (recipe != value)
                {
                    recipe = value;
                    RaisePropertyChanged(nameof(Recipe));
                }
            }
        }

        private int lot_Quantity = 0;
        [XmlIgnore]
        public int Lot_Quantity
        {
            get { return lot_Quantity; }
            set
            {
                if (lot_Quantity != value)
                {
                    lot_Quantity = value;
                    RaisePropertyChanged(nameof(Lot_Quantity));
                }
            }
        }

        private bool oK_To_End_Work_Order = false;
        [XmlIgnore]
        public bool OK_To_End_Work_Order
        {
            get { return oK_To_End_Work_Order; }
            set
            {
                if (oK_To_End_Work_Order != value)
                {
                    oK_To_End_Work_Order = value;
                    RaisePropertyChanged(nameof(OK_To_End_Work_Order));
                }
            }
        }

        private int current_Quantity = 0;
        [XmlIgnore]
        public int Current_Quantity
        {
            get { return current_Quantity; }
            set
            {
                if (current_Quantity != value)
                {
                    current_Quantity = value;
                    RaisePropertyChanged(nameof(Current_Quantity));
                }
            }
        }

        private string shell_Colour = string.Empty;
        [XmlIgnore]
        public string Shell_Colour
        {
            get { return shell_Colour; }
            set
            {
                if (shell_Colour != value)
                {
                    shell_Colour = value;
                    RaisePropertyChanged(nameof(Shell_Colour));
                }
            }
        }


        private bool tsoColumnValue = false;
        [XmlIgnore]
        public bool TSOColumnValue
        {
            get { return tsoColumnValue; }
            set
            {
                if (tsoColumnValue != value)
                {
                    tsoColumnValue = value;
                    RaisePropertyChanged(nameof(TSOColumnValue));
                }
            }
        }


        //---------------------------------------------------------------
        //private string lotID = string.Empty;
        //[XmlIgnore]
        //public string LotID
        //{
        //    get { return lotID; }
        //    set
        //    {
        //        if (lotID != value)
        //        {
        //            lotID = value;
        //            RaisePropertyChanged(nameof(LotID));
        //        }
        //    }
        //}

        //private bool lotCreated = false;
        //[XmlIgnore]
        //public bool LotCreated
        //{
        //    get { return lotCreated; }
        //    set
        //    {
        //        if (lotCreated != value)
        //        {
        //            lotCreated = value;
        //            RaisePropertyChanged(nameof(LotCreated));
        //        }
        //    }
        //}

        //private string operatorID = string.Empty;
        //[XmlIgnore]
        //public string OperatorID
        //{
        //    get { return operatorID; }
        //    set
        //    {
        //        if (operatorID != value)
        //        {
        //            operatorID = value;
        //            RaisePropertyChanged(nameof(OperatorID));
        //        }
        //    }
        //}

        //private string lotQuantity = string.Empty;
        //[XmlIgnore]
        //public string LotQuantity
        //{
        //    get { return lotQuantity; }
        //    set
        //    {
        //        if (lotQuantity != value)
        //        {
        //            lotQuantity = value;
        //            RaisePropertyChanged(nameof(LotQuantity));
        //        }
        //    }
        //}


        //private string customer_ID = string.Empty;
        //[XmlIgnore]
        //public string Customer_ID
        //{
        //    get { return customer_ID; }
        //    set
        //    {
        //        if (customer_ID != value)
        //        {
        //            customer_ID = value;
        //            RaisePropertyChanged(nameof(Customer_ID));
        //        }
        //    }
        //}

        //private string recipe_ID = string.Empty;
        //[XmlIgnore]
        //public string Recipe_ID
        //{
        //    get { return recipe_ID; }
        //    set
        //    {
        //        if (recipe_ID != value)
        //        {
        //            recipe_ID = value;
        //            RaisePropertyChanged(nameof(Recipe_ID));
        //        }
        //    }
        //}
        //private string current_Recipe_Name = string.Empty;
        //[XmlIgnore]
        //public string Current_Recipe_Name
        //{
        //    get { return current_Recipe_Name; }
        //    set
        //    {
        //        if (current_Recipe_Name != value)
        //        {
        //            current_Recipe_Name = value;
        //            RaisePropertyChanged(nameof(Current_Recipe_Name));
        //        }
        //    }
        //}
        //private string pin_Material_ID = string.Empty;
        //[XmlIgnore]
        //public string Pin_Material_ID
        //{
        //    get { return pin_Material_ID; }
        //    set
        //    {
        //        if (pin_Material_ID != value)
        //        {
        //            pin_Material_ID = value;
        //            RaisePropertyChanged(nameof(Pin_Material_ID));
        //        }
        //    }
        //}

        private LotEntryTagModel lotEntryTagModel = new LotEntryTagModel();
        public LotEntryTagModel LotEntryTagModel
        {
            get { return lotEntryTagModel; }
            set
            {
                if (lotEntryTagModel != value)
                {
                    lotEntryTagModel = value;
                    RaisePropertyChanged(nameof(LotEntryTagModel));
                }
            }
        }
        #endregion

    }
}
