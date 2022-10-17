using Application.Payments.Dtos;
using Domain.Guests.Entities;
using Domain.Guests.Enums;
using Action = Domain.Guests.Enums.Action;

namespace DomainTests.Bookings
{
    public class StateMachineTests
    {
        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldAlwaysStartWithCreatedStatus()
        {
            var booking = new Booking();

            Assert.Equal(BookingStatus.Created, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldSetStatusToPaidWhenPayingForABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);

            Assert.Equal(BookingStatus.Paid, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldSetStatusToCanceldWhenCancelingABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);

            Assert.Equal(BookingStatus.Canceled, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldSetStatusToFinishedWhenFinishingAPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);

            Assert.Equal(BookingStatus.Finished, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldSetStatusToRefoundedWhenRefoundingAPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Refound);

            Assert.Equal(BookingStatus.Refounded, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldSetStatusToCreatedWhenReopeningACanceledBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);
            booking.ChangeState(Action.Reopen);

            Assert.Equal(BookingStatus.Created, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldNotChangeStatusWhenRefoundingABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Refound);

            Assert.Equal(BookingStatus.Created, booking.Status);
        }

        [Fact]
        [Trait("Domain", "State Machine")]
        public void ShouldNotChangeStatusWhenRefoundingAFinishedBookin()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);
            booking.ChangeState(Action.Refound);

            Assert.Equal(BookingStatus.Finished, booking.Status);
        }
    }
}