using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using VejrstationAPI;
using VejrstationAPI.Controllers;
using VejrstationAPI.Data;
using VejrstationAPI.Models;
using Xunit;
using Microsoft.Extensions.Configuration;


namespace VejrstationAPITest
{
    public class VejrobservationerControllerTest
    {

        [Fact]
        public async Task GetSidsteVejrobservationer_ReturnsCorrectList()
        {
            DbContextOptionsBuilder<VejrstationAPIContext> optionsBuilder = new DbContextOptionsBuilder<VejrstationAPIContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VejrStationData;Trusted_Connection=True;MultipleActiveResultSets=true");
            
            // Arrange
            List<Vejrobservation> model;
            using (var context = new VejrstationAPIContext(optionsBuilder.Options))
            {
                var controller = new VejrobservationerController(context);

                // Act
                model = controller.GetSidsteVejrobservationer().Result?.Value;
            }

            // Assert
            Assert.Equal(3, model.Count);

        }
    }
}
