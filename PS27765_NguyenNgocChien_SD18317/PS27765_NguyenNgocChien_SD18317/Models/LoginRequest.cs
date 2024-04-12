using System.ComponentModel.DataAnnotations;

namespace PS27765_NguyenNgocChien_SD18317.Models
{
    [Serializable]
    public class LoginRequest
    {
        [Required(ErrorMessage = "Bắt buộc điền Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Không được để trống mật khẩu")]
        public string Password { get; set; }
        public string? Roles { get; set; }
        public bool KeepLogIn { get; set; }
    }
}
