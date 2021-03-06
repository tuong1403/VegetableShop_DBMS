﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VegetableShop_DBMS.Controllers;
namespace VegetableShop_DBMS.Views
{
    public partial class frmShipping : Form
    {
        public string UserName;
        public string DefaultAddress;
        public string PassWord;
        string err;
        public frmShipping(string UserName,string PassWord, string DefaultAddress, string PhoneNumber, string FullName)
        {
            this.UserName = UserName;
            this.DefaultAddress = DefaultAddress;
            this.PassWord = PassWord;
            InitializeComponent();
            this.lblAddress.Text = DefaultAddress;
            this.lblFullName.Text = FullName;
            this.lblPhoneNumber.Text = PhoneNumber;
            this.lblShowPrice.Text = OrderItemsController.ShowTotalPrice(UserName, PassWord).Tables[0].Rows[0][0].ToString();
            DataTable dtAllBillDetails = OrderItemsController.ShowCart(UserName).Tables[0];
            int ylbl = 10;
            foreach(DataRow dr in dtAllBillDetails.Rows)
            {
                Guna.UI.WinForms.GunaLabel lbl = new Guna.UI.WinForms.GunaLabel();
                lbl.Location = new Point(8, ylbl);
                lbl.Font = new Font("Tahoma", 9, FontStyle.Bold);
                lbl.Text = dr["ItemName"].ToString() + " x " + dr["Quantity"].ToString() + " : \t" + dr["PaidPrice"].ToString();
                lbl.AutoSize = true;
                pnAllBillDetails.Controls.Add(lbl);

                ylbl += 24;
            }
        }

        private void btnUpdateItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmShoppingCart frmShopping = new frmShoppingCart(UserName, PassWord);
            frmShopping.ShowDialog();
            this.Close();
        }

        private void btnUpdateAddress_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDefaultAddress frmDefault = new frmDefaultAddress(UserName, PassWord, DefaultAddress);
            frmDefault.ShowDialog();
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            bool check = OrderItemsController.OrderItem(UserName, PassWord, ref err);
            if (check == true)
            {
                DialogResult dialogResult;
                dialogResult = MessageBox.Show("Bạn đã đặt hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    this.Hide();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Chỉnh sửa thất bại, xin thử lại lần nữa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
