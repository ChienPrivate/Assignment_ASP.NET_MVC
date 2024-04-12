using Newtonsoft.Json.Linq;
using PS27765_NguyenNgocChien_SD18317.Data;
using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PS27765_NguyenNgocChien_SD18317.Common
{
    public class CommonMethods
    {
        private readonly ApplicationDbContext _db;
        private const string apiKey = "c9edb9285c810b4fae2b8829e8586f4976b45e55";
        public CommonMethods(ApplicationDbContext db) 
        {
            _db = db;
        }

        public string GenerateNewEUserId()
        {
            if (_db.Product.Count() > 0)
            {
                List<string> userId = _db.EUser.Select(p => p.EUserId).ToList();
                List<int> userIdNumber = userId.Select(p => int.Parse(p.Substring(2))).ToList();
                int idnewNumber = userIdNumber.Max() + 1;

                if (idnewNumber < 10)
                {
                    return "EU" + "00" + idnewNumber;
                }
                else if (idnewNumber < 100)
                {
                    return "EU" + "0" + idnewNumber;
                }
                else
                {
                    return "EU" + idnewNumber;
                }
            }
            return "UE001";
        }

        public string GenerateNewProductId()
        {
            if (_db.Product.Count() > 0)
            {
                List<string> productId = _db.Product.Select(p => p.ProductId).ToList();
                List<int> productIdNumber = productId.Select(p => int.Parse(p.Substring(1))).ToList();
                int idnewNumber = productIdNumber.Max() + 1;

                if (idnewNumber < 10)
                {
                    return "P" + "00" + idnewNumber;
                }
                else if (idnewNumber < 100)
                {
                    return "P" + "0" + idnewNumber;
                }
                else
                {
                    return "P" + idnewNumber;
                }
            }
            return "P001";
        }

        public string GenerateNewCartId()
        {
            if (_db.Cart.Count() > 0)
            {
                List<string> cartId = _db.Cart.Select(p => p.CartId).ToList();
                List<int> cartIdNumber = cartId.Select(p => int.Parse(p.Substring(1))).ToList();
                int idnewNumber = cartIdNumber.Max() + 1;

                if (idnewNumber < 10)
                {
                    return "C" + "00" + idnewNumber;
                }
                else if (idnewNumber < 100)
                {
                    return "C" + "0" + idnewNumber;
                }
                else
                {
                    return "C" + idnewNumber;
                }
            }
            return "C001";
        }

        public string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                sb.Append(characters[index]);
            }

            return sb.ToString();
        }

        public string Encryption(string pwd)
        {
            byte[] pwdBytes = Encoding.UTF8.GetBytes(pwd);
            MD5 md5 = MD5.Create();
            byte[] encryptBytes = md5.ComputeHash(pwdBytes);
            StringBuilder pwdString = new StringBuilder();
            for (int i = 0; i < encryptBytes.Length; i++)
            {
                pwdString.Append(encryptBytes[i].ToString("x2"));
            }
            return pwdString.ToString();
        }

        public string SendKeyToMail(string to, string subject, string content)
        {
            string from = "chienprivate@gmail.com";
            string pass = "aohm alki wbdn zgeu";
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            msg.To.Add(to);
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = $@"
<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Your Secret Key</title>
    <style>
        .container {{
            width: 100%;
            max-width: 600px;
            margin: auto;
            padding: 20px;
            background-color: #1d0404;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(54, 134, 148, 0.1);
        }}
        .text-center{{
            text-align: center;
        }}
        .text-white{{
            color: white;
        }}
        .bg-white{{
            background-color: white;
        }}
        .fs-1{{
            font-size: large;
        }}
    </style>
</head>
<body>
    <div class=""container bg-white"">
        <h2>Thank you for sign up !</h2>
        <span>please do not share this key to anyone</span>
        <br>
        <span>this key will be exspired after 5 mintutes from now</span>
    </div>
    <div class=""container"">
        <div class=""text-center"">
            <h1 class=""text-white"">Your Secret Key</h1>
        </div>
        <div class=""text-center bg-white fs-1"">
            <h2>{content}</h2>
        </div>
    </div>
    
</body>

</html>";
            ;

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(from, pass);
            try
            {
                client.Send(msg);
                return "gửi mail thành công";
            }
            catch (Exception ex)
            {
                return "Gửi mail thất bại \n" + ex.Message;
            }
        }

        public string SendNewPasswordToMail(string to, string subject, string content)
        {
            string from = "chienprivate@gmail.com";
            string pass = "aohm alki wbdn zgeu";
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            msg.To.Add(to);
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = $@"
<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Your Secret Key</title>
    <style>
        .container {{
            width: 100%;
            max-width: 600px;
            margin: auto;
            padding: 20px;
            background-color: #1d0404;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(54, 134, 148, 0.1);
        }}
        .text-center{{
            text-align: center;
        }}
        .text-white{{
            color: white;
        }}
        .bg-white{{
            background-color: white;
        }}
        .fs-1{{
            font-size: large;
        }}
    </style>
</head>
<body>
    <div class=""container bg-white"">
        <h2>Thank you for believing in our service !</h2>
        <span>please do not share this string to anyone</span>
        <br>
        <span>To protect and memorize your account,after logging in, please remember to change your password. </span>
    </div>
    <div class=""container"">
        <div class=""text-center"">
            <h1 class=""text-white"">Your New Password</h1>
        </div>
        <div class=""text-center bg-white fs-1"">
            <h2>{content}</h2>
        </div>
    </div>
    
</body>

</html>";
            ;

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(from, pass);
            try
            {
                client.Send(msg);
                return "gửi mail thành công";
            }
            catch (Exception ex)
            {
                return "Gửi mail thất bại \n" + ex.Message;
            }
        }

        public async Task<string> CheckEmail(string email)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://api.hunter.io/v2/email-verifier?email={email}&api_key={apiKey}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        // phân tích JSON response
                        var data = JObject.Parse(jsonResponse);
                        string status = data["data"]["result"].ToString();

                        if (status.Equals("deliverable", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Email hợp lệ";
                        }
                        else if (status.Equals("undeliverable", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Không thể gửi mail cho địa chỉ email này\n hãy kiểm tra lại địa chỉ email";
                        }
                        else
                        {
                            return "Trạng thái email không xác định \n hãy kiểm tra lại email";
                        }
                    }
                    else
                    {
                        return "Địa chỉ email không hợp lệ";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
