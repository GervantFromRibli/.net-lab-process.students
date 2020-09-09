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
        private AreaSqlRepository _areaRepository = new AreaSqlRepository();

        private EventAreaSqlRepository _eventAreaRepository = new EventAreaSqlRepository();

        private EventSqlRepository _eventRepository = new EventSqlRepository();

        private EventSeatSqlRepository _eventSeatRepository = new EventSeatSqlRepository();

        private LayoutSqlRepository _layoutRepository = new LayoutSqlRepository();

        private SeatSqlRepository _seatRepository = new SeatSqlRepository();

        private VenueSqlRepository _venueRepository = new VenueSqlRepository();

        public void AddEvent(Event @event)
        {
            var layout = _layoutRepository.FindById(@event.LayoutId);
            if (layout == null)
            {
                throw new Exception("No such a layout");
            }
            else
            {
                var venueId = _venueRepository.FindById(layout.VenueId).Id;
                foreach (var eventElem in _eventRepository.GetAll())
                {
                    var layout_check = _layoutRepository.FindById(eventElem.LayoutId);
                    var venue_check = _venueRepository.FindById(layout_check.VenueId);
                    if (venueId == venue_check.Id && ((@event.StartDate >= eventElem.StartDate && @event.StartDate < eventElem.StartDate) || (@event.EndDate > eventElem.StartDate && @event.EndDate <= eventElem.EndDate)))
                    {
                        throw new Exception("There is already an event");
                    }
                }

                bool isSeatExists = false;
                foreach (var areaElem in _areaRepository.GetAll())
                {
                    if (areaElem.LayoutId == layout.Id)
                    {
                        foreach (var seatElem in _seatRepository.GetAll())
                        {
                            if (seatElem.AreaId == areaElem.Id)
                            {
                                isSeatExists = true;
                                break;
                            }
                        }

                        if (isSeatExists == true)
                        {
                            break;
                        }
                    }
                }

                if (isSeatExists == true && @event.Name.Length <= 120 && @event.Description.Length <= 400 && @event.StartDate >= DateTime.Now && @event.EndDate >= @event.StartDate)
                {
                    _eventRepository.Create(@event);
                }
                else
                {
                    throw new Exception("Can`t add an event");
                }
            }
        }

        public void AddVenue(Venue venue)
        {
            foreach (var venueElem in _venueRepository.GetAll())
            {
                if (venueElem.Name == venue.Name)
                {
                    throw new Exception("There is already a venue with such name");
                }
            }

            if (venue.Name.Length <= 50 && venue.Description.Length <= 120 && venue.Address.Length <= 150 && venue.Phone.Length <= 15)
            {
                _venueRepository.Create(venue);
            }
            else
            {
                throw new Exception("Can`t add a venue");
            }
        }

        public void AddLayout(Layout layout)
        {
            foreach (var layoutElem in _layoutRepository.GetAll())
            {
                if (layoutElem.Name == layout.Name && layoutElem.VenueId == layout.VenueId)
                {
                    throw new Exception("There is already a layout with such venue");
                }
            }

            Venue venue = _venueRepository.FindById(layout.Id);
            if (layout.Name.Length <= 50 && venue != null && layout.Description.Length <= 120)
            {
                _layoutRepository.Create(layout);
            }
            else
            {
                throw new Exception("Can`t add a layout");
            }
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

            var layout = _layoutRepository.FindById(area.LayoutId);
            if (layout != null && area.Description.Length <= 200 && area.CoordX >= 0 && area.CoordY >= 0)
            {
                _areaRepository.Create(area);
            }
            else
            {
                throw new Exception("Can`t add an area");
            }
        }

        public void AddSeat(Seat seat)
        {
            foreach (var seatElem in _seatRepository.GetAll())
            {
                if (seatElem.AreaId == seat.AreaId && seatElem.Row == seat.Row && seatElem.Number == seat.Number)
                {
                    throw new Exception("There is already a seat with this coords");
                }
            }

            var area = _areaRepository.FindById(seat.AreaId);
            if (area != null && seat.Row >= 0 && seat.Number >= 0)
            {
                _seatRepository.Create(seat);
            }
            else
            {
                throw new Exception("Can`t add a seat");
            }
        }

        public void AddEventArea(EventArea eventArea)
        {
            foreach (var eventAreaElem in _eventAreaRepository.GetAll())
            {
                if (eventAreaElem.EventId == eventArea.EventId && eventAreaElem.CoordX == eventArea.CoordX && eventArea.CoordY == eventAreaElem.CoordY)
                {
                    throw new Exception("There is already an event area with such coords");
                }
            }

            var @event = _eventRepository.FindById(eventArea.EventId);
            if (@event != null && eventArea.Description.Length <= 200 && eventArea.CoordX >= 0 && eventArea.CoordY >= 0 && eventArea.Price >= 0)
            {
                _eventAreaRepository.Create(eventArea);
            }
            else
            {
                throw new Exception("Can`t add an event area");
            }
        }

        public void AddEventSeat(EventSeat eventSeat)
        {
            foreach (var eventSeatElem in _eventSeatRepository.GetAll())
            {
                if (eventSeatElem.EventAreaId == eventSeat.EventAreaId && eventSeatElem.Row == eventSeat.Row && eventSeatElem.Number == eventSeat.Number)
                {
                    throw new Exception("There is already an event seat with such coords");
                }
            }

            var eventArea = _eventAreaRepository.FindById(eventSeat.EventAreaId);
            if (eventArea != null && eventSeat.Row >= 0 && eventSeat.Number >= 0)
            {
                _eventSeatRepository.Create(eventSeat);
            }
            else
            {
                throw new Exception("Can`t add an event seat");
            }
        }

        public void UpdateEventSeat(EventSeat eventSeat)
        {
            foreach (var eventSeatElem in _eventSeatRepository.GetAll())
            {
                if (eventSeatElem.EventAreaId == eventSeat.EventAreaId && eventSeatElem.Row == eventSeat.Row && eventSeatElem.Number == eventSeat.Number)
                {
                    throw new Exception("There is already an event seat with such coords");
                }
            }

            var eventArea = _eventAreaRepository.FindById(eventSeat.EventAreaId);
            if (eventArea != null && eventSeat.Row >= 0 && eventSeat.Number >= 0)
            {
                _eventSeatRepository.Update(eventSeat);
            }
            else
            {
                throw new Exception("Can`t update an event seat");
            }
        }

        public void UpdateEventArea(EventArea eventArea)
        {
            foreach (var eventAreaElem in _eventAreaRepository.GetAll())
            {
                if (eventAreaElem.EventId == eventArea.EventId && eventAreaElem.CoordX == eventArea.CoordX && eventArea.CoordY == eventAreaElem.CoordY)
                {
                    throw new Exception("There is already an event area with such coords");
                }
            }

            var @event = _eventRepository.FindById(eventArea.EventId);
            if (@event != null && eventArea.Description.Length <= 200 && eventArea.CoordX >= 0 && eventArea.CoordY >= 0 && eventArea.Price >= 0)
            {
                _eventAreaRepository.Update(eventArea);
            }
            else
            {
                throw new Exception("Can`t update an event area");
            }
        }

        public void UpdateEvent(Event @event)
        {
            var layout = _layoutRepository.FindById(@event.LayoutId);
            if (layout == null)
            {
                throw new Exception("No such a layout");
            }
            else
            {
                var venueId = _venueRepository.FindById(layout.VenueId).Id;
                foreach (var eventElem in _eventRepository.GetAll())
                {
                    var layout_check = _layoutRepository.FindById(eventElem.LayoutId);
                    var venue_check = _venueRepository.FindById(layout_check.VenueId);
                    if (venueId == venue_check.Id && ((@event.StartDate >= eventElem.StartDate && @event.StartDate < eventElem.StartDate) || (@event.EndDate > eventElem.StartDate && @event.EndDate <= eventElem.EndDate)))
                    {
                        throw new Exception("There is already an event");
                    }
                }

                bool isSeatExists = false;
                foreach (var areaElem in _areaRepository.GetAll())
                {
                    if (areaElem.LayoutId == layout.Id)
                    {
                        foreach (var seatElem in _seatRepository.GetAll())
                        {
                            if (seatElem.AreaId == areaElem.Id)
                            {
                                isSeatExists = true;
                                break;
                            }
                        }

                        if (isSeatExists == true)
                        {
                            break;
                        }
                    }
                }

                if (isSeatExists == true && @event.Name.Length <= 120 && @event.Description.Length <= 400 && @event.StartDate >= DateTime.Now && @event.EndDate >= @event.StartDate)
                {
                    _eventRepository.Update(@event);
                }
                else
                {
                    throw new Exception("Can`t update an event");
                }
            }
        }

        public void UpdateVenue(Venue venue)
        {
            foreach (var venueElem in _venueRepository.GetAll())
            {
                if (venueElem.Name == venue.Name)
                {
                    throw new Exception("There is already a venue with such name");
                }
            }

            if (venue.Name.Length <= 50 && venue.Description.Length <= 120 && venue.Address.Length <= 150 && venue.Phone.Length <= 15)
            {
                _venueRepository.Update(venue);
            }
            else
            {
                throw new Exception("Can`t update a venue");
            }
        }

        public void UpdateLayout(Layout layout)
        {
            foreach (var layoutElem in _layoutRepository.GetAll())
            {
                if (layoutElem.Name == layout.Name && layoutElem.VenueId == layout.VenueId)
                {
                    throw new Exception("There is already a layout with such venue");
                }
            }

            Venue venue = _venueRepository.FindById(layout.Id);
            if (layout.Name.Length <= 50 && venue != null && layout.Description.Length <= 120)
            {
                _layoutRepository.Update(layout);
            }
            else
            {
                throw new Exception("Can`t update a layout");
            }
        }

        public void UpdateArea(Area area)
        {
            foreach (var areaElem in _areaRepository.GetAll())
            {
                if (areaElem.Description == area.Description && areaElem.LayoutId == area.LayoutId)
                {
                    throw new Exception("There is already a area with such layout and description");
                }
            }

            var layout = _layoutRepository.FindById(area.LayoutId);
            if (layout != null && area.Description.Length <= 200 && area.CoordX >= 0 && area.CoordY >= 0)
            {
                _areaRepository.Update(area);
            }
            else
            {
                throw new Exception("Can`t update an area");
            }
        }

        public void UpdateSeat(Seat seat)
        {
            foreach (var seatElem in _seatRepository.GetAll())
            {
                if (seatElem.AreaId == seat.AreaId && seatElem.Row == seat.Row && seatElem.Number == seat.Number)
                {
                    throw new Exception("There is already a seat with this coords");
                }
            }

            var area = _areaRepository.FindById(seat.AreaId);
            if (area != null && seat.Row >= 0 && seat.Number >= 0)
            {
                _seatRepository.Update(seat);
            }
            else
            {
                throw new Exception("Can`t update a seat");
            }
        }

        public void DeleteEventSeat(EventSeat eventSeat)
        {
            _eventSeatRepository.Remove(eventSeat);
        }

        public void DeleteEventArea(EventArea eventArea)
        {
            _eventAreaRepository.Remove(eventArea);
        }

        public void DeleteEvent(Event @event)
        {
            _eventRepository.Remove(@event);
        }

        public void DeleteVenue(Venue venue)
        {
            _venueRepository.Remove(venue);
        }

        public void DeleteLayout(Layout layout)
        {
            _layoutRepository.Remove(layout);
        }

        public void DeleteArea(Area area)
        {
            _areaRepository.Remove(area);
        }

        public void DeleteSeat(Seat seat)
        {
            _seatRepository.Remove(seat);
        }
    }
}
