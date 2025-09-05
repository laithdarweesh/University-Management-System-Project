using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsApplicationType
    {
        private enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode = enMode.AddNew;
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }
        public clsApplicationTypeDTO AppTypeDTO
        {
            get { return new clsApplicationTypeDTO(this.ApplicationTypeID, this.ApplicationTypeTitle, 
                this.ApplicationFees); }
        }
        public clsApplicationType()
        {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.ApplicationFees = 0.0m;
            _Mode = enMode.AddNew;
        }
        private clsApplicationType(clsApplicationTypeDTO AppDTO)
        {
            this.ApplicationTypeID = AppDTO.ApplicationTypeID;
            this.ApplicationTypeTitle = AppDTO.ApplicationTypeTitle;
            this.ApplicationFees = AppDTO.ApplicationFees;
            _Mode = enMode.Update;
        }
        private bool _AddNewAppType()
        {
            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(AppTypeDTO);
            return this.ApplicationTypeID != -1;
        }
        private bool _UpdateAppType()
        {
            return clsApplicationTypeData.UpdateApplicationType(AppTypeDTO);    
        }
        public static bool DeleteAppType(int AppTypeId)
        {
            return clsApplicationTypeData.DeleteApplicationType(AppTypeId);
        }
        public static clsApplicationType Find(int AppTypeId)
        {
            var AppTypeDTO = clsApplicationTypeData.GetApplicationTypeInfoByID(AppTypeId);

            if (AppTypeDTO != null)
                return new clsApplicationType(AppTypeDTO);
            else
                return null;
        }
        public static List<clsApplicationType> GetAllApplicationTypes()
        {
            var AllAppTypeDTO = clsApplicationTypeData.GetAllApplicationTypes();
            var AllAppTypes = new List<clsApplicationType>();

            foreach (var AppType in AllAppTypeDTO)
            {
                AllAppTypes.Add(new clsApplicationType(AppType));
            }

            return AllAppTypes;    
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewAppType())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateAppType();
                    }
            }

            return false;
        }
    }
}