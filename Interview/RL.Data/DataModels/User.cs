using System.ComponentModel.DataAnnotations;
using RL.Data.DataModels.Common;

namespace RL.Data.DataModels;

public class User : IChangeTrackable
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; }
    public ICollection<UserPlanProcedureMapping> UserPlanProcedureMapping { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}