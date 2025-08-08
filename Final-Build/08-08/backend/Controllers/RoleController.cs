using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models.DTOs;

namespace VehicleServiceAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            _logger.LogInformation("GetAllRoles called.");
            try
            {
            var roles = await _roleService.GetAllRolesAsync();
            _logger.LogInformation("Retrieved {Count} roles.", roles.Count());
            return Ok(roles);
            }
            catch (InvalidOperationException ex)
            {
            _logger.LogWarning(ex, "Invalid operation in GetAllRoles: {Message}", ex.Message);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger.LogWarning(ex, "Unauthorized access in GetAllRoles: {Message}", ex.Message);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Unexpected error in GetAllRoles: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Retrieves a role by its ID.
        /// </summary>
        /// <param name="id">The role's ID.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        {
            _logger.LogInformation("GetRoleById called with id: {Id}", id);
            try
            {
            var role = await _roleService.GetRoleByIdAsync(id);
            _logger.LogInformation("Role with id {Id} retrieved successfully.", id);
            return Ok(role);
            }
            catch (InvalidOperationException ex)
            {
            _logger.LogWarning(ex, "Invalid operation in GetRoleById: {Message}", ex.Message);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger.LogWarning(ex, "Unauthorized access in GetRoleById: {Message}", ex.Message);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Unexpected error in GetRoleById: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="request">The request object containing the role name.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<RoleDTO>> CreateRole([FromBody] RoleRequest request)
        {
            _logger.LogInformation("CreateRole called with role: {Role}", request?.Role);

            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
            _logger.LogWarning("CreateRole failed: Role name must be provided.");
            return BadRequest("Role name must be provided.");
            }
            
            try
            {
            var createdRole = await _roleService.CreateRoleAsync(request.Role);
            _logger.LogInformation("Role '{Role}' created successfully with id {Id}.", createdRole.RoleName, createdRole.Id);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
            }
            catch (InvalidOperationException ex)
            {
            _logger.LogWarning(ex, "Invalid operation in CreateRole: {Message}", ex.Message);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger.LogWarning(ex, "Unauthorized access in CreateRole: {Message}", ex.Message);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Unexpected error in CreateRole: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="id">The role's ID.</param>
        /// <param name="request">The request object containing the new role name.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDTO>> UpdateRole(int id, [FromBody] RoleRequest request)
        {
            _logger.LogInformation("UpdateRole called with id: {Id}, role: {Role}", id, request?.Role);

            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
            _logger.LogWarning("UpdateRole failed: Role name must be provided.");
            return BadRequest("Role name must be provided.");
            }
            
            try
            {
            var updatedRole = await _roleService.UpdateRoleAsync(id, request.Role);
            _logger.LogInformation("Role with id {Id} updated successfully to '{Role}'.", id, updatedRole.RoleName);
            return Ok(updatedRole);
            }
            catch (InvalidOperationException ex)
            {
            _logger.LogWarning(ex, "Invalid operation in UpdateRole: {Message}", ex.Message);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger.LogWarning(ex, "Unauthorized access in UpdateRole: {Message}", ex.Message);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Unexpected error in UpdateRole: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Deletes a role by its ID.
        /// </summary>
        /// <param name="id">The role's ID.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            _logger.LogInformation("DeleteRole called with id: {Id}", id);
            try
            {
            var success = await _roleService.DeleteRoleAsync(id);
            if (!success)
            {
                _logger.LogWarning("DeleteRole failed: Role with id {Id} not found.", id);
                return NotFound($"Role with id {id} not found.");
            }
            _logger.LogInformation("Role with id {Id} deleted successfully.", id);
            return NoContent();
            }
            catch (InvalidOperationException ex)
            {
            _logger.LogWarning(ex, "Invalid operation in DeleteRole: {Message}", ex.Message);
            return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
            _logger.LogWarning(ex, "Unauthorized access in DeleteRole: {Message}", ex.Message);
            return Forbid();
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Unexpected error in DeleteRole: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
            }
        }
    }
}
