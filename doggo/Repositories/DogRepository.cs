using doggo.Models;
using Doggo.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doggo.Repositories
{
    public class DogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT dog.Id as dogId, dog.Name as dogName, dog.OwnerId, dog.Breed as dogBreed, dog.Notes, dog.ImageUrl as dogImageUrl,
                        owner.Id as ownerId, owner.Email as ownerEmail, owner.Name  as ownerName, 
                        owner.Address as ownerAddress, owner.NeighborhoodId as ownerNeighborhood, owner.Phone as ownerPhone
                        FROM Dog dog
                        JOIN Owner owner ON owner.Id = ownerId";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("dogId")),
                            Name = reader.GetString(reader.GetOrdinal("dogName")),                            
                            OwnerId = reader.GetInt32(reader.GetOrdinal("ownerId")),    
                            Breed = reader.GetString(reader.GetOrdinal("dogBreed")),
                            Notes = ReaderHelpers.GetNullableString(reader, "Notes"),
                            ImageUrl = ReaderHelpers.GetNullableString(reader, "dogImageUrl")
                        };
                        
                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }
        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT dog.Id as dogId, dog.Name as dogName, dog.OwnerId, dog.Breed as dogBreed, dog.Notes, dog.ImageUrl as dogImageUrl,
                        owner.Id as ownerId, owner.Email as ownerEmail, owner.Name  as ownerName, 
                        owner.Address as ownerAddress, owner.NeighborhoodId as ownerNeighborhood, owner.Phone as ownerPhone
                        neighborhood.Id as neighborhoodId, neighborhood.Name as neighborhoodName
                        FROM Dog
                        JOIN Owner owner ON owner.Id = dogId
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("dogId")),
                            Name = reader.GetString(reader.GetOrdinal("dogName")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("ownerId")),
                            Breed = reader.GetString(reader.GetOrdinal("dogBreed")),
                            Notes = reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("dogImageUrl"))
                        };
                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
    }
}
