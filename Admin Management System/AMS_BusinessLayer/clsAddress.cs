using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsAddress
    {
        public enum enMode { AddNew = 0, Update = 1}
        public enMode Mode = enMode.AddNew;
        public int AddressID { get; set; }
        public string AddressName { get; set; }
        public int PersonId { get; set; }
        public clsAddressDTO ADTO
        {
            get { return (new clsAddressDTO(this.AddressID, this.AddressName, this.PersonId)); }
        }
        public clsAddress()
        {
            this.AddressID = -1;
            this.AddressName = "";
            this.PersonId = -1;
            Mode = enMode.AddNew;
        }
        private clsAddress(clsAddressDTO ADTO)
        {
            this.AddressID = ADTO.AddressId;
            this.AddressName = ADTO.Address;
            this.PersonId = ADTO.PersonId;
            Mode = enMode.Update;
        }
        private bool _AddNewAddress()
        {
            this.AddressID = clsAddressData.AddNewAddress(ADTO);
            return (AddressID != -1);
        }
        private bool _UpdateAddress()
        {
            return clsAddressData.UpdateAddress(ADTO);
        }
        public static clsAddress FindByAddressID(int AddressId)
        {
            clsAddressDTO ADTO = clsAddressData.GetAddressInfoByID(AddressId);

            if (ADTO != null)
                return new clsAddress(ADTO);
            else
                return null;
        }
        public static List<clsAddress> FindAddressesForPerson(int PersonId)
        {
            var AddressesByPersonDTO = clsAddressData.GetAddressesByPersonID(PersonId);
            var AddressesByPerson = new List<clsAddress>();

            foreach(var ADTO in AddressesByPersonDTO)
            {
                AddressesByPerson.Add(new clsAddress(ADTO));
            }

            return AddressesByPerson;
        }
        public static List<clsAddress> GetAllAddresses()
        {
            var AddressesDTO = clsAddressData.GetAllAddresses();    
            var Addresses = new List<clsAddress>();

            foreach (var ADTO in AddressesDTO)
                Addresses.Add(new clsAddress(ADTO));

            return Addresses;
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                {
                    if(_AddNewAddress())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                }
                case enMode.Update:
                {
                    return _UpdateAddress();
                }
            }

            return false;
        }
    }
}