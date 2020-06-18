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
                        SELECT 
                            d.Id, 
                            d.Name, 
                            d.OwnerId,
                            d.Breed, 
                            d.Notes, 
                            d.ImageUrl                            
                        FROM Dog d
                        JOIN Owner o ON o.Id = d.OwnerId
                        WHERE d.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = ReaderHelpers.GetNullableString(reader, "Notes"),
                            ImageUrl = ReaderHelpers.GetNullableString(reader, "ImageUrl"),

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
        public void Add(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Dog (Name, OwnerId, Breed, Notes, ImageUrl)
                        OUTPUT INSERTED.ID
                        VALUES(@name, @ownerId, @breed, @notes, @imageUrl)";
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = ReaderHelpers.GetNullableString(reader, "Notes"),
                            ImageUrl = ReaderHelpers.GetNullableString(reader, "ImageUrl")
                        };

                        dogs.Add(dog);
                    }

                    reader.Close();
                }
            }
        }
        public void Remove(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Dog WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();   
                }
            }
        }
    }
}
