using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsStudent:clsPerson
    {
        private enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode = enMode.AddNew;
        public int StudentID { get; set; }
        public int RegistrationStudentAppID { get; set; }
        public int RankID { get; set; }
        public byte StudentStatus { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal DebitBalance { get; set; }
        public byte SpecializationHours { get; set; }
        public byte PassedHours { get; set; }
        public float CumulativeAverage { get; set; }
        public int CreatedByAdminID { get; set; }
        public new DateTime CreatedDate { get; set; }
        public new DateTime LastStatusDate { get; set; }
        public clsStudentDTO SDTO
        {
            get 
            { 
                return new clsStudentDTO(StudentID,PersonId,RegistrationStudentAppID, RankID,StudentStatus,
                    Password,IsActive,CreditBalance, DebitBalance,SpecializationHours,PassedHours,
                    CumulativeAverage, CreatedByAdminID, CreatedDate, LastStatusDate);
            }
        }
        public clsStudent()
        {
            this.StudentID = -1;
            this.PersonId = -1;
            this.RegistrationStudentAppID = -1;
            this.RankID = -1;
            this.StudentStatus = 0;
            this.Password = "";
            this.IsActive = false;
            this.CreditBalance = 0.0m;
            this.DebitBalance = 0.0m;
            this.SpecializationHours = 0;
            this.PassedHours = 0;
            this.CumulativeAverage = 0.0f;
            this.CreatedByAdminID = -1;
            this.CreatedDate = DateTime.Now;
            this.LastStatusDate = DateTime.Now;

            _Mode = enMode.AddNew;
        }
        private clsStudent(clsStudentDTO SDTO, clsPerson Person):base(Person.PDTO)
        {
            this.StudentID = SDTO.StudentID;
            this.PersonId = SDTO.PersonID;
            this.RegistrationStudentAppID = SDTO.RegistrationStudentAppID;
            this.RankID = SDTO.RankID;
            this.StudentStatus = SDTO.StudentStatus;
            this.Password = SDTO.Password;
            this.IsActive = SDTO.IsActive;
            this.CreditBalance = SDTO.CreditBalance;
            this.DebitBalance = SDTO.DebitBalance;
            this.SpecializationHours = SDTO.SpecializationHours;
            this.PassedHours = SDTO.PassedHours;
            this.CumulativeAverage = SDTO.CumulativeAverage;
            this.CreatedByAdminID = SDTO.CreatedByAdminID;
            this.CreatedDate = SDTO.CreatedDate;
            this.LastStatusDate = SDTO.LastStatusDate;

            _Mode = enMode.Update;
        }
        private bool _AddNewStudent()
        {
            this.StudentID = clsStudentData.AddNewStudent(SDTO);
            return (this.StudentID != -1);
        }
        private bool _UpdateStudent()
        {
            return clsStudentData.UpdateStudent(SDTO);
        }
        public bool DeleteStudent()
        {
            return clsStudentData.DeleteStudent(this.StudentID);

            //in system after delete student no need to delete personality info, may same person register later in
            //new program

            /*bool IsStudentDeleted = false;
            IsStudentDeleted = clsStudentData.DeleteStudent(this.StudentID);

            if (!IsStudentDeleted)
                return false;

            return base.DeletePerson();*/
        }
        public static clsStudent FindByStudentID(int StudentID)
        {
            clsStudentDTO SDTO = clsStudentData.GetStudentInfoByStudentID(StudentID);

            if (SDTO != null)
            {
                var Person = clsPerson.Find(SDTO.PersonID);
                return (Person != null) ? new clsStudent(SDTO,Person) : null;
            }

            return null;
        }
        public static clsStudent FindByPersonID(int PersonId)
        {
            clsStudentDTO SDTO = clsStudentData.GetStudentInfoByPersonID(PersonId);

            if (SDTO != null)
            {
                var Person = clsPerson.Find(SDTO.PersonID);
                return (Person != null) ? new clsStudent(SDTO, Person) : null;
            }

            return null;
        }
        public static clsStudent FindByRegistrationAppID(int RegistrationAppID)
        {
            clsStudentDTO SDTO = clsStudentData.GetStudentInfoByRegistrationAppID(RegistrationAppID);

            if (SDTO != null)
            {
                var Person = clsPerson.Find(SDTO.PersonID);
                return (Person != null) ? new clsStudent(SDTO, Person) : null;
            }

            return null;
        }
        public static List<clsStudent> GetAllStudents()
        {
            var StudentsDTO = clsStudentData.GetAllStudents();
            var Students = new List<clsStudent>();  

            foreach(var Student in StudentsDTO)
            {
                var Person = clsPerson.Find(Student.PersonID);

                if (Person != null)
                    Students.Add(new clsStudent(Student, Person));
            }
            
            return Students;
        }
        public static bool IsStudentExistByStudentId(int StudentID)
        {
            return clsStudentData.IsStudentExistByStudentId(StudentID);
        }
        public static bool IsStudentExistByPersonId(int PersonID)
        {
            return clsStudentData.IsStudentExistByPersonId(PersonID);
        }
        public static bool ChangePassword(int StudentId, string OldPassword, string NewPassword)
        {
            return clsStudentData.ChangeStudentPassword(StudentId, OldPassword, NewPassword);
        }
        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the person table.

            base.Mode = (clsPerson.enMode)_Mode;

            if(!base.Save())
                return false;

            //After we save the person info now we save the student info.

            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewStudent())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateStudent();
                    }
            }

            return false;
        }
    }
}