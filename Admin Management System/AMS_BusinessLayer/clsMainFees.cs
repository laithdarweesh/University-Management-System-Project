using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsMainFees
    {
        private enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode = enMode.AddNew;
        public int MainFeesID { get; set; }
        public string Title { get; set; }
        public decimal Fees { get; set; }
        public clsMainFeesDTO MFDTO
        {
            get { return new clsMainFeesDTO(this.MainFeesID,this.Title,this.Fees); }
        }
        public clsMainFees()
        { 
            this.MainFeesID = -1;
            this.Title = "";
            this.Fees = 0.0m;

            _Mode = enMode.AddNew;
        }
        private clsMainFees(clsMainFeesDTO MFDTO)
        {
            this.MainFeesID = MFDTO.MainFeesID;
            this.Title = MFDTO.Title;
            this.Fees = MFDTO.Fees;

            _Mode = enMode.Update;
        }
        private bool _AddNewFee()
        {
            this.MainFeesID = clsMainFeesData.AddNewMainFees(MFDTO);
            return (this.MainFeesID != -1);
        }
        private bool _UpdateFee()
        {
            return clsMainFeesData.UpdateMainFees(MFDTO);
        }
        public static bool DeleteFee(int FeeId)
        {
            return clsMainFeesData.DeleteFee(FeeId);
        }
        public static clsMainFees Find(int FeeId)
        {
            clsMainFeesDTO FeeDTO = clsMainFeesData.GetFeesInfoByID(FeeId);

            if (FeeDTO != null)
                return new clsMainFees(FeeDTO);
            else
                return null;
        }
        public static List<clsMainFees> GetAllFees()
        {
            var FeesDTO = clsMainFeesData.GetAllMainFees();
            var Fees = new List<clsMainFees>();

            foreach (var fee in FeesDTO)
            {
                Fees.Add(new clsMainFees(fee));
            }

            return Fees;
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewFee())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateFee();
                    }
            }

            return false;
        }
    }
}