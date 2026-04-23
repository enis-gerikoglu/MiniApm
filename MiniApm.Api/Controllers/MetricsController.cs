using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniApm.Infrastructure.Persistence;
using MiniApm.Api.Dtos;

namespace MiniApm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController: ControllerBase
{
    private readonly MiniApmDbContext _context;

    public MetricsController(MiniApmDbContext context)
    {
        _context = context;
    }
    [HttpGet("latest")]
    public async Task<IActionResult> GetLatest()
    {
        var data = await _context.SystemMetrics
            .OrderByDescending(x => x.Timestamp)
            .Take(20)
            .Select(x => new MetricsDto
            {
                MachineName = x.MachineName,
                CpuUsage = x.CpuUsage,
                MemoryUsage = x.MemoryUsage,
                DiskUsage = x.DiskUsage,
                Timestamp = x.Timestamp.ToUniversalTime()
            })
            .ToListAsync();

        return Ok(data);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.SystemMetrics
            .OrderByDescending(x => x.Timestamp.ToUniversalTime())
            .ToListAsync());
    }

    [HttpGet("last")]
    public async Task<IActionResult> GetLast()
    {
        var data = await _context.SystemMetrics
            .OrderByDescending(x => x.Timestamp)
            .Select(x => new MetricsDto
            {
                MachineName = x.MachineName,
                CpuUsage = x.CpuUsage,
                MemoryUsage = x.MemoryUsage,
                DiskUsage = x.DiskUsage,
                Timestamp = x.Timestamp.ToUniversalTime()
            })
            .FirstOrDefaultAsync();

        return Ok(data);

        return Ok(data);
    }
    [HttpGet("by-machine")]
    public async Task<IActionResult> GetByMachine(string machineName)
    {
        var data = await _context.SystemMetrics
            .Where(x => x.MachineName == machineName)
            .OrderByDescending(x => x.Timestamp)
            .Take(50)
            .ToListAsync();

        return Ok(data);
    }
    
}