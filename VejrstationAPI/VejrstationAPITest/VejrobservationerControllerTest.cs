using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using VejrstationAPI;
using VejrstationAPI.Controllers;
using VejrstationAPI.Data;
using VejrstationAPI.Models;
using Xunit;
using Microsoft.Extensions.Configuration;
using VejrstationAPI.Hubs;


namespace VejrstationAPITest
{
    public class VejrobservationerControllerTest
    {
        private DbContextOptions<VejrstationAPIContext> _options;
        private VejrstationAPIContext _context;
        private VejrobservationerController _controller;
        private IHubContext<VejrHub> _vejrHub;

        public VejrobservationerControllerTest()
        {
            _options = new DbContextOptionsBuilder<VejrstationAPIContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new VejrstationAPIContext(_options);
            _controller = new VejrobservationerController(_vejrHub, _context);
        }

        [Fact]
        public async Task GetSidsteVejrobservationer_ReturnsCorrectList()
        {
            // Arrange
            _context.Vejrobservationer.Add(new Vejrobservation
            {
                Tidspunkt = DateTime.Now,
                Sted = new Sted()
                {
                    Navn = "hej1",
                    Latitude = 22.22,
                    Longitude = 44.44
                },
                Temperatur = 10,
                Luftfugtighed = 33,
                Lufttryk = 1134
            });

            _context.Vejrobservationer.Add(new Vejrobservation
            {
                Tidspunkt = DateTime.Now,
                Sted = new Sted()
                {
                    Navn = "hej2",
                    Latitude = 22.22,
                    Longitude = 44.44
                },
                Temperatur = 10,
                Luftfugtighed = 33,
                Lufttryk = 1134
            });

            _context.Vejrobservationer.Add(new Vejrobservation
            {
                Tidspunkt = DateTime.Now,
                Sted = new Sted()
                {
                    Navn = "hej3",
                    Latitude = 22.22,
                    Longitude = 44.44
                },
                Temperatur = 10,
                Luftfugtighed = 33,
                Lufttryk = 1134
            });

            _context.SaveChanges();

            // Act
            var model = _controller.GetSidsteVejrobservationer()?.Result.Value;
            
            // Assert
            Assert.Equal(3, model.Count);
        }

        [Fact]
        public async Task GetSidsteVejrobservationer_ReturnsCorrectListBySingleDate()
        {
            // Arrange
            _context.Vejrobservationer.Add(new Vejrobservation
            {
                Tidspunkt = DateTime.Now,
                Sted = new Sted()
                {
                    Navn = "hej1",
                    Latitude = 22.22,
                    Longitude = 44.44
                },
                Temperatur = 10,
                Luftfugtighed = 33,
                Lufttryk = 1134
            });

            _context.Vejrobservationer.Add(new Vejrobservation
            {
                Tidspunkt = DateTime.Now,
                Sted = new Sted()
                {
                    Navn = "hej2",
                    Latitude = 22.22,
                    Longitude = 44.44
                },
                Temperatur = 10,
                Luftfugtighed = 33,
                Lufttryk = 1134
            });

            _context.Vejrobservationer.Add(new Vejrobservation
            {
                Tidspunkt = DateTime.Now,
                Sted = new Sted()
                {
                    Navn = "hej3",
                    Latitude = 22.22,
                    Longitude = 44.44
                },
                Temperatur = 10,
                Luftfugtighed = 33,
                Lufttryk = 1134
            });

            _context.SaveChanges();

            // Act
            var model = _controller.GetVejrobservation(DateTime.Now)?.Result.Value;

            // Assert
            Assert.Equal(3, model.Count);
        }
    }
}
