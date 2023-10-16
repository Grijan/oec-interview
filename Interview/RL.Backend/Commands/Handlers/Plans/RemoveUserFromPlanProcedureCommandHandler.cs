using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;

namespace RL.Backend.Commands.Handlers.Plans
{
    public class RemoveUserFromPlanProcedureCommandHandler : IRequestHandler<RemoveUserFromPlanProcedureCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public RemoveUserFromPlanProcedureCommandHandler(RLContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<Unit>> Handle(RemoveUserFromPlanProcedureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.PlanId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanProcedureId"));
                

                var plan = await _context.Plans
                     .Include(p => p.PlanProcedures)
                     .FirstOrDefaultAsync(p => p.PlanId == request.PlanId);

                if (plan is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"PlanId: {request.PlanId} not found"));

                var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);

                if (procedure is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);

                var userId = request.UserId;
                if (userId == null)
                {
                    // remove all users
                    var assingedUserToPlanProcedure = await _context.UserPlanProcedureMappings.Where(upm => upm.PlanId == plan.PlanId && upm.ProcedureId == procedure.ProcedureId).ToListAsync();
                    _context.UserPlanProcedureMappings.RemoveRange(assingedUserToPlanProcedure);
                }
                else
                {
                    // remove that particular user
                    if (user is null)
                        return ApiResponse<Unit>.Fail(new NotFoundException($"UserId: {request.UserId} not found"));

                    var assingedUserToPlanProcedure = await _context.UserPlanProcedureMappings.FirstOrDefaultAsync(upm => upm.PlanId == plan.PlanId && upm.ProcedureId == procedure.ProcedureId && upm.UserId == user.UserId);
                    if (assingedUserToPlanProcedure != null)
                    {
                        _context.UserPlanProcedureMappings.Remove(assingedUserToPlanProcedure);
                    }
                }
                await _context.SaveChangesAsync();
                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Fail(ex);
            }
        }
    }
}
