using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        public frmSinhVien()
        {
            InitializeComponent();
        }

        Model1 db = new Model1();
        public List<Sinhvien> listmodel = new List<Sinhvien>();

        // Phương thức load dữ liệu
        public void LoadData()
        {
            try
            {
                // Lấy danh sách lớp từ database
                var lop = db.Lops.ToList();
                FillClassCombobox(lop);

                // Lấy danh sách sinh viên từ database
                listmodel = db.Sinhviens.Include("Lop").ToList();

                // Gắn dữ liệu vào DataGridView
                BindGrid(listmodel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void BindGrid(List<Sinhvien> listStudents)
        {
            dgvStudent.DataSource = null;
            dgvStudent.DataSource = listStudents;
        }

        private void FillClassCombobox(List<Lop> listClasses)
        {
            cboLop.DataSource = listClasses;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }

        private bool CheckStudentID(string mssv)
        {
            return listmodel.Any(s => s.MaSV == mssv);
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                string mssv = txtMaSV.Text.Trim();
                string fullname = txtHotenSV.Text.Trim();
                DateTime birthday = dtNgaySinh.Value;
                string malop = cboLop.SelectedValue.ToString();

                if (string.IsNullOrEmpty(mssv) || string.IsNullOrEmpty(fullname))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (CheckStudentID(mssv))
                {
                    MessageBox.Show("Sinh viên đã tồn tại trong danh sách!");
                    return;
                }

                // Thêm sinh viên mới vào danh sách
                Sinhvien newSV = new Sinhvien
                {
                    MaSV = mssv,
                    HoTenSV = fullname,
                    NgaySinh = birthday,
                    MaLop = malop,
                    Lop = db.Lops.FirstOrDefault(l => l.MaLop == malop)
                };

                listmodel.Add(newSV);
                BindGrid(listmodel);

                MessageBox.Show("Thêm sinh viên thành công!");
                btLuu.Enabled = true;
                btKhong.Enabled = true;
                ResetInputField();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message);
            }
        }

        private void ResetInputField()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtNgaySinh.Value = DateTime.Now;
            cboLop.SelectedIndex = 0;
        }
        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve student ID from input field
                string mssv = txtMaSV.Text.Trim();
                if (string.IsNullOrEmpty(mssv))
                {
                    MessageBox.Show("Vui lòng nhập mã số sinh viên để xóa!");
                    return;
                }

                // Find the student in the list
                Sinhvien sv = listmodel.FirstOrDefault(s => s.MaSV == mssv);
                if (sv == null)
                {
                    MessageBox.Show("Sinh viên không tồn tại!");
                    return;
                }

                // Ask for confirmation before deleting
                DialogResult res = MessageBox.Show("Bạn có muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    // Remove the student from the list
                    listmodel.Remove(sv);

                    // If you are using a database, you might need to remove from the database as well
                    db.Sinhviens.Remove(sv);  // Remove from the database
                    db.SaveChanges();  // Save the changes to the database

                    // Rebind the updated list to the DataGridView
                    BindGrid(listmodel);

                    MessageBox.Show("Xóa sinh viên thành công!");
                    ResetInputField();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
            }
        }
        private void btSua_Click(object sender, EventArgs e)
        {
            string mssv = txtMaSV.Text.Trim();
            if (string.IsNullOrEmpty(mssv))
            {
                MessageBox.Show("Vui lòng nhập mã số sinh viên!");
                return;
            }

            Sinhvien sv = listmodel.FirstOrDefault(s => s.MaSV == mssv);
            if (sv == null)
            {
                MessageBox.Show("Sinh viên không tồn tại!");
                return;
            }

            DialogResult res = MessageBox.Show("Bạn có muốn cập nhật thông tin sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                sv.HoTenSV = txtHotenSV.Text.Trim();
                sv.NgaySinh = dtNgaySinh.Value;
                sv.MaLop = cboLop.SelectedValue.ToString();
                BindGrid(listmodel);
                MessageBox.Show("Cập nhật dữ liệu thành công!");
                ResetInputField();
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var sv in listmodel)
                {
                    var existingSV = db.Sinhviens.FirstOrDefault(s => s.MaSV == sv.MaSV);
                    if (existingSV == null)
                    {
                        db.Sinhviens.Add(sv);
                    }
                    else
                    {
                        existingSV.HoTenSV = sv.HoTenSV;
                        existingSV.NgaySinh = sv.NgaySinh;
                        existingSV.MaLop = sv.MaLop;
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Lưu dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            // Show confirmation message box
            DialogResult result = MessageBox.Show("Bạn muốn thoát chương trình không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If the user clicks "Yes", close the form
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string searchName = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(searchName))
            {
                MessageBox.Show("Vui lòng nhập họ hoặc tên sinh viên để tìm kiếm!");
                return;
            }

            using (Model1 context = new Model1())
            {
                // Tìm kiếm sinh viên theo họ hoặc tên
                var filteredStudents = context.Sinhviens
                    .Where(s => s.HoTenSV.ToLower().Contains(searchName.ToLower()))
                    .ToList();

                if (filteredStudents.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sinh viên nào có tên phù hợp!");
                }
                else
                {
                    BindGrid(filteredStudents);
                }
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ensure the clicked row is valid (not the header row)
                if (e.RowIndex >= 0)
                {
                    // Get the selected row in the DataGridView
                    DataGridViewRow row = dgvStudent.Rows[e.RowIndex];

                    // Retrieve the student's data from the selected row
                    string mssv = row.Cells[0].Value.ToString();
                    string fullName = row.Cells[1].Value.ToString();
                    DateTime birthDate = Convert.ToDateTime(row.Cells[2].Value);
                    string classCode = row.Cells[3].Value.ToString();

                    // Populate the input fields with the student's data
                    txtMaSV.Text = mssv;
                    txtHotenSV.Text = fullName;
                    dtNgaySinh.Value = birthDate;
                    cboLop.SelectedValue = classCode;

                    // Enable the Edit and Delete buttons when a student is selected
                    btSua.Enabled = true;
                    btXoa.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn sinh viên: " + ex.Message);
            }
        }

        
    }
}
