using System;
using System.Collections.Generic;

namespace Model.SystemModel
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TrueName { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDel { get; set; } = false;
        public List<RoleModel> UserRole { get; set; }
        public List<Guid> RoleId { get; set; }
        public Guid? CreateUser { get; set; }
        public DateTime? CreateTime { get; set; }
        public Guid? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class UserFilter : PageModel
    {
        public string UserName { get; set; }
    }

    public class ChangePwdModel
    {
        public string CurrentPwd { get;set;}

        public string NewPwd { get; set; }

        public string ConfirmPwd { get; set; }
    }
}
