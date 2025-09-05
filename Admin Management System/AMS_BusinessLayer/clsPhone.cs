using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsPhone
    {
        public enum enMode { AddNew = 0, Update = 1};
        public enMode Mode = enMode.AddNew;
        public int PhoneId {  get; set; }
        public string PhoneNumber {  get; set; }
        public int PersonId {  get; set; }
        public clsPhoneDTO PhDTO 
        {
            get { return (new clsPhoneDTO(this.PhoneId, this.PhoneNumber, this.PersonId)); }
        }
        public clsPhone()
        {
            this.PhoneId = -1;
            this.PhoneNumber = "";
            this.PersonId = -1;
            Mode = enMode.AddNew;
        }
        private clsPhone(clsPhoneDTO PhoneDTO)
        {
            this.PhoneId = PhoneDTO.PhoneId;
            this.PhoneNumber = PhoneDTO.PhoneNumber;
            this.PersonId = PhoneDTO.PersonId;
            Mode = enMode.Update;
        }
        private bool _AddNewPhone()
        {
            this.PhoneId = clsPhoneData.AddNewPhoneNumber(PhDTO);
            return (this.PhoneId != -1);
        }
        private bool _UpdatePhoneNumber()
        {
            return clsPhoneData.UpdatePhoneNumber(PhDTO);
        }
        public static clsPhone Find(int PhoneId)
        {
            clsPhoneDTO PhDTO = clsPhoneData.GetPhoneInfoByID(PhoneId);

            if(PhDTO != null)
                return new clsPhone(PhDTO);
            else
                return null;
        }
        public static clsPhone Find(string PhoneNumber)
        {
            clsPhoneDTO PhDTO = clsPhoneData.GetPhoneInfoByPhoneNumber(PhoneNumber);

            if(PhDTO != null)
                return new clsPhone(PhDTO);
            else
                return null;
        }
        public static List<clsPhone> GetAllPhones()
        {
            var PhonesDTO = clsPhoneData.GetAllPhones();
            var Phones = new List<clsPhone>();

            foreach(var PhDTO in PhonesDTO)
            {
                Phones.Add(new clsPhone(PhDTO));
            }

            return Phones;
        }
        public static List<clsPhone> GetAllPhonesByPerson(int PersonId)
        {
            var PhonesByPersonDTO = clsPhoneData.GetAllPhonesByPerson(PersonId);
            var PhonesByPerson = new List<clsPhone>();

            foreach(var PhDto in PhonesByPersonDTO)
            {
                PhonesByPerson.Add(new clsPhone(PhDto));
            }

            return PhonesByPerson;
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewPhone())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdatePhoneNumber();
                    }
            }

            return false;
        }
    }
}