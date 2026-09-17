using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
using ResearchFlow.Models;
using ResearchFlow.ViewModels;
namespace ResearchFlow.Services;

public class PhaseService(ApplicationDbContext db)
{
    public Task<ResearchPhase?> Get(int id)
        => db.ResearchPhases
        .Include(x => x.ResearchStudy)
        .Include(x => x.ParentPhase)
        .Include(x => x.ChildPhases)
        .Include(x => x.AssignedResearcher)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);
    public async Task Fill(PhaseFormViewModel v, int? edit = null)
    {
        v.Researchers = 
            await db.Users.OrderBy(x => x.FullName).Select(x =>
            new SelectListItem(x.FullName, x.Id)).ToListAsync();
        
        v.ParentPhases =
            await db.ResearchPhases.Where(x => x.ResearchStudyId == v.ResearchStudyId && x.ParentPhaseId == null && x.Id != edit).Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToListAsync();
    }
    async Task Validate(PhaseFormViewModel v, int? id = null)
    {
        if (!await db.ResearchStudies.AnyAsync(x => x.Id == v.ResearchStudyId))
            throw new InvalidOperationException("الدراسة غير موجودة");
       
        if (v.ParentPhaseId is not null && !await db.ResearchPhases.AnyAsync(x => x.Id == 
v.ParentPhaseId && x.ResearchStudyId == v.ResearchStudyId && x.ParentPhaseId == null && x.Id != id))
          
            throw new InvalidOperationException("المرحلة الرئيسية غير صالحة");
        if (v.AssignedResearcherId is not null && !await db.Users.AnyAsync(x => x.Id == v.AssignedResearcherId))
            throw new InvalidOperationException("الباحث غير موجود");
    }
    public async Task<int> Create(PhaseFormViewModel v)
    {
        await Validate(v);
        var x = new ResearchPhase
        {
            ResearchStudyId = v.ResearchStudyId,
            ParentPhaseId = v.ParentPhaseId,
            AssignedResearcherId = v.AssignedResearcherId,
            Title = v.Title.Trim(),
            Description = v.Description?.Trim(),
            Priority = v.Priority,
            CreatedAt = DateTime.UtcNow
        };
        db.Add(x);
        await db.SaveChangesAsync();
        return x.Id;
    }
    public async Task<bool> Update(int id, PhaseFormViewModel v)
    {
        var x = await db.ResearchPhases.FindAsync(id);
        if (x is null)
            return false;

        await Validate(v, id);

        x.Title = v.Title.Trim();

        x.Description = v.Description?.Trim();
        
        x.Priority = v.Priority;
        
        x.ParentPhaseId = v.ParentPhaseId;
        
        x.AssignedResearcherId = v.AssignedResearcherId;
        
        x.UpdatedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<string?> Delete(int id)
    {
        var x = 
            await db.ResearchPhases.Include(x => x.ChildPhases).FirstOrDefaultAsync(x => x.Id == id);
        if (x is null)
            return "غير موجود";
        
        if (x.ChildPhases.Count > 0)
            return "احذف الإجراءات الفرعية أولًا";
        
        db.Remove(x);
        
        await db.SaveChangesAsync();
        
        return null;
    }
    public async Task<bool> Change(int id, ResearchStatus status, string uid, string? note)
    {
        var x = await db.ResearchPhases.FindAsync(id);
        if (x is null) return false;
        
        if (x.Status == status)
            return true;
        
        var old = x.Status;
        
        await using var tx = await db.Database.BeginTransactionAsync();
        try
        {
            x.Status = status;
        
            x.UpdatedAt = DateTime.UtcNow;
            
            db.ActivityLogs.Add(
                new() 
                { 
                    ResearchPhaseId = id, ChangedByUserId = uid, OldStatus = old, 
                    NewStatus = status, Note = note, ChangedAt = DateTime.UtcNow 
                });
            await db.SaveChangesAsync();
            
            await tx.CommitAsync();
            
            return true;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}