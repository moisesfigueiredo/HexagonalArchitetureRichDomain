using Application.Bookings.Commands;
using Application.Bookings.Dtos;
using Application.Rooms.Responses;
using Domain.Bookings.Entities;
using Domain.Bookings.Ports;
using Domain.Guests.Entities;
using Domain.Guests.Enums;
using Domain.Guests.Ports;
using Domain.Guests.ValueObjects;
using Domain.Rooms.Entities;
using Domain.Rooms.Ports;
using Domain.Rooms.ValueObjects;
using Moq;

namespace ApplicationTests
{
    public class CreateBookingCommandHandlerTests
    {
        private CreateBookingCommandHandler GetCommmandMock(
               Mock<IRoomRepository> roomRepository = null,
               Mock<IGuestRepository> guestRepository = null,
               Mock<IBookingRepository> bookingRepository = null)
        {
            var _bookingRepository = bookingRepository ?? new Mock<IBookingRepository>();
            var _guestRepository = guestRepository ?? new Mock<IGuestRepository>();
            var _roomkingRepository = roomRepository ?? new Mock<IRoomRepository>();

            var commandHanler = new CreateBookingCommandHandler(_bookingRepository.Object, _roomkingRepository.Object, _guestRepository.Object);

            return commandHanler;
        }


        [Fact]
        [Trait("Application", "Create Booking Command Handler")]
        public async Task Should_Not_Create_Booking_If_RoomIsMissing()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new BookingDto
                {
                    GuestId = 1,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(2)
                }
            };

            var fakeGuest = new Guest
            {
                Id = command.BookingDto.GuestId,
                DocumentId = new PersonId
                {
                    DocumentType = DocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "a@a.com",
                Name = "Fake Guest",
                Surname = "Fake Guest Surname"
            };

            var guestRepository = new Mock<IGuestRepository>();

            guestRepository.Setup(x => x.Get(command.BookingDto.GuestId))
                .ReturnsAsync(fakeGuest);

            var fakeBooking = new Booking
            {
                Id = 1
            };

            var bookingRepositoryMock = new Mock<IBookingRepository>();

            bookingRepositoryMock.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                .ReturnsAsync(fakeBooking);

            var handler = GetCommmandMock(null, guestRepository, bookingRepositoryMock);

            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.False(resp.Success);
            Assert.Equal(ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION, resp.ErrorCode);
            Assert.Equal("Room is a required information", resp.Message);
        }


        [Fact]
        [Trait("Application", "Create Booking Command Handler")]
        public async Task Should_Not_Create_Booking_If_StartDateIsMissing()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new BookingDto
                {
                    RoomId = 1,
                    GuestId = 1,
                    End = DateTime.Now.AddDays(2)
                }
            };

            var fakeGuest = new Guest
            {
                Id = command.BookingDto.GuestId,
                DocumentId = new PersonId
                {
                    DocumentType = DocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "a@a.com",
                Name = "Fake Guest",
                Surname = "Fake Guest Surname"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.GuestId))
                .ReturnsAsync(fakeGuest);

            var fakeRoom = new Room
            {
                Id = command.BookingDto.RoomId,
                InMaintenance = false,
                Price = new Price
                {
                    Currency = AcceptedCurrencies.Dollar,
                    Value = 100,
                },
                Name = "Fake Room 01",
                Level = 1
            };

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.BookingDto.RoomId))
                .ReturnsAsync(fakeRoom);

            var handler = GetCommmandMock(roomRepository, guestRepository);

            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.False(resp.Success);
            Assert.Equal(ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION, resp.ErrorCode);
            Assert.Equal("Start is a required information", resp.Message);
        }

        [Fact]
        [Trait("Application", "Create Booking Command Handler")]
        public async Task Should_CreateBooking()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new BookingDto
                {
                    RoomId = 1,
                    GuestId = 1,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(2)
                }
            };

            var fakeGuest = new Guest
            {
                Id = command.BookingDto.GuestId,
                DocumentId = new PersonId
                {
                    DocumentType = DocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "a@a.com",
                Name = "Fake Guest",
                Surname = "Fake Guest Surname"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.GuestId))
                .ReturnsAsync(fakeGuest);

            var fakeRoom = new Room
            {
                Id = command.BookingDto.RoomId,
                InMaintenance = false,
                Price = new Price
                {
                    Currency = AcceptedCurrencies.Dollar,
                    Value = 100,
                },
                Name = "Fake Room 01",
                Level = 1
            };

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.BookingDto.RoomId))
                .ReturnsAsync(fakeRoom);

            var fakeBooking = new Booking
            {
                Id = 1
            };

            var bookingRepositoryMock = new Mock<IBookingRepository>();
            bookingRepositoryMock.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                .ReturnsAsync(fakeBooking);

            var handler = GetCommmandMock(roomRepository, guestRepository, bookingRepositoryMock);

            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.True(resp.Success);
            Assert.NotNull(resp.Data);
            Assert.Equal(resp.Data.Id, fakeRoom.Id);
        }
    }
}
