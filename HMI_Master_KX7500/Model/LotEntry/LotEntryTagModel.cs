using GalaSoft.MvvmLight;

namespace HMI_Master_KX7500.Model.LotEntry
{
    public class LotEntryTagModel : ViewModelBase
    {
        #region PublicProperties
        //private string newLotTag = string.Empty;
        //public string NewLotTag
        //{
        //    get { return newLotTag; }
        //    set
        //    {
        //        if (newLotTag != value)
        //        {
        //            newLotTag = value;
        //            RaisePropertyChanged(nameof(NewLotTag));
        //        }
        //    }
        //}

        //private string endLotTag = string.Empty;
        //public string EndLotTag
        //{
        //    get { return endLotTag; }
        //    set
        //    {
        //        if (endLotTag != value)
        //        {
        //            endLotTag = value;
        //            RaisePropertyChanged(nameof(EndLotTag));
        //        }
        //    }
        //}

        //private string lotIDTag = string.Empty;
        //public string LotIDTag
        //{
        //    get { return lotIDTag; }
        //    set
        //    {
        //        if (lotIDTag != value)
        //        {
        //            lotIDTag = value;
        //            RaisePropertyChanged(nameof(LotIDTag));
        //        }
        //    }
        //}

        //private string lotCreatedTag = string.Empty;
        //public string LotCreatedTag
        //{
        //    get { return lotCreatedTag; }
        //    set
        //    {
        //        if (lotCreatedTag != value)
        //        {
        //            lotCreatedTag = value;
        //            RaisePropertyChanged(nameof(LotCreatedTag));
        //        }
        //    }
        //}

        //private string operatorIDTag = string.Empty;
        //public string OperatorIDTag
        //{
        //    get { return operatorIDTag; }
        //    set
        //    {
        //        if (operatorIDTag != value)
        //        {
        //            operatorIDTag = value;
        //            RaisePropertyChanged(nameof(OperatorIDTag));
        //        }
        //    }
        //}

        //private string lotQuantityTag = string.Empty;
        //public string LotQuantityTag
        //{
        //    get { return lotQuantityTag; }
        //    set
        //    {
        //        if (lotQuantityTag != value)
        //        {
        //            lotQuantityTag = value;
        //            RaisePropertyChanged(nameof(LotQuantityTag));
        //        }
        //    }
        //}

        //private string customer_IDTag = string.Empty;
        //public string Customer_IDTag
        //{
        //    get { return customer_IDTag; }
        //    set
        //    {
        //        if (customer_IDTag != value)
        //        {
        //            customer_IDTag = value;
        //            RaisePropertyChanged(nameof(Customer_IDTag));
        //        }
        //    }
        //}

        //private string recipe_IDTag = string.Empty;
        //public string Recipe_IDTag
        //{
        //    get { return recipe_IDTag; }
        //    set
        //    {
        //        if (recipe_IDTag != value)
        //        {
        //            recipe_IDTag = value;
        //            RaisePropertyChanged(nameof(Recipe_IDTag));
        //        }
        //    }
        //}
        //private string current_Recipe_NameTag = string.Empty;
        //public string Current_Recipe_NameTag
        //{
        //    get { return current_Recipe_NameTag; }
        //    set
        //    {
        //        if (current_Recipe_NameTag != value)
        //        {
        //            current_Recipe_NameTag = value;
        //            RaisePropertyChanged(nameof(Current_Recipe_NameTag));
        //        }
        //    }
        //}
        //private string pin_Material_IDTag = string.Empty;
        //public string Pin_Material_IDTag
        //{
        //    get { return pin_Material_IDTag; }
        //    set
        //    {
        //        if (pin_Material_IDTag != value)
        //        {
        //            pin_Material_IDTag = value;
        //            RaisePropertyChanged(nameof(Pin_Material_IDTag));
        //        }
        //    }
        //}

        //-------------------------------------------------------------
        private string work_Order_Started_Tag = string.Empty;
        public string Work_Order_Started_Tag
        {
            get { return work_Order_Started_Tag; }
            set
            {
                if (work_Order_Started_Tag != value)
                {
                    work_Order_Started_Tag = value;
                    RaisePropertyChanged(nameof(Work_Order_Started_Tag));
                }
            }
        }

        private string work_Order_ID_Tag = string.Empty;
        public string Work_Order_ID_Tag
        {
            get { return work_Order_ID_Tag; }
            set
            {
                if (work_Order_ID_Tag != value)
                {
                    work_Order_ID_Tag = value;
                    RaisePropertyChanged(nameof(Work_Order_ID_Tag));
                }
            }
        }

        private string set_Work_Order_Info_Button_Tag = string.Empty;
        public string Set_Work_Order_Info_Button_Tag
        {
            get { return set_Work_Order_Info_Button_Tag; }
            set
            {
                if (set_Work_Order_Info_Button_Tag != value)
                {
                    set_Work_Order_Info_Button_Tag = value;
                    RaisePropertyChanged(nameof(Set_Work_Order_Info_Button_Tag));
                }
            }
        }

        private string end_Work_Order_Button_Tag = string.Empty;
        public string End_Work_Order_Button_Tag
        {
            get { return end_Work_Order_Button_Tag; }
            set
            {
                if (end_Work_Order_Button_Tag != value)
                {
                    end_Work_Order_Button_Tag = value;
                    RaisePropertyChanged(nameof(End_Work_Order_Button_Tag));
                }
            }
        }

        private string operatorID_Tag = string.Empty;
        public string OperatorID_Tag
        {
            get { return operatorID_Tag; }
            set
            {
                if (operatorID_Tag != value)
                {
                    operatorID_Tag = value;
                    RaisePropertyChanged(nameof(OperatorID_Tag));
                }
            }
        }

        private string recipe_Tag = string.Empty;
        public string Recipe_Tag
        {
            get { return recipe_Tag; }
            set
            {
                if (recipe_Tag != value)
                {
                    recipe_Tag = value;
                    RaisePropertyChanged(nameof(Recipe_Tag));
                }
            }
        }

        private string lot_Quantity_Tag = string.Empty;
        public string Lot_Quantity_Tag
        {
            get { return lot_Quantity_Tag; }
            set
            {
                if (lot_Quantity_Tag != value)
                {
                    lot_Quantity_Tag = value;
                    RaisePropertyChanged(nameof(Lot_Quantity_Tag));
                }
            }
        }

        private string oK_To_End_Work_Order_Tag = string.Empty;
        public string OK_To_End_Work_Order_Tag
        {
            get { return oK_To_End_Work_Order_Tag; }
            set
            {
                if (oK_To_End_Work_Order_Tag != value)
                {
                    oK_To_End_Work_Order_Tag = value;
                    RaisePropertyChanged(nameof(OK_To_End_Work_Order_Tag));
                }
            }
        }

        private string current_Quantity_Tag = string.Empty;
        public string Current_Quantity_Tag
        {
            get { return current_Quantity_Tag; }
            set
            {
                if (current_Quantity_Tag != value)
                {
                    current_Quantity_Tag = value;
                    RaisePropertyChanged(nameof(Current_Quantity_Tag));
                }
            }
        }
        
        private string _tsoColumnTag = string.Empty;
        public string TSOColumnTag
        {
            get { return _tsoColumnTag; }
            set
            {
                if (_tsoColumnTag != value)
                {
                    _tsoColumnTag = value;
                    RaisePropertyChanged(nameof(TSOColumnTag));
                }
            }
        }

        #endregion
    }
}