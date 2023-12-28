using Logix.Models;
using Logix.Service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly LogixService logixService;

    public UserController(LogixService logixService)
    {
        this.logixService = logixService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await logixService.Register(model);
                return Ok("Registration successful!");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        return BadRequest(ModelState);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        if (ModelState.IsValid)
        {
            await logixService.Login(model);
            return Ok("Success");
        }

        return BadRequest(ModelState);
    }

    [HttpGet("GetUserInfo")]
    public async Task<dynamic> GetUserInfo(int userId)
    {
        return await logixService.GetUserInfo(userId);
    }

    [HttpPost("CreateClass")]
    public async Task<IActionResult> CreateClass(ClassModel classModel)
    {
        if (ModelState.IsValid)
        {
            await logixService.CreateClass(classModel);
            return Ok();
        }
        return BadRequest(ModelState);
    }

    [HttpPost("AddClassToUser")]
    public async Task<IActionResult> AddClassToUser(AddClassToUser addClassToUser)
    {
        await logixService.AddClassToUser(addClassToUser);
        return Ok("Class Succesfully Added");
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(int userId, string password)
    {
        await logixService.DeleteUser(userId, password);
        return Ok($"User with id {userId} Deleted succesfully");
    }

    [HttpPut("EditUser")]
    public async Task<IActionResult> EditUser(int userId, string password,EditUserModel editUserModel)
    {
        if (ModelState.IsValid && password != null)
        {
            await logixService.EditUser(userId, password, editUserModel);
            return Ok("User Edited Successfully");
        }
        else
            return BadRequest("Fill All Required Data");
    }

    [HttpDelete("DeleteClassForUser")]
    public async Task<IActionResult> DeleteClassForUser(int userId, string password, int classId)
    {
        await logixService.DeleteClassForUser(userId, password, classId);
        return Ok("Class Deleted Successfully");
    }
}


