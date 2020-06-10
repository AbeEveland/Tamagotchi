using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Tamagotchi.Models;

namespace tamagotchi.Controllers
{

    [ApiController]

    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public PetController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Pet>> GetAll()
        {
            return Ok(_context.Pets);
        }
        [HttpGet("{id}")]
        public ActionResult<Pet> GetByID(int id)
        {
            var pet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }
        [HttpPost]
        public ActionResult<Pet> Create(Pet petToCreate)
        {
            petToCreate.Birthday = DateTime.Now;
            petToCreate.HungerLevel = 0;
            petToCreate.HappinessLevel = 0;

            Console.WriteLine("***********************");
            Console.WriteLine($"Name: {petToCreate.Name}");
            Console.WriteLine("***********************");
            _context.Pets.Add(petToCreate);
            _context.SaveChanges();
            return CreatedAtAction(null, null, petToCreate);
        }
        [HttpPut("{id}")]
        public ActionResult<Pet> AddHappinessAndHungerLevel(int id, Pet add)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundPet == null)
            {
                return NotFound();
            }
            foundPet.HungerLevel = add.HungerLevel + 3;
            foundPet.HappinessLevel = add.HappinessLevel + 5;

            _context.Entry(foundPet).State = EntityState.Modified;

            return Ok(foundPet);
            _context.SaveChanges();


        }
        [HttpPut("{id}")]
        public ActionResult<Pet> SubtractHappinessAndHunger(int id, Pet subtract)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundPet == null)
            {
                return NotFound();
            }
            foundPet.HungerLevel = subtract.HungerLevel - 5;
            foundPet.HappinessLevel = subtract.HappinessLevel - 3;

            return Ok(foundPet);
        }
        public ActionResult<Pet> SubtractHappiness(int id, Pet subtractHappiness)
        {
            var foundPet = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundPet == null)
            {
                return NotFound();
            }
            foundPet.HungerLevel = subtractHappiness.HungerLevel - 5;
            foundPet.HappinessLevel = subtractHappiness.HappinessLevel - 3;

            return Ok(foundPet);
        }
        [HttpDelete("{id}")]
        public ActionResult<Pet> Delete(int id)
        {
            var foundGame = _context.Pets.FirstOrDefault(pet => pet.Id == id);
            if (foundGame == null)
            {
                return NotFound();
            }
            _context.Pets.Remove(foundGame);
            _context.SaveChanges();
            return Ok(foundGame);
        }
    }
}
