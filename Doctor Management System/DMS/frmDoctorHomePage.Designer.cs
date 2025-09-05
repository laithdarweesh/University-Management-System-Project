namespace DMS
{
    partial class frmDoctorHomePage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.msMainMenu = new System.Windows.Forms.MenuStrip();
            this.tsmAcademicOperations = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmManageMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAddMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDeleteMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUpdateMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmManageTest = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmScheduleTest = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRecordTestResults = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGradesApproval = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmManageLectures = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSemesterMaterials = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMySchedule = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMyCourses = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDoctors = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmStudents = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmReport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAcademicRecords = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmStudentsGrades = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAttendanceRecords = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAccountSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMyInformation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLogOut = new System.Windows.Forms.ToolStripMenuItem();
            this.PbCoverPhoto = new System.Windows.Forms.PictureBox();
            this.PbLogo = new System.Windows.Forms.PictureBox();
            this.msMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbCoverPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // msMainMenu
            // 
            this.msMainMenu.AutoSize = false;
            this.msMainMenu.BackColor = System.Drawing.Color.White;
            this.msMainMenu.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAcademicOperations,
            this.tsmMySchedule,
            this.tsmMyCourses,
            this.tsmDoctors,
            this.tsmStudents,
            this.tsmReport,
            this.tsmAccountSetting});
            this.msMainMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.msMainMenu.Size = new System.Drawing.Size(1200, 72);
            this.msMainMenu.TabIndex = 1;
            this.msMainMenu.Text = "menuStrip1";
            // 
            // tsmAcademicOperations
            // 
            this.tsmAcademicOperations.AutoSize = false;
            this.tsmAcademicOperations.DoubleClickEnabled = true;
            this.tsmAcademicOperations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmManageMaterial,
            this.tsmManageTest,
            this.tsmManageLectures,
            this.tsmSemesterMaterials});
            this.tsmAcademicOperations.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsmAcademicOperations.Image = global::DMS.Properties.Resources.Academic_Operations_64;
            this.tsmAcademicOperations.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAcademicOperations.Name = "tsmAcademicOperations";
            this.tsmAcademicOperations.Size = new System.Drawing.Size(193, 72);
            this.tsmAcademicOperations.Text = "Academic Operations";
            // 
            // tsmManageMaterial
            // 
            this.tsmManageMaterial.BackColor = System.Drawing.Color.White;
            this.tsmManageMaterial.DoubleClickEnabled = true;
            this.tsmManageMaterial.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAddMaterial,
            this.tsmDeleteMaterial,
            this.tsmUpdateMaterial});
            this.tsmManageMaterial.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmManageMaterial.Image = global::DMS.Properties.Resources.Manage_Materilas_32;
            this.tsmManageMaterial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmManageMaterial.Name = "tsmManageMaterial";
            this.tsmManageMaterial.Size = new System.Drawing.Size(196, 38);
            this.tsmManageMaterial.Text = "Manage Material";
            // 
            // tsmAddMaterial
            // 
            this.tsmAddMaterial.BackColor = System.Drawing.Color.White;
            this.tsmAddMaterial.DoubleClickEnabled = true;
            this.tsmAddMaterial.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAddMaterial.Image = global::DMS.Properties.Resources.Material_Assignment_32;
            this.tsmAddMaterial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAddMaterial.Name = "tsmAddMaterial";
            this.tsmAddMaterial.Size = new System.Drawing.Size(196, 38);
            this.tsmAddMaterial.Text = "Add Material";
            // 
            // tsmDeleteMaterial
            // 
            this.tsmDeleteMaterial.BackColor = System.Drawing.Color.White;
            this.tsmDeleteMaterial.CheckOnClick = true;
            this.tsmDeleteMaterial.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmDeleteMaterial.Image = global::DMS.Properties.Resources.Delete_Material_32;
            this.tsmDeleteMaterial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmDeleteMaterial.Name = "tsmDeleteMaterial";
            this.tsmDeleteMaterial.Size = new System.Drawing.Size(196, 38);
            this.tsmDeleteMaterial.Text = "Delete Material";
            // 
            // tsmUpdateMaterial
            // 
            this.tsmUpdateMaterial.BackColor = System.Drawing.Color.White;
            this.tsmUpdateMaterial.DoubleClickEnabled = true;
            this.tsmUpdateMaterial.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmUpdateMaterial.Image = global::DMS.Properties.Resources.Update_Material_32;
            this.tsmUpdateMaterial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmUpdateMaterial.Name = "tsmUpdateMaterial";
            this.tsmUpdateMaterial.Size = new System.Drawing.Size(196, 38);
            this.tsmUpdateMaterial.Text = "Update Material";
            // 
            // tsmManageTest
            // 
            this.tsmManageTest.BackColor = System.Drawing.Color.White;
            this.tsmManageTest.DoubleClickEnabled = true;
            this.tsmManageTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmScheduleTest,
            this.tsmRecordTestResults,
            this.tsmGradesApproval});
            this.tsmManageTest.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmManageTest.Image = global::DMS.Properties.Resources.Test_32;
            this.tsmManageTest.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmManageTest.Name = "tsmManageTest";
            this.tsmManageTest.Size = new System.Drawing.Size(196, 38);
            this.tsmManageTest.Text = "Manage Test";
            // 
            // tsmScheduleTest
            // 
            this.tsmScheduleTest.BackColor = System.Drawing.Color.White;
            this.tsmScheduleTest.DoubleClickEnabled = true;
            this.tsmScheduleTest.Image = global::DMS.Properties.Resources.Schedule_Test_32;
            this.tsmScheduleTest.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmScheduleTest.Name = "tsmScheduleTest";
            this.tsmScheduleTest.Size = new System.Drawing.Size(196, 38);
            this.tsmScheduleTest.Text = "Schedule Test";
            // 
            // tsmRecordTestResults
            // 
            this.tsmRecordTestResults.BackColor = System.Drawing.Color.White;
            this.tsmRecordTestResults.DoubleClickEnabled = true;
            this.tsmRecordTestResults.Image = global::DMS.Properties.Resources.Test_Result_32;
            this.tsmRecordTestResults.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmRecordTestResults.Name = "tsmRecordTestResults";
            this.tsmRecordTestResults.Size = new System.Drawing.Size(196, 38);
            this.tsmRecordTestResults.Text = "Record Test Results";
            // 
            // tsmGradesApproval
            // 
            this.tsmGradesApproval.BackColor = System.Drawing.Color.White;
            this.tsmGradesApproval.DoubleClickEnabled = true;
            this.tsmGradesApproval.Image = global::DMS.Properties.Resources.Grades_Approval_32;
            this.tsmGradesApproval.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmGradesApproval.Name = "tsmGradesApproval";
            this.tsmGradesApproval.Size = new System.Drawing.Size(196, 38);
            this.tsmGradesApproval.Text = "Grades Approval";
            // 
            // tsmManageLectures
            // 
            this.tsmManageLectures.BackColor = System.Drawing.Color.White;
            this.tsmManageLectures.DoubleClickEnabled = true;
            this.tsmManageLectures.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmManageLectures.Image = global::DMS.Properties.Resources.Manage_Lectures_32;
            this.tsmManageLectures.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmManageLectures.Name = "tsmManageLectures";
            this.tsmManageLectures.Size = new System.Drawing.Size(196, 38);
            this.tsmManageLectures.Text = "Manage Lectures";
            // 
            // tsmSemesterMaterials
            // 
            this.tsmSemesterMaterials.BackColor = System.Drawing.Color.White;
            this.tsmSemesterMaterials.DoubleClickEnabled = true;
            this.tsmSemesterMaterials.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmSemesterMaterials.Image = global::DMS.Properties.Resources.Student_Semester_Material_32;
            this.tsmSemesterMaterials.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmSemesterMaterials.Name = "tsmSemesterMaterials";
            this.tsmSemesterMaterials.Size = new System.Drawing.Size(196, 38);
            this.tsmSemesterMaterials.Text = "Semester Materials";
            // 
            // tsmMySchedule
            // 
            this.tsmMySchedule.AutoSize = false;
            this.tsmMySchedule.DoubleClickEnabled = true;
            this.tsmMySchedule.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmMySchedule.Image = global::DMS.Properties.Resources.My_Schedule_64;
            this.tsmMySchedule.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmMySchedule.Name = "tsmMySchedule";
            this.tsmMySchedule.Size = new System.Drawing.Size(172, 72);
            this.tsmMySchedule.Text = "My Schedule";
            // 
            // tsmMyCourses
            // 
            this.tsmMyCourses.AutoSize = false;
            this.tsmMyCourses.DoubleClickEnabled = true;
            this.tsmMyCourses.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmMyCourses.Image = global::DMS.Properties.Resources.My_Courses_64;
            this.tsmMyCourses.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmMyCourses.Name = "tsmMyCourses";
            this.tsmMyCourses.Size = new System.Drawing.Size(172, 72);
            this.tsmMyCourses.Text = "My Courses";
            // 
            // tsmDoctors
            // 
            this.tsmDoctors.AutoSize = false;
            this.tsmDoctors.DoubleClickEnabled = true;
            this.tsmDoctors.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmDoctors.Image = global::DMS.Properties.Resources.Doctors_64;
            this.tsmDoctors.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmDoctors.Name = "tsmDoctors";
            this.tsmDoctors.Size = new System.Drawing.Size(172, 72);
            this.tsmDoctors.Text = "Doctors";
            // 
            // tsmStudents
            // 
            this.tsmStudents.AutoSize = false;
            this.tsmStudents.DoubleClickEnabled = true;
            this.tsmStudents.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmStudents.Image = global::DMS.Properties.Resources.Students_64;
            this.tsmStudents.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmStudents.Name = "tsmStudents";
            this.tsmStudents.Size = new System.Drawing.Size(185, 72);
            this.tsmStudents.Text = "Students";
            // 
            // tsmReport
            // 
            this.tsmReport.AutoSize = false;
            this.tsmReport.DoubleClickEnabled = true;
            this.tsmReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAcademicRecords,
            this.tsmStudentsGrades,
            this.tsmAttendanceRecords});
            this.tsmReport.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmReport.Image = global::DMS.Properties.Resources.Report_64;
            this.tsmReport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmReport.Name = "tsmReport";
            this.tsmReport.Size = new System.Drawing.Size(122, 72);
            this.tsmReport.Text = "Report";
            // 
            // tsmAcademicRecords
            // 
            this.tsmAcademicRecords.BackColor = System.Drawing.Color.White;
            this.tsmAcademicRecords.DoubleClickEnabled = true;
            this.tsmAcademicRecords.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAcademicRecords.Image = global::DMS.Properties.Resources.Academic_Record_32;
            this.tsmAcademicRecords.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAcademicRecords.Name = "tsmAcademicRecords";
            this.tsmAcademicRecords.Size = new System.Drawing.Size(196, 38);
            this.tsmAcademicRecords.Text = "Academic Records";
            // 
            // tsmStudentsGrades
            // 
            this.tsmStudentsGrades.BackColor = System.Drawing.Color.White;
            this.tsmStudentsGrades.DoubleClickEnabled = true;
            this.tsmStudentsGrades.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmStudentsGrades.Image = global::DMS.Properties.Resources.Students_Grades_32;
            this.tsmStudentsGrades.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmStudentsGrades.Name = "tsmStudentsGrades";
            this.tsmStudentsGrades.Size = new System.Drawing.Size(196, 38);
            this.tsmStudentsGrades.Text = "Students Grades";
            // 
            // tsmAttendanceRecords
            // 
            this.tsmAttendanceRecords.BackColor = System.Drawing.Color.White;
            this.tsmAttendanceRecords.DoubleClickEnabled = true;
            this.tsmAttendanceRecords.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAttendanceRecords.Image = global::DMS.Properties.Resources.Attendance_and_Absence_32;
            this.tsmAttendanceRecords.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAttendanceRecords.Name = "tsmAttendanceRecords";
            this.tsmAttendanceRecords.Size = new System.Drawing.Size(196, 38);
            this.tsmAttendanceRecords.Text = "Attendance Records";
            // 
            // tsmAccountSetting
            // 
            this.tsmAccountSetting.AutoSize = false;
            this.tsmAccountSetting.DoubleClickEnabled = true;
            this.tsmAccountSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMyInformation,
            this.tsmChangePassword,
            this.tsmLogOut});
            this.tsmAccountSetting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAccountSetting.Image = global::DMS.Properties.Resources.Account_Setting_64;
            this.tsmAccountSetting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAccountSetting.Name = "tsmAccountSetting";
            this.tsmAccountSetting.Size = new System.Drawing.Size(176, 72);
            this.tsmAccountSetting.Text = "Account Setting";
            // 
            // tsmMyInformation
            // 
            this.tsmMyInformation.BackColor = System.Drawing.Color.White;
            this.tsmMyInformation.DoubleClickEnabled = true;
            this.tsmMyInformation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmMyInformation.Image = global::DMS.Properties.Resources.My_Info_32;
            this.tsmMyInformation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmMyInformation.Name = "tsmMyInformation";
            this.tsmMyInformation.Size = new System.Drawing.Size(196, 38);
            this.tsmMyInformation.Text = "My Information";
            // 
            // tsmChangePassword
            // 
            this.tsmChangePassword.BackColor = System.Drawing.Color.White;
            this.tsmChangePassword.DoubleClickEnabled = true;
            this.tsmChangePassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmChangePassword.Image = global::DMS.Properties.Resources.password_32;
            this.tsmChangePassword.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmChangePassword.Name = "tsmChangePassword";
            this.tsmChangePassword.Size = new System.Drawing.Size(196, 38);
            this.tsmChangePassword.Text = "Change Password";
            // 
            // tsmLogOut
            // 
            this.tsmLogOut.BackColor = System.Drawing.Color.White;
            this.tsmLogOut.DoubleClickEnabled = true;
            this.tsmLogOut.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmLogOut.Image = global::DMS.Properties.Resources.Log_Out_32;
            this.tsmLogOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmLogOut.Name = "tsmLogOut";
            this.tsmLogOut.Size = new System.Drawing.Size(196, 38);
            this.tsmLogOut.Text = "Log Out";
            // 
            // PbCoverPhoto
            // 
            this.PbCoverPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PbCoverPhoto.Image = global::DMS.Properties.Resources.Cover_Photo___Professor_Management_System;
            this.PbCoverPhoto.Location = new System.Drawing.Point(0, 72);
            this.PbCoverPhoto.Name = "PbCoverPhoto";
            this.PbCoverPhoto.Size = new System.Drawing.Size(1200, 349);
            this.PbCoverPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbCoverPhoto.TabIndex = 2;
            this.PbCoverPhoto.TabStop = false;
            // 
            // PbLogo
            // 
            this.PbLogo.Image = global::DMS.Properties.Resources.NextGen_University_Logo;
            this.PbLogo.Location = new System.Drawing.Point(10, 82);
            this.PbLogo.Name = "PbLogo";
            this.PbLogo.Size = new System.Drawing.Size(111, 113);
            this.PbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbLogo.TabIndex = 16;
            this.PbLogo.TabStop = false;
            // 
            // frmDoctorHomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1200, 421);
            this.Controls.Add(this.PbLogo);
            this.Controls.Add(this.PbCoverPhoto);
            this.Controls.Add(this.msMainMenu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msMainMenu;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmDoctorHomePage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doctor Home Page";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbCoverPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmAcademicOperations;
        private System.Windows.Forms.ToolStripMenuItem tsmDoctors;
        private System.Windows.Forms.ToolStripMenuItem tsmReport;
        private System.Windows.Forms.ToolStripMenuItem tsmAccountSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmManageMaterial;
        private System.Windows.Forms.ToolStripMenuItem tsmAddMaterial;
        private System.Windows.Forms.ToolStripMenuItem tsmDeleteMaterial;
        private System.Windows.Forms.ToolStripMenuItem tsmUpdateMaterial;
        private System.Windows.Forms.ToolStripMenuItem tsmManageTest;
        private System.Windows.Forms.ToolStripMenuItem tsmScheduleTest;
        private System.Windows.Forms.ToolStripMenuItem tsmRecordTestResults;
        private System.Windows.Forms.ToolStripMenuItem tsmManageLectures;
        private System.Windows.Forms.ToolStripMenuItem tsmSemesterMaterials;
        private System.Windows.Forms.ToolStripMenuItem tsmAcademicRecords;
        private System.Windows.Forms.ToolStripMenuItem tsmAttendanceRecords;
        private System.Windows.Forms.ToolStripMenuItem tsmMyInformation;
        private System.Windows.Forms.ToolStripMenuItem tsmChangePassword;
        private System.Windows.Forms.ToolStripMenuItem tsmLogOut;
        private System.Windows.Forms.ToolStripMenuItem tsmMyCourses;
        private System.Windows.Forms.ToolStripMenuItem tsmMySchedule;
        private System.Windows.Forms.ToolStripMenuItem tsmGradesApproval;
        private System.Windows.Forms.ToolStripMenuItem tsmStudentsGrades;
        private System.Windows.Forms.ToolStripMenuItem tsmStudents;
        private System.Windows.Forms.PictureBox PbCoverPhoto;
        private System.Windows.Forms.PictureBox PbLogo;
    }
}