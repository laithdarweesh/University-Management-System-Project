using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public clsCountryDTO CountryDTO
        {
            get { return new clsCountryDTO(this.CountryID, this.CountryName); }
        }
        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";
        }
        private clsCountry(clsCountryDTO CDTO)
        {
            this.CountryID = CDTO.CountryID;
            this.CountryName = CDTO.CountryName;
        }
        public static clsCountry Find(int CountryID)
        {
            clsCountryDTO CountryDTO = clsCountryData.GetCountryInfoByID(CountryID);

            if(CountryDTO != null)
                return new clsCountry(CountryDTO);
            else
                return null;
        }
        public static clsCountry Find(string CountryName)
        {
            clsCountryDTO CountryDTO = clsCountryData.GetCountryInfoByName(CountryName);

            if (CountryDTO != null)
                return new clsCountry(CountryDTO);
            else
                return null;
        }
        public static List<clsCountry> GetAllCountries()
        {
            var CountriesDTO = clsCountryData.GetAllCountries();
            var Countries = new List<clsCountry>();

            foreach(var CDTO in CountriesDTO)
            {
                Countries.Add(new clsCountry(CDTO));
            }

            return Countries;
        }
    }
}