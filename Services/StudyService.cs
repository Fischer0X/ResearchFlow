using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
using ResearchFlow.Models;
using ResearchFlow.ViewModels;
namespace ResearchFlow.Services; 
public class StudyService(ApplicationDbContext db) 
{ 
    public Task<List<ResearchStudy>> List() =>
        db.ResearchStudies.Include(x => x.Phases).AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync();
    public Task<ResearchStudy?> Get(int id) => 
        db.ResearchStudies.Include(x => x.CreatedBy).Include(x => x.Phases).ThenInclude(x => x.AssignedResearcher).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task<int> Create(StudyFormViewModel v, string uid)
    {
        var x = new ResearchStudy 
        {
            Title = v.Title.Trim(),
            Description = v.Description?.Trim(),
            StartDate = v.StartDate,
            ExpectedEndDate = v.ExpectedEndDate,
            CreatedById = uid,
            CreatedAt = DateTime.UtcNow 
        };
        
        db.Add(x);
        
        await db.SaveChangesAsync();
        
        return x.Id;
    }
    public async Task<bool> Update(int id, StudyFormViewModel v) 
    {
        var x = await db.ResearchStudies.FindAsync(id); 
        
        if (x is null)         
            return false; 
        
        x.Title = v.Title.Trim(); 
        
        x.Description = v.Description?.Trim();
        
        x.StartDate = v.StartDate;
        
        x.ExpectedEndDate = v.ExpectedEndDate;
        
        x.UpdatedAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync();
        
        return true; 
    }
    public async Task<string?> Delete(int id) 
    {
        var x = await db.ResearchStudies.Include(x => x.Phases).FirstOrDefaultAsync(x => x.Id == id); 
       
        if (x is null)
            return "غير موجود";
       
        if (x.Phases.Count > 0)
            return "لا يمكن حذف دراسة تحتوي مراحل"; 
      
        db.Remove(x);
        
        await db.SaveChangesAsync();
        
        return null;
    }
}