﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VegetableShop_DBMS.Controllers;


namespace VegetableShop_DBMS.Views
{
    public partial class frmInformationAccount : Form
    {
        public string UserName;
        public string PassWord;
        public string ImageNameEdit = "1";
        public string ImageTemp;
        public string err;
        public frmInformationAccount(string UserName, string PassWord)
        {
            this.UserName = UserName;
            this.PassWord = PassWord;
            InitializeComponent();
            ImageTemp = UserController.ImageUser(UserName).Tables[0].Rows[0][0].ToString();
            DataTable dataTable = UserController.User_Infor(UserName).Tables[0];
            DataRow dr = dataTable.Rows[0];
            txtAccount.Text = dr["UserName"].ToString();
            txtAccount.ReadOnly = true;
            txtFullName.Text = dr["FullName"].ToString();
            txtPhone.Text = dr["PhoneNumber"].ToString();
            txtEmail.Text = dr["Email"].ToString();
            cbbGender.SelectedItem = dr["Gender"].ToString();
            txtUserName.Text = dr["UserName"].ToString();
            dtpDateOfBirth.Value = DateTime.Parse(dr["DateofBirth"].ToString());
            if (ImageTemp != "")
            {
                string appPath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\images\imagesUser\";
                string FileName = appPath + ImageTemp;
                ptBImageUser.Image = Image.FromFile(FileName);
                ptBImageUser.SizeMode = PictureBoxSizeMode.StretchImage;
            }


        }

        private void frmInformationAccount_Load(object sender, EventArgs e)
        {

        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select a Image";
            openFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            string appPath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\images\imagesUser\";
            if (Directory.Exists(appPath) == false)
            {
                Directory.CreateDirectory(appPath);
            }
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                ImageNameEdit = openFile.SafeFileName;
                string filepath = openFile.FileName;
                string fileName = appPath + ImageNameEdit;
                if (File.Exists(fileName) == false)
                {
                    File.Copy(filepath, fileName);
                    ptbImageEdit.Image = new Bitmap(fileName);
                }
                else
                {
                    MessageBox.Show("Hình ảnh bạn chọn bị trùng. Vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSaveInfor_Click(object sender, EventArgs e)
        {
            string UserName = txtAccount.Text.Trim();
            string FullName = txtFullName.Text.Trim();
            string Gender = cbbGender.SelectedItem.ToString();
            DateTime DateofBirth = dtpDateOfBirth.Value;
            string PhoneNumber = txtPhone.Text.Trim();
            string Email = txtEmail.Text.Trim();
            string ImageName = "";
            if (ImageNameEdit == "1")
            {
                ImageName = ImageTemp;
            }
            else
            {
                ImageName = ImageNameEdit;
            }
            bool check = UserController.EditUser(UserName, PassWord, FullName, Gender, DateofBirth, PhoneNumber, Email, ImageName, ref err);
            if (check == true)
            {
                DialogResult dialogResult;
                dialogResult = MessageBox.Show("Bạn đã chỉnh sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    DataTable dataTable = UserController.User_Infor(UserName).Tables[0];
                    DataRow dr = dataTable.Rows[0];
                    txtAccount.Text = dr["UserName"].ToString();
                    txtAccount.ReadOnly = true;
                    txtFullName.Text = dr["FullName"].ToString();
                    txtPhone.Text = dr["PhoneNumber"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    cbbGender.SelectedItem = dr["Gender"].ToString();
                    dtpDateOfBirth.Value = DateTime.Parse(dr["DateofBirth"].ToString());
                    ImageTemp = UserController.ImageUser(UserName).Tables[0].Rows[0][0].ToString();
                    if (ImageTemp != "")
                    {
                        string appPath = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10)) + @"\images\imagesUser\";
                        string FileName = appPath + ImageTemp;
                        ptBImageUser.Image = Image.FromFile(FileName);
                        ptBImageUser.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            else
            {
                MessageBox.Show("Chỉnh sửa thất bại, xin thử lại lần nữa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            this.pnChangePwd.Visible = true;
            this.ptBOpacity.Visible = true;
        }

        private void btnCancelChangePwd_Click(object sender, EventArgs e)
        {
            this.pnChangePwd.Visible = false;
            this.ptBOpacity.Visible = false;
            this.txtOldPwd.Clear();
            this.txtNewPwd.Clear();
            this.txtRePwd.Clear();
        }

        private void btnAcceptChangePwd_Click(object sender, EventArgs e)
        {
            string PassWordOld = this.txtOldPwd.Text;
            string PassWordNew = this.txtNewPwd.Text;
            string RePwd = this.txtRePwd.Text;
            if (RePwd != PassWordNew)
            {
                MessageBox.Show("Mật khẩu nhập lại không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtRePwd.Text = "";        
            }
            else
            {
                bool check = UserController.ChangePassWord(UserName, PassWordOld, PassWordNew, err);
                if (check == true)
                {
                    MessageBox.Show("Đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtNewPwd.Text = "";
                    this.txtOldPwd.Text = "";
                    this.txtRePwd.Text = "";
                }
                else
                {
                    MessageBox.Show("Mật khẩu cũ không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtNewPwd.Text = "";
                    this.txtOldPwd.Text = "";
                    this.txtRePwd.Text = "";
                }
            }    
        }
    }
}
