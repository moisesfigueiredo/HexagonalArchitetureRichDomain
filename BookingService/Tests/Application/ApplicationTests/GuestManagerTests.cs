using Application.Guests;
using Application.Guests.DTO;
using Application.Guests.Requests;
using Application.Rooms.Responses;
using Domain.Guests.Entities;
using Domain.Guests.Enums;
using Domain.Guests.Ports;
using Domain.Guests.ValueObjects;
using Moq;

namespace ApplicationTests
{
    public class GuestManagerTests
    {
        GuestManager guestManager;

        [Fact]
        [Trait("Application", "Guest Manager")]
        public async Task HappyPath()
        {
            var guestDto = new GuestDto
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "abc@gmail.com",
                IdNumber = "abca",
                IdTypeCode = 1
            };

            int expectedId = 222;

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.Create(
                It.IsAny<Guest>()))
                .ReturnsAsync(expectedId);

            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);
            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.Equal(res.Data.Id, expectedId);
            Assert.Equal(res.Data.Name, guestDto.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [Trait("Application", "Guest Manager")]
        public async Task Should_Return_InvalidPersonDocumentIdException_WhenDocsAreInvalid(string docNumber)
        {
            var guestDto = new GuestDto
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "abc@gmail.com",
                IdNumber = docNumber,
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.Create(
                It.IsAny<Guest>()))
                .ReturnsAsync(222);

            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrorCodes.INVALID_PERSON_ID, res.ErrorCode);
            Assert.Equal("The ID passed is not valid", res.Message);
        }

        [Theory]
        [InlineData("", "surnametest", "asdf@gmail.com")]
        [InlineData(null, "surnametest", "asdf@gmail.com")]
        [InlineData("Fulano", "", "asdf@gmail.com")]
        [InlineData("Fulano", null, "asdf@gmail.com")]
        [InlineData("Fulano", "surnametest", "")]
        [InlineData("Fulano", "surnametest", null)]
        [Trait("Application", "Guest Manager")]
        public async Task Should_Return_MissingRequiredInformation_WhenDocsAreInvalid(string name, string surname, string email)
        {
            var guestDto = new GuestDto
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "abcd",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            var fakeRepo = new Mock<IGuestRepository>();

            fakeRepo.Setup(x => x.Create(
                It.IsAny<Guest>()))
                .ReturnsAsync(222);

            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.CreateGuest(request);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrorCodes.MISSING_REQUIRED_INFORMATION, res.ErrorCode);
            Assert.Equal("Missing required information passed", res.Message);
        }

        [Fact]
        [Trait("Application", "Guest Manager")]
        public async Task Should_Return_GuestNotFound_When_GuestDoesntExist()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            var guestId = 333;

            Guest nullGuest = null;

            fakeRepo.Setup(x => x.Get(guestId))
                .ReturnsAsync(nullGuest);

            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.GetGuest(333);

            Assert.NotNull(res);
            Assert.False(res.Success);
            Assert.Equal(ErrorCodes.GUEST_NOT_FOUND, res.ErrorCode);
            Assert.Equal("No Guest record was found with the given Id", res.Message);
        }

        [Fact]
        [Trait("Application", "Guest Manager")]
        public async Task Should_Return_Guest_Success()
        {
            var fakeRepo = new Mock<IGuestRepository>();

            var guestId = 333;

            var fakeGuest = new Guest
            {
                Id = 333,
                Name = "Test",
                DocumentId = new PersonId
                {
                    DocumentType = DocumentType.DriveLicence,
                    IdNumber = "123"
                }
            };

            Guest nullGuest = null;

            fakeRepo.Setup(x => x.Get(guestId))
                .ReturnsAsync(fakeGuest);

            guestManager = new GuestManager(fakeRepo.Object);

            var res = await guestManager.GetGuest(333);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.Equal(res.Data.Id, fakeGuest.Id);
            Assert.Equal(res.Data.Name, fakeGuest.Name);
        }
    }
}