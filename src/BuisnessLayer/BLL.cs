using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DomainEntities;

namespace BuisnessLayer
{
    public class BLL
    {
        private AreaRepository _areaRepository = new AreaRepository();

        private EventAreaRepository _eventAreaRepository = new EventAreaRepository();

        private EventRepository _eventRepository = new EventRepository();

        private EventSeatRepository _eventSeatRepository = new EventSeatRepository();

        private LayoutRepository _layoutRepository = new LayoutRepository();

        private SeatRepository _seatRepository = new SeatRepository();

        private VenueRepository _venueRepository = new VenueRepository();

        public void AddEvent(Event @event)
        {
            var layout = _layoutRepository.GetAll().Select(elem => elem).Where(elem => elem.Id == @event.LayoutId).Single();
            var venueId = _venueRepository.GetAll().Select(elem => elem).Where(elem => elem.Id == layout.VenueId).Select(elem => elem.Id).Single();
            foreach (var eventElem in _eventRepository.GetAll())
            {
                var layout_check = _layoutRepository.GetAll().Select(elem => elem).Where(elem => elem.Id == eventElem.LayoutId).Single();
                var venue = _venueRepository.GetAll().Select(elem => elem).Where(elem => elem.Id == layout.VenueId).Single();
                if (venueId == venue.Id)
                {
                    throw new Exception("There is already an event");
                }
            }

            _eventRepository.Create(@event);
        }

        public void AddVenue(Venue venue)
        {
            foreach (var venueElem in _venueRepository.GetAll())
            {
                if (venueElem.Description == venue.Description)
                {
                    throw new Exception("There is already a venue with such name");
                }
            }

            _venueRepository.Create(venue);
        }

        public void AddLayout(Layout layout)
        {
            foreach (var layoutElem in _layoutRepository.GetAll())
            {
                if (layoutElem.Description == layout.Description)
                {
                    throw new Exception("There is already a layout with such venue");
                }
            }

            _layoutRepository.Create(layout);
        }

        public void AddArea(Area area)
        {
            foreach (var areaElem in _areaRepository.GetAll())
            {
                if (areaElem.Description == area.Description && areaElem.LayoutId == area.LayoutId)
                {
                    throw new Exception("There is already a area with such layout and description");
                }
            }

            _areaRepository.Create(area);
        }

        public void AddSeat(Seat seat)
        {
            foreach (var seatElem in _seatRepository.GetAll())
            {
                if (seatElem.AreaId == seat.AreaId && seatElem.Row == seat.Row && seatElem.Number == seat.Number)
                {
                    throw new Exception("There is already a seat with coords");
                }
            }

            _seatRepository.Create(seat);
        }
    }
}
