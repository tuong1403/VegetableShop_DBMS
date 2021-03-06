﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VegetableShop_DBMS.Views.SignIn;
using VegetableShop_DBMS.Models;
using VegetableShop_DBMS.Controllers;
using System.IO;

namespace VegetableShop_DBMS.Views
{
    public partial class frmSignUp : Form
    {
        string err;
        string iName = "";
        public frmSignUp()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSignIn frm = new frmSignIn();
            frm.ShowDialog();
        }

        private void cbbProvince_Click(object sender, EventArgs e)
        {
            DataSet dataSet = SignUpController.Province_Show();
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0] as DataTable;
            cbbProvince.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                string temp = dr["ProvinceName"].ToString();
                cbbProvince.Items.Add(temp);
            }
        }

        private void cbbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ProvinceNametemp = cbbProvince.SelectedItem.ToString();
            string ProvinceName = "";
            if (ProvinceNametemp.Contains("Tỉnh"))
            {
                ProvinceName = ProvinceNametemp.Substring(5);
            }
            if (ProvinceNametemp.Contains("Thành Phố"))
            {
                ProvinceName = ProvinceNametemp.Substring(10);
            }
            DataTable dt = SignUpController.IDProvince_Find(ProvinceName).Tables[0];
            string IDTemp = dt.Rows[0][0].ToString();
            int ID = int.Parse(IDTemp);

            DataTable District = SignUpController.District_Show(ID).Tables[0];
            cbbDistrict.Items.Clear();
            foreach (DataRow dr in District.Rows)
            {
                string temp = dr["DistrictName"].ToString();
                cbbDistrict.Items.Add(temp);
            }
        }

        private void cbbDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DistrictNametemp = cbbDistrict.SelectedItem.ToString();
            string DistrictName = "";
            if (DistrictNametemp.Contains("Thành Phố"))
            {
                DistrictName = DistrictNametemp.Substring(10);
            }
            if (DistrictNametemp.Contains("Huyện"))
            {
                DistrictName = DistrictNametemp.Substring(6);
            }
            if (DistrictNametemp.Contains("Quận"))
            {
                DistrictName = DistrictNametemp.Substring(5);
            }
            if (DistrictNametemp.Contains("Thị Xã"))
            {
                DistrictName = DistrictNametemp.Substring(7);
            }
            DataTable dt = SignUpController.IDDistrict_Find(DistrictName).Tables[0];
            string IDTemp = dt.Rows[0][0].ToString();
            int ID = int.Parse(IDTemp);

            DataTable Ward = SignUpController.Ward_Show(ID).Tables[0];
            cbbWard.Items.Clear();
            foreach (DataRow dr in Ward.Rows)
            {
                string temp = dr["WardName"].ToString();
                cbbWard.Items.Add(temp);
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string UserName = txtAccount.Text.Trim();
            string PassWord = txtPassword.Text.Trim();
            string FullName = txtFullName.Text.Trim();
            string Gender = cbbGender.SelectedItem.ToString();
            DateTime DateofBirth = dtpDateOfBirth.Value;
            string PhoneNumber = txtPhone.Text.Trim();
            string Email = txtEmail.Text.Trim();
            string Province = cbbProvince.SelectedItem.ToString();
            string District = cbbDistrict.SelectedItem.ToString();
            string Ward = cbbWard.SelectedItem.ToString();
            string Street = txtStreet.Text.Trim();
            if(iName == "")
            {
                iName = "noImageUser.png";
            }
            string Image = iName;
            bool check = SignUpController.Register_Customer(UserName, PassWord, FullName, Gender, DateofBirth, PhoneNumber, Email, Image, Province, District, Ward, Street, ref err);
            if (check == true)
            {
                DialogResult dialogResult;
                dialogResult = MessageBox.Show("Bạn đã tạo tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    this.Hide();
                    frmSignIn form = new frmSignIn();
                    form.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại, xin thử lại lần nữa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                iName = openFile.SafeFileName;
                string filepath = openFile.FileName;
                string fileName = appPath + iName;
                if (File.Exists(fileName) == false)
                {
                    File.Copy(filepath, fileName);
                    ptBImageUser.Image = new Bitmap(fileName);
                }
                else
                {
                    MessageBox.Show("Hình ảnh bạn chọn bị trùng. Vui lòng chọn lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
            }
        }
    }
}
