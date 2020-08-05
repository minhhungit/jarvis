﻿using Einvoice.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jarvis.Models.Identity.Models.Identity
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(256, ErrorMessage = "Tên đăng nhập dài tối đa 256 ký tự")]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        [StringLength(256, ErrorMessage = "Email tài khoản dài tối đa 256 ký tự")]
        [EmailAttribute(ErrorMessage = "Email tài khoản không đúng định dạng email")]
        public string Email { get; set; }

        public string Metadata { get; set; }

        public CreateUserInfoModel Infos { get; set; }

        public List<Guid> IdRoles { get; set; }
    }

    public class CreateUserInfoModel
    {
        [Required(ErrorMessage = "Họ tên tài khoản không được để trống")]
        public string FullName { get; set; }
    }
}
