using System;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task<Member?> GetMemberByIdAsync(string id)
    {
        return await context.Members.FindAsync(id);
    }

    // 1. GetMemberForUpdate: IgnoreQueryFilters add kiya gaya hai taaki unapproved photos bhi delete ho sakein
    public async Task<Member?> GetMemberForUpdate(string id)
    {
        return await context.Members
            .Include(x => x.User)
            .Include(x => x.Photos)
            .IgnoreQueryFilters() // Instruction 13 ke mutabiq
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams)
    {
        var query = context.Members.AsQueryable();

        query = query.Where(x => x.Id != memberParams.CurrentMemberId);

        if (memberParams.Gender != null)
        {
            query = query.Where(x => x.Gender == memberParams.Gender);
        }

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-memberParams.MinAge));

        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        query = memberParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };

        return await PaginationHelper.CreateAsync(query,
                memberParams.PageNumber, memberParams.PageSize);
    }

    // 2. GetPhotosForMemberAsync: Isme query filters ka logic set kiya gaya hai
    public async Task<IEnumerable<Photo>> GetPhotosForMemberAsync(string userId, bool isCurrentUser)
    {
        var query = context.Members
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Photos);

        // Agar user khud apni photos dekh raha hai, toh use unapproved photos bhi dikhni chahiye
        if (isCurrentUser) 
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.ToListAsync();
    }

    public void Update(Member member)
    {
        context.Entry(member).State = EntityState.Modified;
    }
}