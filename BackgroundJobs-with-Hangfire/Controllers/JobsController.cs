using BackgroundJobs_with_Hangfire.Data;
using BackgroundJobs_with_Hangfire.Models;
using BackgroundJobs_with_Hangfire.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs_with_Hangfire.Controllers;

[Route("api/[controller]"), ApiController]
public class JobsController : ControllerBase
{
    private readonly AppDataContext _context;

    public JobsController(AppDataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddDriver(Driver driver)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        await _context.AddAsync(driver);
        await _context.SaveChangesAsync();
        BackgroundJob.Enqueue<IServiceManagement>(s => s.SendMail());
        return CreatedAtAction("GetDriver", new { driver.Id }, driver);
    }

    [HttpGet]
    public async Task<IActionResult> GetDriver(Guid id)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);
        if (driver == null) return NotFound();
        return Ok(driver);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDriver(Guid id)
    {
        var existDriver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);
        if (existDriver == null) return NotFound();
        existDriver.Status = 0;
        _context.Drivers.Remove(existDriver);
        await _context.SaveChangesAsync();
        RecurringJob.AddOrUpdate<IServiceManagement>(s => s.UpdateDatabase(), Cron.Minutely);
        return NoContent();
    }
}