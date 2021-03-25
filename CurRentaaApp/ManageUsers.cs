using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurRentaaApp
{
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities3 _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities3();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                //query database for recoed
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                var hashed_password = Utils.DefaultHashPassword();
                user.password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s password has benn reset!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ResetPasswordError: {ex.Message}");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                //query database for recoed
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                //if(user.isActive == true)
                //  user.isActive = false;
                //else
                //  user.isActive = true;
                user.isActive = user.isActive == true ? false : true ;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s active status has changed.");
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DeActivateError: {ex.Message}");
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = _db.Users
                .Select(q => new
                {
                    q.id,
                    q.username,
                    q.UserRoles.FirstOrDefault().Role.name,
                    q.isActive,
                })
                .ToList();
            gvUserList.DataSource = users;
            gvUserList.Columns["username"].HeaderText = "User Name";
            gvUserList.Columns["name"].HeaderText = "Role Name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            gvUserList.Columns["id"].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
