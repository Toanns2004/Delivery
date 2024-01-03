using api.Context;
using Microsoft.AspNetCore.Mvc;

namespace api.DTOs;

public class UserDTO
{
    public int id { get; set; }
    public string fullname { get; set; }
    
    public string email { get; set; }
    
    public string token { get; set; }
}