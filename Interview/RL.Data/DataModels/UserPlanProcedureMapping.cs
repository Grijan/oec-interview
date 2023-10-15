using RL.Data.DataModels.Common;

namespace RL.Data.DataModels
{
    public class UserPlanProcedureMapping : IChangeTrackable
    {
        public int Id { get; set; }
        public int ProcedureId { get; set; }
        public int PlanId { get; set; }
        public PlanProcedure PlanProcedure { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreateDate { get ; set ; }
        public DateTime UpdateDate { get ; set ; }
    }
}
