using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int PersonId { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
        }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor { get; set; }
        public string Email { get; set; }
        public int NationalityCountryId { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public int CreatedByAdminId { get; set; }

        //About Phones & Addresses
        public clsPhone Phone { get; set; }
        public clsAddress Address { get; set; }
        public List<clsPhone> AllPhones { get; set; }
        public List<clsAddress> AllAddresses { get; set; }
        public clsPhone MainPhone => AllPhones?.FirstOrDefault();
        public clsAddress MainAddress => AllAddresses?.FirstOrDefault();
        public clsPersonDTO PDTO
        {
            get { return new clsPersonDTO(PersonId,NationalNo,FirstName, SecondName,ThirdName,LastName,
                DateOfBirth,Gendor, Email,NationalityCountryId,ImagePath,CreatedDate, LastStatusDate,
                CreatedByAdminId);}
        }
        public clsPerson()
        {
            this.PersonId = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 0;
            this.Email = "";
            this.NationalityCountryId = -1;
            this.ImagePath = "";
            this.CreatedDate = DateTime.Now;
            this.LastStatusDate = DateTime.Now;
            this.CreatedByAdminId = -1;

            this.Phone = new clsPhone();
            this.Address = new clsAddress();
            this.AllPhones = new List<clsPhone>();
            this.AllAddresses = new List<clsAddress>();
            Mode = enMode.AddNew;
        }
        protected clsPerson(clsPersonDTO PDTO)
        {
            this.PersonId = PDTO.PersonID;
            this.NationalNo = PDTO.NationalNo;
            this.FirstName = PDTO.FirstName;
            this.SecondName = PDTO.SecondName;
            this.ThirdName = PDTO.ThirdName;
            this.LastName = PDTO.LastName;
            this.DateOfBirth = PDTO.DateOfBirth;
            this.Gendor = PDTO.Gendor;
            this.Email = PDTO.Email;
            this.NationalityCountryId = PDTO.NationalityCountryID;
            this.ImagePath = PDTO.ImagePath;
            this.CreatedDate = PDTO.CreatedDate;
            this.LastStatusDate = PDTO.LastStatusDate;
            this.CreatedByAdminId = PDTO.CreatedByAdminID;

            this.AllPhones = clsPhone.GetAllPhonesByPerson(this.PersonId) ?? new List<clsPhone>();
            this.Phone = AllPhones.FirstOrDefault();

            this.AllAddresses = clsAddress.FindAddressesForPerson(this.PersonId) ?? new List<clsAddress>();
            this.Address = AllAddresses.FirstOrDefault();

            Mode = enMode.Update;
        }
        private bool _AddNewPerson()
        {
            this.PersonId = clsPersonData.AddNewPerson(PDTO);

            return (this.PersonId != -1);
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PDTO);
        }
        public bool DeletePerson()
        {
            return clsPersonData.DeletePerson(this.PersonId);
        }
        public static clsPerson Find(int PersonId)
        {
            clsPersonDTO PersonDTO = clsPersonData.GetPersonInfoByID(PersonId);

            if (PersonDTO != null)
                return new clsPerson(PersonDTO);
            else
                return null;
        }
        public static clsPerson Find(string NationalNo)
        {
            clsPersonDTO PersonDTO = clsPersonData.GetPersonInfoByNationalNo(NationalNo);

            if (PersonDTO != null)
                return new clsPerson(PersonDTO);
            else
                return null;
        }
        public static bool IsPersonExist(int PersonId)
        {
            return clsPersonData.IsPersonExist(PersonId);
        }
        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
        public static List<clsPerson> GetAllPeople()
        {
            var PeopleDTO = clsPersonData.GetAllPeople();
            var People = new List<clsPerson>();

            foreach (var Person in PeopleDTO)
            {
                People.Add(new clsPerson(Person));
            }

            return People;
        }
        private void _SavePhoneAndAddress()
        {
            if (!string.IsNullOrWhiteSpace(this.Phone?.PhoneNumber))
            {
                this.Phone.PersonId = this.PersonId;
                this.Phone.Save();
            }

            if(!string.IsNullOrWhiteSpace(this.Address?.AddressName))
            {
                this.Address.PersonId = this.PersonId;
                this.Address.Save();
            }
        }
        public bool Save()
        {
            bool IsSuccess = false;

            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        IsSuccess = _AddNewPerson();

                        if (IsSuccess)
                            Mode = enMode.Update;

                        break;
                    }
                case enMode.Update:
                    {
                        IsSuccess = _UpdatePerson();
                        break;
                    }
            }

            if (IsSuccess)
                _SavePhoneAndAddress();

            return IsSuccess;
        }
    }
}