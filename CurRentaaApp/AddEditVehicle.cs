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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities3 _db;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            _db = new CarRentalEntities3();
        }

        public AddEditVehicle(TypeOfCar carToEdit, ManageVehicleListing manageVehicleListing = null) {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities3();
                PopulateFields(carToEdit);
            }
        }

        private void PopulateFields(TypeOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicenseNum.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Add validation for make and model
                if (string.IsNullOrWhiteSpace(tbMake.Text) ||
                    string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please ensure that you provide a make and a model");
                }
                else
                {
                    //if (isEditMode == true)
                    if (isEditMode)
                    {
                        //Edit code here
                        var id = int.Parse(lblId.Text);
                        var car = _db.TypeOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLicenseNum.Text;

                       /* _db.SaveChanges();
                        Close();*/
                    }
                    else
                    {
                        //Add code here
                        var newCar = new TypeOfCar
                        {
                            LicensePlateNumber = tbLicenseNum.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text)
                        };

                        _db.TypeOfCars.Add(newCar);
                    }
                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"SaveError: {err.Message}");
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
