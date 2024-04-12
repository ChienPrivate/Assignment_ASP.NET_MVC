using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    public class EUser
    {
        [Key]
        public string EUserId { get; set; }
        [Required(ErrorMessage = "Email không thể trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "không thể để trống mật khẩu")]
        public string Pwd { get; set; }
        [Required(ErrorMessage = "Họ tên không thể trống")]
        public string EUserName { get; set; }
        [Required(ErrorMessage = "Không thể để trống ngày sinh")]
        public DateTime Birthday { get; set; }
        public string? EUserAddress { get; set; }
        [Required]
        public string? EUserRole { get; set; }
        public bool IsConfirm { get; set; } = false;

    }
}
