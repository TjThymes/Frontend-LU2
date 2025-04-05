using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    // 🔹 **Supporting Classes**
    [System.Serializable]
    public class AuthResponse
    {
        public string accessToken;
        public string userId;
    }

    [System.Serializable]
    public class RegisterModel
    {
        public string Name;
        public string Email;
        public string Password;
    }

    [System.Serializable]
    public class LoginModel
    {
        public string UserName;
        public string Password;
    }

}
