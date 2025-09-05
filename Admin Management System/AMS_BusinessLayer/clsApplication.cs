using AMS_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AMS_BusinessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByAdminID { get; set; }
        public DateTime LastStatusDate { get; set; }
        public clsPerson PersonInfo { get; set; }
        public clsApplicationType AppTypeInfo { get; set; }
        public clsAdminWithDetails CreatedByAdminInfo { get; set; }
        public clsApplicationDTO AppDTO
        {
            get
            {
                return new clsApplicationDTO(ApplicationID, ApplicantPersonID, ApplicationTypeID,
                                               ApplicationStatus, ApplicationDate, PaidFees,
                                               CreatedByAdminID, LastStatusDate);
            }
        }
        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.ApplicationDate = DateTime.Now;
            this.PaidFees = 0.0m;
            this.CreatedByAdminID = -1;
            this.LastStatusDate = DateTime.Now;

            this.PersonInfo = new clsPerson();
            this.AppTypeInfo = new clsApplicationType();
            this.CreatedByAdminInfo = new clsAdminWithDetails();

            this.Mode = enMode.AddNew;
        }
        private clsApplication(clsApplicationDTO AppDTO)
        {
            this.ApplicationID = AppDTO.ApplicationID;
            this.ApplicantPersonID = AppDTO.ApplicantPersonID;
            this.ApplicationTypeID = AppDTO.ApplicationTypeID;
            this.ApplicationStatus = AppDTO.ApplicationStatus;
            this.ApplicationDate = AppDTO.ApplicationDate;
            this.PaidFees = AppDTO.PaidFees;
            this.CreatedByAdminID = AppDTO.CreatedByAdminID;
            this.LastStatusDate = AppDTO.LastStatusDate;

            this.PersonInfo = clsPerson.Find(ApplicantPersonID);
            this.AppTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.CreatedByAdminInfo = clsAdminWithDetails.FindByPersonID(ApplicantPersonID);

            this.Mode = enMode.Update;
        }
        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(AppDTO);
            return this.ApplicationID != -1;
        }
        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(AppDTO);
        }
        public bool DeleteApplication()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }
        public static clsApplication FindByApplicationId(int ApplicationId)
        {
            clsApplicationDTO ApplicationDTO = clsApplicationData.GetApplicationInfoByID(ApplicationId);
            return (ApplicationDTO != null) ? new clsApplication(ApplicationDTO) : null;
        }
        public static clsApplication FindByPersonId(int PersonID, int AppTypeID)
        {
            clsApplicationDTO AppDTO = clsApplicationData.GetApplicationInfoByPersonId(PersonID, AppTypeID);
            return (AppDTO != null) ? new clsApplication(AppDTO) : null;
        }
        public static bool IsApplicationExist(int ApplicationId)
        {
            return clsApplicationData.IsApplicationExist(ApplicationId);
        }
        public static bool DoesPersonHaveActiveApplication(int PersonId)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonId);
        }
        public static List<clsApplication> GetAllApplications()
        {
            var ApplicationDTO = clsApplicationData.GetAllApplications();
            var Applications = new List<clsApplication>();

            foreach (var App in ApplicationDTO)
            {
                if (App != null)
                    Applications.Add(new clsApplication(App));
            }

            return Applications;
        }
        public static List<clsApplication> GetAllApplicationsByAppType(int AppTypeID)
        {
            var ApplicationDTO = clsApplicationData.GetAllApplicationsByAppType(AppTypeID);
            var Applications = new List<clsApplication>();

            foreach (var App in ApplicationDTO)
            {
                if (App != null)
                    Applications.Add(new clsApplication(App));
            }

            return Applications;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewApplication())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateApplication();
                    }
            }

            return false;
        }
    }
    public class clsApplicationWithDetails
    {
        // Use expression-bodied properties for read-only properties; no need to write full getters
        private clsApplicationWithDetailsDTO _AppDetailsDTO { get; set; }
        public clsApplicationDTO ApplicationDTO => _AppDetailsDTO.ApplicationDTO;
        public string FullApplicantName => _AppDetailsDTO.FullApplicantName;
        public string ApplicationTypeTitle => _AppDetailsDTO.ApplicationTypeTitle;
        public string CreatedByAdminName => _AppDetailsDTO.CreatedByAdminName;
        public clsApplicationWithDetails()
        {
            _AppDetailsDTO = new clsApplicationWithDetailsDTO(null, "", "", "");
        }
        private clsApplicationWithDetails(clsApplicationWithDetailsDTO AppDetailsDTO)
        {
            this._AppDetailsDTO = AppDetailsDTO;
        }
        public static List<clsApplicationWithDetails> GetAllApplicationsWithDetails()
        {
            var AppsDetailsDTO = clsApplicationData.GetAllApplicationsWithDetails();
            var Apps = new List<clsApplicationWithDetails>();

            foreach (var App in AppsDetailsDTO)
            {
                if (App != null)
                    Apps.Add(new clsApplicationWithDetails(App));
            }

            return Apps;
        }
        public static clsApplicationWithDetails GetApplicationWithDetailsByAppId(int ApplicationId)
        {
            var AppWithDetailsDTO = clsApplicationData.GetApplicationWithDetailsByAppId(ApplicationId);
            return (AppWithDetailsDTO != null) ? new clsApplicationWithDetails(AppWithDetailsDTO) : null;
        }
        public static List<clsApplicationWithDetails> GetApplicationsWithDetailsByPersonId(int PersonId)
        {
            var AppsWithDetailsDTO = clsApplicationData.GetApplicationsWithDetailsByPersonId(PersonId);
            var AppsByPerson = new List<clsApplicationWithDetails>();

            foreach (var App in AppsWithDetailsDTO)
            {
                if (App != null)
                    AppsByPerson.Add(new clsApplicationWithDetails(App));
            }

            return AppsByPerson;
        }
        public static List<clsApplicationWithDetails> GetApplicationsWithDetailsByAppTypeId(int AppTypeId)
        {
            var AppsWithDetailsDTO = clsApplicationData.GetApplicationsWithDetailsByAppTypeId(AppTypeId);
            var AppsByAppType = new List<clsApplicationWithDetails>();

            foreach (var App in AppsWithDetailsDTO)
            {
                if (App != null)
                    AppsByAppType.Add(new clsApplicationWithDetails(App));
            }

            return AppsByAppType;
        }
        public static List<clsApplicationWithDetails> GetApplicationsWithDetailsByAppStatus(byte AppStatus)
        {
            var AppsWithDetailsDTO = clsApplicationData.GetApplicationsWithDetailsByAppStatus(AppStatus);
            var AppsByAppStatus = new List<clsApplicationWithDetails>();

            foreach (var App in AppsWithDetailsDTO)
            {
                if (App != null)
                    AppsByAppStatus.Add(new clsApplicationWithDetails(App));
            }

            return AppsByAppStatus;
        }
        public static List<clsApplicationWithDetails> GetActiveApplicationsForPerson(int PersonId, int AppTypeId)
        {
            var ActiveAppsDTO = clsApplicationData.GetActiveApplicationsForPerson(PersonId, AppTypeId);
            var ActiveAppsForPerson = new List<clsApplicationWithDetails>();

            foreach (var App in ActiveAppsDTO)
            {
                if (App != null)
                    ActiveAppsForPerson.Add(new clsApplicationWithDetails(App));
            }

            return ActiveAppsForPerson;
        }
    }
}

// update AppTypeId parameter after add enum enApplicationStatus