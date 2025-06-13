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
        
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            try{
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
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
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                return Ok(role);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
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
            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest("Role name must be provided.");
            }
            
            try
            {
                var createdRole = await _roleService.CreateRoleAsync(request.Role);
                return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
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
            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest("Role name must be provided.");
            }
            
            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(id, request.Role);
                return Ok(updatedRole);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
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
            try
            {
                var success = await _roleService.DeleteRoleAsync(id);
                if (!success)
                {
                    return NotFound($"Role with id {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    // Request DTO for both role creation and updating
    public class RoleRequest
    {
        public string Role { get; set; }
    }
}
