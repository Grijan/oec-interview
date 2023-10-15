using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Commands.Handlers.Plans
{
    public class AssignUserToPlanProcedureCommandHandler : IRequestHandler<AssignUserToPlanProcedureCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public AssignUserToPlanProcedureCommandHandler(RLContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Unit>> Handle(AssignUserToPlanProcedureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //validate request
                if (request.PlanId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanProcedureId"));
                if (request.UserId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid UserId"));

                var plan = await _context.Plans
                 .Include(p => p.PlanProcedures)
                 .FirstOrDefaultAsync(p => p.PlanId == request.PlanId);

                if (plan is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"PlanId: {request.PlanId} not found"));

                var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);

                if (procedure is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);

                if (user is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"UserId: {request.UserId} not found"));

                var existingUserPlanProcedureMapping = await _context.UserPlanProcedureMappings.FirstOrDefaultAsync(upm => upm.UserId == request.UserId && upm.PlanId == request.PlanId && upm.ProcedureId == request.ProcedureId);

                if (existingUserPlanProcedureMapping != null)
                {
                    // The user is already assigned to the PlanProcedure
                    return ApiResponse<Unit>.Succeed(new Unit());
                }
                var newUserPlanProcedureMappings = new UserPlanProcedureMapping{
                    PlanId = plan.PlanId,
                    ProcedureId = procedure.ProcedureId,
                    UserId = user.UserId
                };
                _context.UserPlanProcedureMappings.Add(newUserPlanProcedureMappings);

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
