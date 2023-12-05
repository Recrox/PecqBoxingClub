using RamDam.BackEnd.Core.Models.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RamDam.BackEnd.Core.Models.Table
{
    [Table("User", Schema = "dbo")]
    public partial class User : ITableObject<Guid>
    {
        //public User()
        //{

        //    UserLogon = new HashSet<UserLogon>();
        //}

        [Key]
        public Guid Id { get; set; }
        
        public string? UserName { get; set; }
        //[Required]
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        public Guid IdSocialNetwork { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public bool WantsNewsLetters { get; set; }
        [StringLength(50)]
        public string IdSocial { get; set; }

        [ForeignKey(nameof(IdSocialNetwork))]
        public virtual SocialNetwork SocialNetwork { get; set; }



        public virtual ICollection<UserLogon> UserLogon { get; set; }
        public ICollection<RoleUser> Roles { get; set; }

    } 
}
