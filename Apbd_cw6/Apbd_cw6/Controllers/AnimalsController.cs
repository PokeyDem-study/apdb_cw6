using Apbd_cw6.Models;
using Apbd_cw6.Models.DTOs;
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
    public IActionResult GetAnimals(string argument = "Name")
    {
        //open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        //Define command
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        if (Validator.CheckArgument(argument))
            command.CommandText = "SELECT * FROM Animal ORDER BY " + argument + " ASC"; //secured concatenation
        else
            return BadRequest("Invalid argument");
        
        //Execute command
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal"); //numer columny dla IdAnimal
        int nameOrdinal = reader.GetOrdinal("Name");
        int descriptionOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = reader.GetString(descriptionOrdinal),
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal)
            });
        }
        return Ok(animals);
    }

    [HttpPost]

    public IActionResult AddAnimal(AddAnimal animal)
    {
       
        //open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        //Define command
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName, '', '', '')";
        command.Parameters.AddWithValue("@animalName", animal.Name);

        command.ExecuteNonQuery();
        return Created("", null);
    }
}