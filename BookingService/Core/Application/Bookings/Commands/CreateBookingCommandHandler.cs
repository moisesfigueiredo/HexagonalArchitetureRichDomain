using Application.Bookings.Dtos;
using Application.Bookings.Ports;
using Application.Payments;
using Application.Rooms.Responses;
using Domain.Bookings.Exceptions;
using Domain.Bookings.Ports;
using Domain.Guests.Ports;
using Domain.Rooms.Ports;
using MediatR;

namespace Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IGuestRepository guestRepository)
        {
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookingDto = request.BookingDto;
                var booking = BookingDto.MapToEntity(bookingDto);
                booking.Guest = await _guestRepository.Get(bookingDto.GuestId);
                booking.Room = await _roomRepository.GetAggregate(bookingDto.RoomId);

                await booking.Save(_bookingRepository);

                bookingDto.Id = booking.Id;

                return new BookingResponse
                {
                    Success = true,
                    Data = bookingDto,
                };
            }
            catch (PlacedAtIsARequiredInformationException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "PlacedAt is a required information"
                };
            }
            catch (StartDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Start is a required information"
                };
            }
            catch (RoomIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Room is a required information"
                };
            }
            catch (GuestIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Guest is a required information"
                };
            }
            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED,
                    Message = "The selected Room is not available"
                };
            }
            catch (Exception ex)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
