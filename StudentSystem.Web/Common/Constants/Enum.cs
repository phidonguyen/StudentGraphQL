using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Web.Common.Constants
{
    public enum UserStatusEnum
    {
        None = 0,
        [Display(Name = "Waiting")] 
        Waiting = 1,
        [Display(Name = "Banned")] 
        Banned = 2,
        [Display(Name = "Activated")] 
        Activated = 3
    }
}

