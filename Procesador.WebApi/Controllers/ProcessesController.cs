using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Procesador.Interfaces;
using System;
using System.Linq;

namespace Procesador.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessesController : ControllerBase
    {
        private readonly ILogger<ProcessesController> _logger;
        private readonly IProcessService processService;

        public ProcessesController(ILogger<ProcessesController> logger, IProcessService processService)
        {
            _logger = logger;
            this.processService = processService;
        }

        [HttpGet]
        public IActionResult GetProcesses()
        {
            var processes = processService.GetAll();
            return Ok(processes);
        }

        [HttpPost]
        public CreatedResult CreateProcess()
        {
            Guid processIdentifier = Guid.Empty;

            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    processIdentifier = processService.Create(file.OpenReadStream());
                }
            }

            return Created("", processIdentifier);
        }
    }
}
