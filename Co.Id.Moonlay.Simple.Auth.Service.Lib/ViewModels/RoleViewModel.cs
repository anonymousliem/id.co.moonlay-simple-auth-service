﻿using Co.Id.Moonlay.Simple.Auth.Service.Lib.BusinessLogic.Services;
using Co.Id.Moonlay.Simple.Auth.Service.Lib.Models;
using Co.Id.Moonlay.Simple.Auth.Service.Lib.Utilities.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Co.Id.Moonlay.Simple.Auth.Service.Lib.BusinessLogic.Interfaces;

namespace Co.Id.Moonlay.Simple.Auth.Service.Lib.ViewModels
{
    public class RoleViewModel : BaseOldViewModel, IValidatableObject
    {
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public ICollection<PermissionViewModel> permissions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int Count = 0;

            if (string.IsNullOrWhiteSpace(this.code))
                yield return new ValidationResult("Code is required", new List<string> { "code" });

            if (string.IsNullOrWhiteSpace(this.name))
                yield return new ValidationResult("Name is required", new List<string> { "name" });

            /* Service Validation */
            var service = validationContext.GetService<IRoleService>();

            if (service.CheckDuplicate(_id, code)) /* Unique */
            {
                yield return new ValidationResult("Code already exists", new List<string> { "code" });
            }

            string permissionError = "[";

            if(permissions != null)
            {
                foreach (PermissionViewModel permission in permissions)
                {
                    if (string.IsNullOrWhiteSpace(permission.jobTitle.Name))
                    {
                        Count++;
                        permissionError += "{ jobTitle: 'Unit is required' }, ";
                    }
                    else
                    {
                        permissionError += "{}, ";
                    }
                }
            }
            

            permissionError += "]";

            if (Count > 0)
            {
                yield return new ValidationResult(permissionError, new List<string> { "jobTitles" });
            }
        }
    }
}
