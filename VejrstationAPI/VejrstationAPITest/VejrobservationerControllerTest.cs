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
        public async Task GetSidsteVejrobservationer_ReturnsLastThreeObvservations()
        {
            // Arrange
            var observation1 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05,21,22,15,0),
                Sted = new Sted()
                {
                    Navn = "Aarhus N",
                    Latitude = 56.12,
                    Longitude = 10.14
                },
                Temperatur = 10,
                Luftfugtighed = 20,
                Lufttryk = 1136
            };

            var observation2 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 16, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus C",
                    Latitude = 56.14,
                    Longitude = 10.17
                },
                Temperatur = 11,
                Luftfugtighed = 22,
                Lufttryk = 1120
            };

            var observation3 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 17, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus V",
                    Latitude = 56.17,
                    Longitude = 10.13
                },
                Temperatur = 12,
                Luftfugtighed = 19,
                Lufttryk = 1130
            };

            _context.Vejrobservationer.Add(observation1);
            _context.Vejrobservationer.Add(observation2);
            _context.Vejrobservationer.Add(observation3);

            _context.SaveChanges();

            var obs1 = _controller.GetVejrobservationById(observation1.VejrobservationId)?.Result.Value;
            var obs2 = _controller.GetVejrobservationById(observation2.VejrobservationId)?.Result.Value;
            var obs3 = _controller.GetVejrobservationById(observation3.VejrobservationId)?.Result.Value;

            // Act
            var model = _controller.GetSidsteVejrobservationer()?.Result.Value;

            // Assert
            Assert.Equal(obs1.VejrobservationId, model[2].VejrobservationId);
            Assert.Equal(obs2.VejrobservationId, model[1].VejrobservationId);
            Assert.Equal(obs3.VejrobservationId, model[0].VejrobservationId);
        }

        [Fact]
        public async Task GetSidsteVejrobservationer_ReturnsAllObservationsForAGivenDate()
        {
            // Arrange
            var observation1 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 15, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus N",
                    Latitude = 56.12,
                    Longitude = 10.14
                },
                Temperatur = 10,
                Luftfugtighed = 20,
                Lufttryk = 1136
            };

            var observation2 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 16, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus C",
                    Latitude = 56.14,
                    Longitude = 10.17
                },
                Temperatur = 11,
                Luftfugtighed = 22,
                Lufttryk = 1120
            };

            var observation3 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 22, 22, 17, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus V",
                    Latitude = 56.17,
                    Longitude = 10.13
                },
                Temperatur = 12,
                Luftfugtighed = 19,
                Lufttryk = 1130
            };
            var observation4 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 22, 22, 18, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus",
                    Latitude = 56.17,
                    Longitude = 10.13
                },
                Temperatur = 12,
                Luftfugtighed = 19,
                Lufttryk = 1130
            };

            _context.Vejrobservationer.Add(observation1);
            _context.Vejrobservationer.Add(observation2);
            _context.Vejrobservationer.Add(observation3);
            _context.Vejrobservationer.Add(observation4);

            _context.SaveChanges();

            var obs1 = _controller.GetVejrobservationById(observation3.VejrobservationId)?.Result.Value;
            var obs2 = _controller.GetVejrobservationById(observation4.VejrobservationId)?.Result.Value;

            // Act
            var model = _controller.GetVejrobservation(new DateTime(2020, 5, 22))?.Result.Value;

            // Assert
            Assert.Equal(obs1.VejrobservationId, model[0].VejrobservationId);
            Assert.Equal(obs2.VejrobservationId, model[1].VejrobservationId);
        }

        [Fact]
        public async Task GetSidsteVejrobservationer_ReturnsAllObservationsForAGivenDateStartAndEnd()
        {
            // Arrange
            var observation1 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 15, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus N",
                    Latitude = 56.12,
                    Longitude = 10.14
                },
                Temperatur = 10,
                Luftfugtighed = 20,
                Lufttryk = 1136
            };

            var observation2 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 16, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus C",
                    Latitude = 56.14,
                    Longitude = 10.17
                },
                Temperatur = 11,
                Luftfugtighed = 22,
                Lufttryk = 1120
            };

            var observation3 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 17, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus V",
                    Latitude = 56.17,
                    Longitude = 10.13
                },
                Temperatur = 12,
                Luftfugtighed = 19,
                Lufttryk = 1130
            };
            var observation4 = new Vejrobservation
            {
                Tidspunkt = new DateTime(2020, 05, 21, 22, 18, 0),
                Sted = new Sted()
                {
                    Navn = "Aarhus",
                    Latitude = 56.17,
                    Longitude = 10.13
                },
                Temperatur = 12,
                Luftfugtighed = 19,
                Lufttryk = 1130
            };

            _context.Vejrobservationer.Add(observation1);
            _context.Vejrobservationer.Add(observation2);
            _context.Vejrobservationer.Add(observation3);
            _context.Vejrobservationer.Add(observation4);

            _context.SaveChanges();

            var obs1 = _controller.GetVejrobservationById(observation1.VejrobservationId)?.Result.Value;
            var obs2 = _controller.GetVejrobservationById(observation2.VejrobservationId)?.Result.Value;
            var obs3 = _controller.GetVejrobservationById(observation3.VejrobservationId)?.Result.Value;
            var obs4 = _controller.GetVejrobservationById(observation4.VejrobservationId)?.Result.Value;

            // Act
            var model = _controller.GetVejrobservationer(new DateTime(2020, 5, 20), new DateTime(2020, 5, 22))?.Result.Value;

            // Assert
            Assert.Equal(obs1.VejrobservationId, model[3].VejrobservationId);
            Assert.Equal(obs2.VejrobservationId, model[2].VejrobservationId);
            Assert.Equal(obs3.VejrobservationId, model[1].VejrobservationId);
            Assert.Equal(obs4.VejrobservationId, model[0].VejrobservationId);
        }
    }
}
