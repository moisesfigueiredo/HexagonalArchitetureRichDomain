using Domain.Bookings.Exceptions;
using Domain.Bookings.Ports;
using Domain.Guests.Entities;
using Domain.Guests.Enums;
using Domain.Rooms.Entities;
using Action = Domain.Guests.Enums.Action;

namespace Domain.Bookings.Entities
{
    public class Booking
    {
        public Booking()
        {
            Status = BookingStatus.Created;
            PlacedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }
        public BookingStatus Status { get; set; }

        public void ChangeState(Action action)
        {
            Status = (Status, action) switch
            {
                (BookingStatus.Created, Action.Pay) => BookingStatus.Paid,
                (BookingStatus.Created, Action.Cancel) => BookingStatus.Canceled,
                (BookingStatus.Paid, Action.Finish) => BookingStatus.Finished,
                (BookingStatus.Paid, Action.Refound) => BookingStatus.Refounded,
                (BookingStatus.Canceled, Action.Reopen) => BookingStatus.Created,
                _ => Status
            };
        }

        public bool IsValid()
        {
            try
            {
                ValidateState();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ValidateState()
        {
            if (PlacedAt == default)
            {
                throw new PlacedAtIsARequiredInformationException();
            }

            if (Start == default)
            {
                throw new StartDateTimeIsRequiredException();
            }

            if (End == default)
            {
                throw new EndDateTimeIsRequiredException();
            }

            if (Room == null)
            {
                throw new RoomIsRequiredException();
            }

            if (Guest == null)
            {
                throw new GuestIsRequiredException();
            }
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            ValidateState();

            Guest.IsValid();

            if (!Room.CanBeBooked())
            {
                throw new RoomCannotBeBookedException();
            }

            if (Id == 0)
            {
                var resp = await bookingRepository.CreateBooking(this);
                Id = resp.Id;
            }
            else
            {
                //await bookingRepository.Update(this);
            }
        }
    }
}
