using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsDoctor: clsPerson
    {
        private enum enMode { AddNew = 0, Update = 1};
        private enMode _Mode = enMode.AddNew;
        private decimal _Salary;
        public int DoctorID { get; set; }
        public int CollegeID { get; set; }
        public int DepartmentID { get; set; }
        public byte DoctorStatus { get; set; }
        public decimal Salary
        {
            get => _Salary;
            private set => _Salary = value;
        }
        public new DateTime CreatedDate { get; set; } // using new keyword to hide property in base class
        public int CreatedByAdminID { get; set; }
        public new DateTime LastStatusDate { get; set; } // using new keyword to hide property in base class
        public byte PermissionLevel { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public void UpdateSalary(decimal NewSalary, clsAdmin CurrentAdmin)
        {
            /*
            if(!CurrentAdmin.HasPermission)
                throw new UnauthorizedAccessException("You are not authorized to update the salary.");
            if(NewSalary < 0)
                throw new ArgumentException("Salary must be positive.");*/

            this.Salary = NewSalary;
        }
        public decimal GetSalary(clsAdmin CurrentAdmin)
        {
            /*if (!currentUser.HasPermission("ViewSalary"))
                throw new UnauthorizedAccessException("You are not authorized to view the salary.");*/

            return this.Salary;
        }
        public clsDoctorDTO DoctorDTO
        {
            get { return new clsDoctorDTO(DoctorID,PersonId,CollegeID,DepartmentID, DoctorStatus,
                CreatedDate,CreatedByAdminID,LastStatusDate, PermissionLevel,IsActive); }
        }
        public clsDoctorCredentialsDTO CredentialsDTO
        {
            get { return new clsDoctorCredentialsDTO(DoctorID, Password); }
        }
        public clsDoctorSalaryDTO SalaryDTO
        {
            get { return new clsDoctorSalaryDTO(DoctorID, Salary); }
        }
        public clsDoctor()
        {
            this.DoctorID = -1;
            this.PersonId = -1;
            this.CollegeID = -1;
            this.DepartmentID = -1;
            this.DoctorStatus = 0;
            this.Salary = 0.0m;
            this.CreatedDate = DateTime.Now;
            this.CreatedByAdminID = -1;
            this.LastStatusDate = DateTime.Now;
            this.PermissionLevel = 0;
            this.IsActive = false;
            this.Password = "";

            _Mode = enMode.AddNew;
        }
        public clsDoctor(clsDoctorDTO DoctorDTO, clsPerson Person): base(Person.PDTO)
        {
            this.DoctorID = DoctorDTO.DoctorID;
            this.CollegeID = DoctorDTO.CollegeID;
            this.DepartmentID = DoctorDTO.DepartmentID;
            this.DoctorStatus = DoctorDTO.DoctorStatus;
            this.CreatedDate = DoctorDTO.CreatedDate;
            this.CreatedByAdminID = DoctorDTO.CreatedByAdminID;
            this.LastStatusDate = DoctorDTO.LastStatusDate;
            this.PermissionLevel = DoctorDTO.PermissionLevel;
            this.IsActive = DoctorDTO.IsActive;

            _Mode = enMode.Update;
        }
        private bool _AddNewDoctor()
        {
            this.DoctorID = clsDoctorData.AddNewDoctor(DoctorDTO,CredentialsDTO,SalaryDTO);
            return (this.DoctorID != -1);
        }
        private bool _UpdateDoctor()
        {
            return clsDoctorData.UpdateDoctor(DoctorDTO);
        }
        public bool DeleteDoctor()
        {
            return clsDoctorData.DeleteDoctor(this.DoctorID);

            //in system after delete doctor no need to delete personality info, may same person register later
            //in new position

            /*bool IsDoctorDeleted = false;

            //first we delete doctor

            IsDoctorDeleted = clsDoctorData.DeleteDoctor(this.DoctorID);

            if (!IsDoctorDeleted)
                return false;

            //after delete doctor from system, we delete person

            return base.DeletePerson();*/
        }
        public static bool IsDoctorExistByDoctorID(int DoctorID)
        {
            return clsDoctorData.IsDoctorExistByDoctorID(DoctorID);
        }
        public static bool IsDoctorExistByPersonID(int PersonID)
        {
            return clsDoctorData.IsDoctorExistByPersonID(PersonID);
        }
        public static bool SetPermissionLevel(int DoctorID, byte PermissionLevel)
        {
            return clsDoctorData.SetDoctorPermissionLevel(DoctorID,PermissionLevel);
        }
        public static bool ChangePassword(int DoctorID, string OldPassword, string NewPassword)
        {
            return clsDoctorData.ChangeDoctorPassword(DoctorID,OldPassword,NewPassword);
        }
        public static bool DeactivateAccount(int DoctorID, bool IsActive)
        {
            return clsDoctorData.DeactivateDoctorAccount(DoctorID,IsActive);
        }
        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the person table.

            base.Mode = (clsPerson.enMode)_Mode;

            if(!base.Save())
                return false;

            //After we save the person info now we save the doctor info.

            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewDoctor())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateDoctor();
                    }
            }

            return false;
        }
    }
    public class clsDoctorWithDetails
    {
        public clsDoctor Doctor { get; set; }
        public clsDoctorWithDetailsDTO DoctorWithDetailsDTO { get; set; }
        public string FullName { get; set; }
        public string College { get; set; }
        public string Department { get; set; }
        public string CreatedBy { get; set; }
        public clsDoctorWithDetails(clsDoctorWithDetailsDTO DoctorDetailsDTO, clsPerson Person)
        {
            this.Doctor = new clsDoctor(DoctorDetailsDTO.DoctorDTO, Person);
            this.DoctorWithDetailsDTO = DoctorDetailsDTO;
        }
        public static clsDoctorWithDetails FindByDoctorID(int DoctorId)
        {
            clsDoctorWithDetailsDTO DoctorDetailsDTO = clsDoctorData.GetDoctorInfoByDoctorId(DoctorId);

            if (DoctorDetailsDTO != null)
            {
                clsPerson Person = clsPerson.Find(DoctorDetailsDTO.DoctorDTO.PersonID);
                return (Person != null) ? new clsDoctorWithDetails(DoctorDetailsDTO, Person) : null;
            }

            return null;
        }
        public static clsDoctorWithDetails FindByPersonID(int PersonId)
        {
            clsDoctorWithDetailsDTO DoctorDetailsDTO = clsDoctorData.GetDoctorInfoByPersonId(PersonId);

            if (DoctorDetailsDTO != null)
            {
                clsPerson Person = clsPerson.Find(DoctorDetailsDTO.DoctorDTO.PersonID);
                return (Person != null) ? new clsDoctorWithDetails(DoctorDetailsDTO, Person) : null;
            }

            return null;
        }
        public static List<clsDoctorWithDetails> GetAllDoctors()
        {
            var DoctorDetailsDTO = clsDoctorData.GetAllDoctors();
            var Doctors = new List<clsDoctorWithDetails>();

            foreach (var doctor in DoctorDetailsDTO)
            {
                var Person = clsPerson.Find(doctor.DoctorDTO.PersonID);

                if (Person != null)
                    Doctors.Add(new clsDoctorWithDetails(doctor, Person));
            }

            return Doctors;
        }
    }
}