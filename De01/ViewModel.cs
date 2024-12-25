using System;

namespace De01
{
    internal class ViewModel
    {
        public string MaSV { get; set; }  // Student ID
        public string HoTenSV { get; set; }  // Full name of the student
        public DateTime NgaySinh { get; set; }  // Birthdate
        public string TenLop { get; set; }  // Class name

        public ViewModel() { }
    }
}
