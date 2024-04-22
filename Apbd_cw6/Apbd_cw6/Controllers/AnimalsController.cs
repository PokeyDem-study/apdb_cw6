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
            command.CommandText = "SELECT * FROM Animals ORDER BY " + argument + " ASC"; //secured concatenation
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
        command.CommandText = "INSERT INTO Animals VALUES(@animalName, @animalDescription, @animalCategory, @animalArea)";
        
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);

        command.ExecuteNonQuery();
        return Created("", null);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateAnimal(int id, AddAnimal animal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        
        connection.Open();
        using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT IdAnimal FROM Animals WHERE IdAnimal = @idAnimal";
        command.Parameters.AddWithValue("@IdAnimal", id);

        var reader = command.ExecuteReader();

        if (!reader.HasRows)
            return NotFound("Cant find animal with id: " + id);
        
        reader.Close();

        command.CommandText = "UPDATE Animals " +
                              "SET Name = @animalName, Description = @animalDescription, Category = @animalCategory, Area = @animalArea " +
                              "WHERE IdAnimal = @animalId";

        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);
        command.Parameters.AddWithValue("@animalId", id);

        command.ExecuteNonQuery();
            
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteAnimal(int id)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        
        connection.Open();
        using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT IdAnimal FROM Animals WHERE IdAnimal = @idAnimal";
        command.Parameters.AddWithValue("@idAnimal", id);

        var reader = command.ExecuteReader();

        if (!reader.HasRows)
            return NotFound("Cant find animal with id: " + id);
        
        reader.Close();

        command.CommandText = "DELETE FROM Animals WHERE IdAnimal = @idAnimalToDelete";
        command.Parameters.AddWithValue("@idAnimalToDelete", id);

        command.ExecuteNonQuery();
            
        return Ok();
    }
}