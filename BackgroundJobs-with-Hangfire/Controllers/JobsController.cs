using BackgroundJobs_with_Hangfire.Data;
using BackgroundJobs_with_Hangfire.Models;
using BackgroundJobs_with_Hangfire.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJobs_with_Hangfire.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly AppDataContext _context;
    public JobsController(AppDataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddDriver(Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        _context.Add(driver);
        _context.SaveChanges();
        var jobId = BackgroundJob.Enqueue<IServiceManagement>(s => s.SendMail());
        return CreatedAtAction("GetDriver", new { driver.Id }, driver);
    }

    [HttpGet]
    public IActionResult GetDriver(Guid id)
    {
        var driver = _context.Drivers.FirstOrDefault(d => d.Id == id);
        if (driver == null) { return NotFound(); }
        return Ok(driver);
    }

    [HttpDelete]
    public IActionResult DeleteDriver(Guid id)
    {
        var existDriver = _context.Drivers.FirstOrDefault(d => d.Id == id);
        if (existDriver == null) { return NotFound(); }
        existDriver.Status = 0;
        _context.Drivers.Remove(existDriver);
        _context.SaveChanges();
        RecurringJob.AddOrUpdate<IServiceManagement>(s => s.UpdateDatabase(), Cron.Minutely);
        return NoContent();
    }
}
