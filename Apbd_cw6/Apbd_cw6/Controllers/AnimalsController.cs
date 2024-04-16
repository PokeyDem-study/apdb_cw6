using Apbd_cw6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Apbd_cw6.Controllers;
[ApiController]
[Route("api/[controller]")]

public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        //open connection
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        //Define command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal";
        
        //Execute command
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal"); //numer columny dla IdAnimal
        int nameOrdinal = reader.GetOrdinal("Name");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal)
            });
        }
        return Ok(animals);
    }
}