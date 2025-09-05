namespace SMS
{
    partial class frmStudentHomePage
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
            this.tsmStudyPlan = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAcademicRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCourseRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmStudySchedule = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTestAppointment = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPayments = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAccountSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMyInfo = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tsmStudyPlan,
            this.tsmAcademicRecord,
            this.tsmCourseRegistration,
            this.tsmStudySchedule,
            this.tsmTestAppointment,
            this.tsmPayments,
            this.tsmAccountSetting});
            this.msMainMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.msMainMenu.Location = new System.Drawing.Point(0, 0);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.msMainMenu.Size = new System.Drawing.Size(1151, 72);
            this.msMainMenu.TabIndex = 0;
            this.msMainMenu.Text = "menuStrip1";
            // 
            // tsmStudyPlan
            // 
            this.tsmStudyPlan.AutoSize = false;
            this.tsmStudyPlan.DoubleClickEnabled = true;
            this.tsmStudyPlan.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsmStudyPlan.Image = global::SMS.Properties.Resources.Study_Plan_64;
            this.tsmStudyPlan.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmStudyPlan.Name = "tsmStudyPlan";
            this.tsmStudyPlan.Size = new System.Drawing.Size(139, 72);
            this.tsmStudyPlan.Text = "Study Plan";
            // 
            // tsmAcademicRecord
            // 
            this.tsmAcademicRecord.AutoSize = false;
            this.tsmAcademicRecord.DoubleClickEnabled = true;
            this.tsmAcademicRecord.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAcademicRecord.Image = global::SMS.Properties.Resources.Academic_Record_64;
            this.tsmAcademicRecord.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAcademicRecord.Name = "tsmAcademicRecord";
            this.tsmAcademicRecord.Size = new System.Drawing.Size(172, 72);
            this.tsmAcademicRecord.Text = "Academic Record";
            // 
            // tsmCourseRegistration
            // 
            this.tsmCourseRegistration.AutoSize = false;
            this.tsmCourseRegistration.DoubleClickEnabled = true;
            this.tsmCourseRegistration.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmCourseRegistration.Image = global::SMS.Properties.Resources.Registration_Materials_64;
            this.tsmCourseRegistration.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmCourseRegistration.Name = "tsmCourseRegistration";
            this.tsmCourseRegistration.Size = new System.Drawing.Size(185, 72);
            this.tsmCourseRegistration.Text = "Course Registration";
            // 
            // tsmStudySchedule
            // 
            this.tsmStudySchedule.AutoSize = false;
            this.tsmStudySchedule.DoubleClickEnabled = true;
            this.tsmStudySchedule.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmStudySchedule.Image = global::SMS.Properties.Resources.Study_Schedule_64;
            this.tsmStudySchedule.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmStudySchedule.Name = "tsmStudySchedule";
            this.tsmStudySchedule.Size = new System.Drawing.Size(163, 72);
            this.tsmStudySchedule.Text = "Study Schedule";
            // 
            // tsmTestAppointment
            // 
            this.tsmTestAppointment.AutoSize = false;
            this.tsmTestAppointment.DoubleClickEnabled = true;
            this.tsmTestAppointment.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmTestAppointment.Image = global::SMS.Properties.Resources.Test_Appointment_64;
            this.tsmTestAppointment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmTestAppointment.Name = "tsmTestAppointment";
            this.tsmTestAppointment.Size = new System.Drawing.Size(176, 72);
            this.tsmTestAppointment.Text = "Test Appointment";
            // 
            // tsmPayments
            // 
            this.tsmPayments.AutoSize = false;
            this.tsmPayments.DoubleClickEnabled = true;
            this.tsmPayments.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmPayments.Image = global::SMS.Properties.Resources.Payments_64;
            this.tsmPayments.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmPayments.Name = "tsmPayments";
            this.tsmPayments.Size = new System.Drawing.Size(134, 72);
            this.tsmPayments.Text = "Payments";
            // 
            // tsmAccountSetting
            // 
            this.tsmAccountSetting.AutoSize = false;
            this.tsmAccountSetting.DoubleClickEnabled = true;
            this.tsmAccountSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMyInfo,
            this.tsmChangePassword,
            this.tsmLogOut});
            this.tsmAccountSetting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmAccountSetting.Image = global::SMS.Properties.Resources.Account_Setting_64;
            this.tsmAccountSetting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAccountSetting.Name = "tsmAccountSetting";
            this.tsmAccountSetting.Size = new System.Drawing.Size(166, 72);
            this.tsmAccountSetting.Text = "Account Setting";
            // 
            // tsmMyInfo
            // 
            this.tsmMyInfo.BackColor = System.Drawing.Color.White;
            this.tsmMyInfo.DoubleClickEnabled = true;
            this.tsmMyInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmMyInfo.Image = global::SMS.Properties.Resources.My_Info_32;
            this.tsmMyInfo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmMyInfo.Name = "tsmMyInfo";
            this.tsmMyInfo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.tsmMyInfo.Size = new System.Drawing.Size(196, 38);
            this.tsmMyInfo.Text = "My Information";
            // 
            // tsmChangePassword
            // 
            this.tsmChangePassword.BackColor = System.Drawing.Color.White;
            this.tsmChangePassword.DoubleClickEnabled = true;
            this.tsmChangePassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmChangePassword.Image = global::SMS.Properties.Resources.password_32;
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
            this.tsmLogOut.Image = global::SMS.Properties.Resources.Log_Out_32;
            this.tsmLogOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmLogOut.Name = "tsmLogOut";
            this.tsmLogOut.Size = new System.Drawing.Size(196, 38);
            this.tsmLogOut.Text = "Log Out";
            // 
            // PbCoverPhoto
            // 
            this.PbCoverPhoto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PbCoverPhoto.Image = global::SMS.Properties.Resources.Cover_Photo___Student_Management_System1;
            this.PbCoverPhoto.Location = new System.Drawing.Point(0, 72);
            this.PbCoverPhoto.Name = "PbCoverPhoto";
            this.PbCoverPhoto.Size = new System.Drawing.Size(1151, 447);
            this.PbCoverPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbCoverPhoto.TabIndex = 2;
            this.PbCoverPhoto.TabStop = false;
            // 
            // PbLogo
            // 
            this.PbLogo.Image = global::SMS.Properties.Resources.NextGen_University_Logo;
            this.PbLogo.Location = new System.Drawing.Point(19, 94);
            this.PbLogo.Name = "PbLogo";
            this.PbLogo.Size = new System.Drawing.Size(111, 113);
            this.PbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbLogo.TabIndex = 15;
            this.PbLogo.TabStop = false;
            // 
            // frmStudentHomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1151, 519);
            this.Controls.Add(this.PbLogo);
            this.Controls.Add(this.PbCoverPhoto);
            this.Controls.Add(this.msMainMenu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msMainMenu;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmStudentHomePage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Home Page";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbCoverPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMainMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmStudyPlan;
        private System.Windows.Forms.ToolStripMenuItem tsmAcademicRecord;
        private System.Windows.Forms.ToolStripMenuItem tsmCourseRegistration;
        private System.Windows.Forms.ToolStripMenuItem tsmStudySchedule;
        private System.Windows.Forms.ToolStripMenuItem tsmTestAppointment;
        private System.Windows.Forms.ToolStripMenuItem tsmPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmAccountSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmChangePassword;
        private System.Windows.Forms.ToolStripMenuItem tsmLogOut;
        private System.Windows.Forms.PictureBox PbCoverPhoto;
        private System.Windows.Forms.PictureBox PbLogo;
        private System.Windows.Forms.ToolStripMenuItem tsmMyInfo;
    }
}