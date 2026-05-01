using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteProject.DTO;
using NoteProject.Interfaces;
using NoteProject.Models;
using System;
using System.Linq;

namespace NoteProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly NoteDbContext _db;
    private readonly IJwtService _jwt;

    public AuthController(NoteDbContext db, IJwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email đã tồn tại");

        var user = new User
        {
            Username = dto.UserName,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwt.GenerateToken(user.Id, user.Username, user.Email);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if ((!dto.Email.EndsWith("@gmail.com")) || dto.Email.IsNullOrEmpty())
            return BadRequest("Email không đúng định dạng");
        if (dto.Password.IsNullOrEmpty())
            return BadRequest("Mật khẩu chưa đúng định dạng");
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return Unauthorized("Email hoặc mật khẩu không đúng");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Unauthorized("Email hoặc mật khẩu không đúng");

        var token = _jwt.GenerateToken(user.Id, user.Username, user.Email);
        return Ok(new { token });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto dto)
    {
        // 👉 lấy userId từ token
        var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
        if (userId == null) return Unauthorized();
        // 👉 tìm user trong DB
        var user = await _db.Users.FindAsync(Guid.Parse(userId));
        if (user == null) return NotFound();

        user.Username = dto.UserName ?? user.Username;

        user.Email = dto.Email ?? user.Email;
        if (!string.IsNullOrEmpty(dto.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        user.UpdatedAt = DateTime.UtcNow;

        //Update avatar
        if (dto.AvatarUrl != null && dto.AvatarUrl.Length > 0)
        {
            // 👉 validate file (optional)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(dto.AvatarUrl.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                return BadRequest("Chỉ cho phép jpg/png");

            // 👉 tạo folder nếu chưa có
            var folder = Path.Combine("wwwroot", "avatars");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // 👉 xóa avatar cũ (nếu có)
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldPath = Path.Combine("wwwroot", user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            // 👉 tạo tên file mới
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(folder, fileName);

            // 👉 lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.AvatarUrl.CopyToAsync(stream);
            }

            // 👉 lưu URL vào DB
            user.AvatarUrl = $"/avatars/{fileName}";
        }
        await _db.SaveChangesAsync();
        return Ok("Cập nhật thành công");
    }
}
