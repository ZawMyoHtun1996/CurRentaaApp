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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities3 _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities3();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord {
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                //query database for recoed
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                
                var addEditRentalRecord = new AddEditRentalRecord(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"EditError: {ex.Message}");
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //get ID of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                //query database for recoed
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                //delete vehicle from table
                _db.CarRentalRecords.Remove(record);
                _db.SaveChanges();

                PopulateGrid();
            }
            catch (Exception err)
            {
                MessageBox.Show($"DeleteError: {err.Message}");
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"LoadError: {ex.Message}");
            }
        }

        private void PopulateGrid()
        {
            var records = _db.CarRentalRecords
                .Select(q => new
                {
                    Customer = q.CustomerName,
                    DateOut = q.DateRented,
                    DateIn = q.DateReturned,
                    Id = q.id,
                    q.Cost,
                    Car = q.TypeOfCar.Make + " " + q.TypeOfCar.Model
                })
                .ToList();

            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            gvRecordList.Columns["Id"].Visible = false;
        }
    }
}
