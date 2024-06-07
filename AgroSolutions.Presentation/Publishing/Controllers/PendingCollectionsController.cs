using System.Net.Mime;
using _1_API.Response;
using AutoMapper;
using Domain;
using LearningCenter.Domain.Publishing.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Presentation.Request;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PendingCollectionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPendingCollectionsCommandService _pendingCollectionsCommandService;
    private readonly IPendingCollectionsQueryService _pendingCollectionsQueryService;

    public PendingCollectionsController(IPendingCollectionsQueryService pendingCollectionsQueryService, IPendingCollectionsCommandService pendingCollectionsCommandService,
        IMapper mapper)
    {
        _pendingCollectionsQueryService = pendingCollectionsQueryService;
        _pendingCollectionsCommandService = pendingCollectionsCommandService;
        _mapper = mapper;
    }


    // GET: api/PendingCollections
    ///<summary>Obtain all the active PendingCollections</summary>
    /// <remarks>
    /// GET /api/PendingCollections
    ///   </remarks>
    /// <response code="200">Returns all the PendingCollections</response>
    /// <response code="404">If there are no PendingCollections</response>
    /// <response code="500">If there is an internal server error</response>
    [HttpGet]
    [ProducesResponseType( typeof(List<PendingCollectionsResponse>), 200)]
    [ProducesResponseType( typeof(void),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _pendingCollectionsQueryService.Handle(new GetAllPendingCollectionsQuery());
        
        if (result.Count == 0) return NotFound();

        return Ok(result);
    }

    // GET: api/PendingCollections/Search
    [HttpGet]
    [Route("Search")]
    public async Task<IActionResult> GetSearchAsync(string? name)
    {
        //var data = await _financeRepository.GetSearchAsync(name, year);

        var result = await _pendingCollectionsQueryService.Handle(new GetAllPendingCollectionsQuery());
        if (result.Count == 0) return NotFound();

        return Ok(result);
    }

    // GET: api/PendingCollections/5
    [HttpGet("{id}", Name = "Getter")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var result = await _pendingCollectionsQueryService.Handle(new GetPendingCollectionsByIdQuery(id));

        if (result==null) StatusCode(StatusCodes.Status404NotFound);
        
        return Ok(result);
    }

    // POST: api/PendingCollections
    // POST: api/PendingCollections
    /// <summary>
    /// Creates a new PendingCollections.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/PendingCollections
    ///     {
    ///         "id": "1",
    ///         "type": "Salarios",
    ///         "cost": "5891",
    ///         "description": "Pago de salarios",
    /// 
    ///     }
    ///
    /// </remarks>
    /// <param name="CreatePendingCollectionsCommand">The PendingCollections to create</param>
    /// <returns>A newly created PendingCollections</returns>
    /// <response code="201">Returns the newly created PendingCollections</response>
    /// <response code="400">If the PendingCollections has invalid property</response>
    /// <response code="409">Error validating data</response>
    /// <response code="500">Unexpected error</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> PostAsync([FromBody] CreatePendingCollections command)
    {
        if (!ModelState.IsValid) return BadRequest();


        var result = await _pendingCollectionsCommandService.Handle(command);

        //if (result > 0)
            return StatusCode(StatusCodes.Status201Created, result);

        //return BadRequest();
    }
    /*
    // PUT: api/PendingCollections/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateFinanceCommand command)
    {
        command.Id = id;
        if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);

        var result = await _pendingCollectionsCommandService.Handle(command);

        return Ok();
    }
    */
    // DELETE: api/PendingCollections/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        DeletePendingCollections command = new DeletePendingCollections { Id = id };

        var result = await _pendingCollectionsCommandService.Handle(command);

        return Ok();
    }
}