using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Exercises;
using StarterProject.Api.Features.Exercises.Dtos;
using StarterProject.Api.Infrastructure;
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
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IValidator<ExerciseEditDto> _exerciseEditDtoValidator;
        private readonly IValidator<ExerciseCreateDto> _exerciseCreateDtoValidator;
        private readonly IValidator<ExerciseGetDto> _exerciseGetDtoValidator;
        private readonly IValidator<ExerciseDeleteDto> _exerciseDeleteDtoValidator;

        public ExerciseController(IExerciseRepository exerciseRepository
                        , IValidator<ExerciseEditDto> exerciseEditDtoValidator
                        , IValidator<ExerciseCreateDto> exerciseCreateDtoValidator
                        , IValidator<ExerciseGetDto> exerciseGetDtoValidator
                        , IValidator<ExerciseDeleteDto> exerciseDeleteDtoValidator)
        {
            _exerciseRepository = exerciseRepository;
            _exerciseEditDtoValidator = exerciseEditDtoValidator;
            _exerciseCreateDtoValidator = exerciseCreateDtoValidator;
            _exerciseGetDtoValidator = exerciseGetDtoValidator;
            _exerciseDeleteDtoValidator = exerciseDeleteDtoValidator;
        }

        [HttpPost("[controller]")]
        [ProducesResponseType(typeof(ExerciseGetDto), (int)HttpStatusCode.Created)]
        public IActionResult Post([FromBody] ExerciseCreateDto exerciseCreateDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            exerciseCreateDto.UserId = userId;

            var validationResult = _exerciseCreateDtoValidator.Validate(exerciseCreateDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var exercise = _exerciseRepository.CreateExercise(exerciseCreateDto);
            return Created($"api/exercise/{exercise.Id}", exercise);
        }

        [HttpPut("[controller]/{exerciseId:int}")]
        [ProducesResponseType(typeof(ExerciseGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Put(int exerciseId, [FromBody] ExerciseEditDto exerciseEditDto)
        {
            exerciseEditDto.Id = exerciseId;
            exerciseEditDto.UserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);

            var validationResult = _exerciseEditDtoValidator.Validate(exerciseEditDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var exercise = _exerciseRepository.EditExercise(exerciseId, exerciseEditDto);
            return Ok(exercise);
        }

        [HttpGet("[controller]")]
        [ProducesResponseType(typeof(List<ExerciseGetDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            var exercise = _exerciseRepository.GetAllExercises(userId);
            return Ok(exercise);
        }

        [HttpGet("[controller]/{exerciseId:int}")]
        [ProducesResponseType(typeof(ExerciseGetDto), (int)HttpStatusCode.OK)]
        public IActionResult Get(int exerciseId)
        {
            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            var exerciseGetDto = new ExerciseGetDto { Id = exerciseId, UserId = userId };
            var validationResult = _exerciseGetDtoValidator.Validate(exerciseGetDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            var exercise = _exerciseRepository.GetExercise(exerciseId);
            return Ok(exercise);
        }

        [HttpDelete("[controller]/{exerciseId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Delete(int exerciseId)
        {
            ExerciseDeleteDto exerciseDeleteDto = new ExerciseDeleteDto { Id = exerciseId };

            var userIdString = User.FindFirst(ClaimTypes.Name)?.Value;
            var userId = int.Parse(userIdString);

            exerciseDeleteDto.UserId = userId;

            var validationResult = _exerciseDeleteDtoValidator.Validate(exerciseDeleteDto);

            if (!validationResult.IsValid)
                return BadRequest(ValidationHelpers.ConvertValidationResult(validationResult));

            _exerciseRepository.DeleteExercise(exerciseId);
            return Ok();
        }
    }
}