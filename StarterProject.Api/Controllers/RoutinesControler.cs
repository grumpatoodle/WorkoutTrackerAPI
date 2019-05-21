using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Routines;
using StarterProject.Api.Features.Routines.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using StarterProject.Api.Infrastructure;

namespace StarterProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class RoutinesController : ControllerBase
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IValidator<RoutineEditDto> _routineEditDtoValidator;
        private readonly IValidator<RoutineCreateDto> _routineCreateDtoValidator;
        private readonly IValidator<RoutineGetDto> _routineGetDtoValidator;
        private readonly IValidator<RoutineDeleteDto> _routineDeleteDtoValidator;

        public RoutinesController(IRoutineRepository routineRepository
                                , IValidator<RoutineEditDto> routineEditDtoValidator
                                , IValidator<RoutineCreateDto> routineCreateDtoValidator
                                , IValidator<RoutineGetDto> routineGetDtoValidator
                                , IValidator<RoutineDeleteDto> routineDeleteDtoValidator)
        {
            _routineRepository = routineRepository;
            _routineEditDtoValidator = routineEditDtoValidator;
            _routineCreateDtoValidator = routineCreateDtoValidator;
            _routineGetDtoValidator = routineGetDtoValidator;
            _routineDeleteDtoValidator = routineDeleteDtoValidator;
        }
        
        [HttpPost("[controller]")]
        [ProducesResponseType(typeof(RoutineGetDto), (int)HttpStatusCode.Created)]
        public IActionResult Post(RoutineCreateDto routineCreateDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            routineCreateDto.UserId = userId;

            var validationResult = _routineCreateDtoValidator.Validate(routineCreateDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var routine = _routineRepository.CreateRoutine(routineCreateDto);
            return Created($"api/routine/{routine.Id}", routine);
        }

        [HttpPut("[controller]/{routineId:int}")]
        [ProducesResponseType(typeof(RoutineGetDto),(int)HttpStatusCode.OK)]
        public IActionResult Put(int routineId, [FromBody] RoutineEditDto routineEditDto)
        {
            routineEditDto.Id = routineId;
            routineEditDto.UserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);

            var validationResult = _routineEditDtoValidator.Validate(routineEditDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var routine = _routineRepository.EditRoutine(routineId, routineEditDto);
            return Ok(routine);
        }

        [HttpDelete("[controller]/{routineId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Delete(int routineId)
        {
            RoutineDeleteDto routineDeleteDto = new RoutineDeleteDto { Id = routineId};

            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            routineDeleteDto.UserId = userId;

            var validationResult = _routineDeleteDtoValidator.Validate(routineDeleteDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            _routineRepository.DeleteRoutine(routineId);
            return Ok();
        }

        [HttpGet("[controller]/{routineId:int}")]
        [ProducesResponseType(typeof(RoutineGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Get(int routineId)
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            var routineGetDto = new RoutineGetDto { Id = routineId, UserId = userId };
            var validationResult = _routineGetDtoValidator.Validate(routineGetDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var routine = _routineRepository.GetRoutine(routineId);
            return Ok(routine);
        }

        [HttpGet("[controller]")]
        [ProducesResponseType(typeof(List<RoutineGetDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            var routine = _routineRepository.GetAllRoutines(userId);
            return Ok(routine);
        }
    }
}
