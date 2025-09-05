using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsAdmin: clsPerson
    {
        private enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode = enMode.AddNew;
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string Password { get; set; } // check
        public byte PermissionLevel { get; set; }
        public bool IsActive { get; set; }
        public new DateTime CreatedDate { get; set; } // using new keyword to hide property in base class
        public int CreatedByAdminID { get; set; }
        public new DateTime LastStatusDate { get; set; } // using new keyword to hide property in base class
        
        // DTOs
        public clsAdminDTO ADTO 
        {
            get { return new clsAdminDTO(AdminID, PersonId, AdminName, PermissionLevel, IsActive, 
                CreatedDate, CreatedByAdminID, LastStatusDate); }
        }
        public clsAdminCredentialsDTO CredentialsDTO 
        { 
            get { return new clsAdminCredentialsDTO(AdminID,AdminName,Password); } 
        }
        
        //Constructors
        public clsAdmin()
        {
            this.AdminID = -1;
            this.PersonId = -1;
            this.AdminName = "";
            this.Password = "";
            this.PermissionLevel = 0;
            this.IsActive = false;
            this.CreatedDate = DateTime.Now;
            this.CreatedByAdminID = -1;
            this.LastStatusDate = DateTime.Now;

            _Mode = enMode.AddNew;
        }
        public clsAdmin(clsAdminDTO ADTO, clsPerson Person): base(Person.PDTO)
        {
            this.AdminID = ADTO.AdminID;
            this.AdminName = ADTO.AdminName;
            this.Password = CredentialsDTO.Password;// check
            this.PermissionLevel = ADTO.PermissionLevel;
            this.IsActive = ADTO.IsActive;
            this.CreatedDate = ADTO.CreatedDate;
            this.CreatedByAdminID = ADTO.CreatedByAdminID;
            this.LastStatusDate = ADTO.LastStatusDate;

            _Mode = enMode.Update;
        }
        public clsAdmin(clsAdminCredentialsDTO CredentialsDTO) // check
        {
            this.AdminName = CredentialsDTO.AdminName;
            this.Password = CredentialsDTO.Password;
        }

        //Methods to Perform Operations on Admins and Check Admin Case
        private bool _AddNewAdmin()
        {
            this.AdminID = clsAdminData.AddNewAdmin(ADTO, CredentialsDTO);
            return (this.AdminID != -1);
        }
        private bool _UpdateAdmin() 
        {
            return (clsAdminData.UpdateAdmin(ADTO));
        }
        public bool Delete()
        {
            return (clsAdminData.DeleteAdmin(this.AdminID));

            //in system after delete admin no need to delete personality info, may same person register later
            //in new position

            /*bool IsAdminDeleted = false;
            
            //first we delete admin
            IsAdminDeleted = (clsAdminData.DeleteAdmin(this.AdminID));

            if(!IsAdminDeleted)
                return false;

            //after delete admin from system, we delete person
            return base.DeletePerson();*/
        }
        public static bool IsAdminExistByAdminId(int AdminId)
        {
            return (clsAdminData.IsAdminExistByAdminId(AdminId));
        }
        public static bool IsAdminExistByPersonId(int PersonId)
        {
            return (clsAdminData.IsAdminExistByPersonId(PersonId));
        }
        public static bool IsAdminExistByAdminName(string AdminName)
        {
            return (clsAdminData.IsAdminExistByAdminName(AdminName));
        }
        public static bool SetAdminPermissionLevel(int AdminId, short PermissionLevel)
        {
            return (clsAdminData.SetAdminPermissionLevel(AdminId,PermissionLevel));
        }
        public static bool ChangeAdminPassword(int AdminID, string OldPassword, string NewPassword)
        {
            return (clsAdminData.ChangeAdminPassword(AdminID, OldPassword, NewPassword));
        }
        public static bool DeactivateAdminAccount(int AdminId, bool IsActive)
        {
            return (clsAdminData.DeactivateAdminAccount(AdminId,IsActive));
        }
        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the person table.

            base.Mode = (clsPerson.enMode)_Mode;

            if (!base.Save())
                return false;

            //After we save the person info now we save the admin info.

            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewAdmin())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateAdmin();
                    }
            }

            return false;
        }
    }
    public class clsAdminWithDetails
    {
        public clsAdminWithDetailsDTO AdminDetailsDTO { get; set; }
        public clsAdmin Admin {  get; set; }
        public string CreatedByAdmin { get; set; }
        //Constructors
        public clsAdminWithDetails()
        {
            this.AdminDetailsDTO = new clsAdminWithDetailsDTO(null, "", "", "");
        }
        private clsAdminWithDetails(clsAdminWithDetailsDTO AdminDetailsDTO, clsPerson Person)
        {
            this.AdminDetailsDTO = AdminDetailsDTO;
            Admin = new clsAdmin(AdminDetailsDTO.AdminDTO,Person);
        }
        //Methods to get Admin Info
        public static clsAdminWithDetails FindByAdminID(int AdminId)
        {
            clsAdminWithDetailsDTO AdminDetailsDTO = clsAdminData.GetAdminInfoByAdminId(AdminId);

            if (AdminDetailsDTO != null)
            {
                var Person = clsPerson.Find(AdminDetailsDTO.AdminDTO.PersonID);
                return (Person != null) ? new clsAdminWithDetails(AdminDetailsDTO, Person) : null;
            }

            return null;
        }
        public static clsAdminWithDetails FindByAdminName(string AdminName)
        {
            clsAdminWithDetailsDTO AdminDetailsDTO = clsAdminData.GetAdminInfoByAdminName(AdminName);

            if(AdminDetailsDTO != null)
            {
                var Person = clsPerson.Find(AdminDetailsDTO.AdminDTO.PersonID);
                return (Person != null) ? new clsAdminWithDetails(AdminDetailsDTO, Person) : null;
            }

            return null;
        }
        public static clsAdminWithDetails FindByPersonID(int PersonId)
        {
            clsAdminWithDetailsDTO AdminDetailsDTO = clsAdminData.GetAdminInfoByPersonId(PersonId);

            if (AdminDetailsDTO != null)
            {
                var Person = clsPerson.Find(AdminDetailsDTO.AdminDTO.PersonID);
                return (Person != null) ? new clsAdminWithDetails(AdminDetailsDTO, Person) : null;
            }

            return null;
        }
        public static clsAdminWithDetails FindByAdminNameAndPassword(string AdminName, string Password)
        {
            bool IsAdminExist = clsAdminData.IsAdminExistByAdminNameAndPassword(AdminName, Password);
            return (IsAdminExist) ? FindByAdminName(AdminName) : null;
        }
        public static List<clsAdminWithDetails> GetAllAdmins()
        {
            var AdminsDetailsDTO = clsAdminData.GetAllAdminsWithDetails();
            var Admins = new List<clsAdminWithDetails>();

            foreach (var ADTO in AdminsDetailsDTO)
            {
                var Person = clsPerson.Find(ADTO.AdminDTO.PersonID);

                if (Person != null)
                    Admins.Add(new clsAdminWithDetails(ADTO, Person));
            }

            return Admins;
        }
    }
}