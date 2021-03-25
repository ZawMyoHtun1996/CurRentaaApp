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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities3 _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities3();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //select * from TypeOfCars
            //var cars = _db.TypeOfCars.ToList();

            //select id as CarId, name as CarName from TypeOfCars
            /* var cars = _db.TypeOfCars.Select(q => new{ CarId = q.Id, CarName = q.Make}).ToList(); //q =>(Landa expression)*/

            /* var cars = _db.TypeOfCars
                 .Select(q => new 
                 { Make = q.Make
                 , Model = q.Model
                 , VIN = q.VIN
                 , Year = q.Year
                 , LicensePlateNumber = q.LicensePlateNumber
                 , q.Id
                 })
                 .ToList();
             gvVehicleList.DataSource = cars;
             gvVehicleList.Columns[4].HeaderText = "License Plate Number"; 
             gvVehicleList.Columns[5].Visible = false;*/

            /*gvVehicleList.Columns[0].HeaderText = "ID";   //change column header name
            gvVehicleList.Columns[1].HeaderText = "NAME";  //change column header name*/

            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"LoadError: {ex.Message}");
            }
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //query database for recoed
                var car = _db.TypeOfCars.FirstOrDefault(q => q.Id == id);

                //launch AddEditVehicle window with data
                var addEditVehicle = new AddEditVehicle(car, this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"EditError: {ex.Message}");
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //query database for recoed
                var car = _db.TypeOfCars.FirstOrDefault(q => q.Id == id);

                DialogResult dr = MessageBox.Show("Are you sure want to delete this record?",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    //delete vehicle from table
                    _db.TypeOfCars.Remove(car);
                    _db.SaveChanges();
                }
                PopulateGrid();
            }
            catch (Exception err)
            {
                MessageBox.Show($"DeleteError: {err.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Simple Refresh Option
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var cars = _db.TypeOfCars
                .Select(q => new
                {
                    Make = q.Make
                ,
                    Model = q.Model
                ,
                    VIN = q.VIN
                ,
                    Year = q.Year
                ,
                    LicensePlateNumber = q.LicensePlateNumber
                ,
                    q.Id
                })
                .ToList();
            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            gvVehicleList.Columns["Id"].Visible = false;
        }
    }
}
