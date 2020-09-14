using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DomainEntities;

namespace BuisnessLayer
{
    // Class for making interaction between Data Access Layer and user
    public class BLL
    {
        // Instances of repositories
        private AreaSqlRepository _areaRepository;

        private EventAreaSqlRepository _eventAreaRepository;

        private EventSqlRepository _eventRepository;

        private EventSeatSqlRepository _eventSeatRepository;

        private LayoutSqlRepository _layoutRepository;

        private SeatSqlRepository _seatRepository;

        private VenueSqlRepository _venueRepository;

        // Constructors for layer with base repositories and users` repositories
        public BLL()
        {
            _areaRepository = new AreaSqlRepository();

            _eventAreaRepository = new EventAreaSqlRepository();

            _eventRepository = new EventSqlRepository();

            _eventSeatRepository = new EventSeatSqlRepository();

            _layoutRepository = new LayoutSqlRepository();

            _seatRepository = new SeatSqlRepository();

            _venueRepository = new VenueSqlRepository();
        }

        public BLL(AreaSqlRepository areaSqlRepository, EventAreaSqlRepository eventAreaSqlRepository, EventSqlRepository eventSqlRepository, EventSeatSqlRepository eventSeatSqlRepository,
            LayoutSqlRepository layoutSqlRepository, SeatSqlRepository seatSqlRepository, VenueSqlRepository venueSqlRepository)
        {
            _areaRepository = areaSqlRepository;

            _eventAreaRepository = eventAreaSqlRepository;

            _eventRepository = eventSqlRepository;

            _eventSeatRepository = eventSeatSqlRepository;

            _layoutRepository = layoutSqlRepository;

            _seatRepository = seatSqlRepository;

            _venueRepository = venueSqlRepository;
        }

        // Method that adds event after checking data validation
        public void AddEvent(int id, string name, string description, int layoutId, DateTime startDate, DateTime endDate)
        {
            var layout = _layoutRepository.FindById(layoutId);
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
                    if (venueId == venue_check.Id && ((startDate >= eventElem.StartDate && startDate < eventElem.StartDate) || (endDate > eventElem.StartDate && endDate <= eventElem.EndDate) || (startDate <= eventElem.StartDate && endDate >= eventElem.EndDate)))
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

                if (isSeatExists == true && name.Length <= 120 && description.Length <= 400 && startDate >= DateTime.Now && endDate >= startDate)
                {
                    _eventRepository.Create(new Event(id, name, description, layoutId, startDate, endDate));
                }
                else
                {
                    throw new Exception("Can`t add an event");
                }
            }
        }

        // Method that adds venue after checking data validation
        public void AddVenue(int id, string name, string description, string address, string phone)
        {
            foreach (var venueElem in _venueRepository.GetAll())
            {
                if (venueElem.Name == name)
                {
                    throw new Exception("There is already a venue with such name");
                }
            }

            if (name.Length <= 50 && description.Length <= 120 && address.Length <= 150 && phone.Length <= 15)
            {
                _venueRepository.Create(new Venue(id, name, description, address, phone));
            }
            else
            {
                throw new Exception("Can`t add a venue");
            }
        }

        // Method that adds layout after checking data validation
        public void AddLayout(int id, string name, int venueId, string description)
        {
            foreach (var layoutElem in _layoutRepository.GetAll())
            {
                if (layoutElem.Name == name && layoutElem.VenueId == venueId)
                {
                    throw new Exception("There is already a layout with such venue");
                }
            }

            Venue venue = _venueRepository.FindById(venueId);
            if (name.Length <= 50 && venue != null && description.Length <= 120)
            {
                _layoutRepository.Create(new Layout(id, name, venueId, description));
            }
            else
            {
                throw new Exception("Can`t add a layout");
            }
        }

        // Method that adds area after checking data validation
        public void AddArea(int id, int layoutId, string description, int coordX, int coordY)
        {
            foreach (Area areaElem in _areaRepository.GetAll())
            {
                if (areaElem.Description == description && areaElem.LayoutId == layoutId)
                {
                    throw new Exception("There is already a area with such layout and description");
                }
            }

            var layout = _layoutRepository.FindById(layoutId);
            if (layout != null && description.Length <= 200 && coordX >= 0 && coordY >= 0)
            {
                _areaRepository.Create(new Area(id, layoutId, description, coordX, coordY));
            }
            else
            {
                throw new Exception("Can`t add an area");
            }
        }

        // Method that adds seat after checking data validation
        public void AddSeat(int id, int areaId, int row, int number)
        {
            foreach (var seatElem in _seatRepository.GetAll())
            {
                if (seatElem.AreaId == areaId && seatElem.Row == row && seatElem.Number == number)
                {
                    throw new Exception("There is already a seat with this coords");
                }
            }

            var area = _areaRepository.FindById(areaId);
            if (area != null && row >= 0 && number >= 0)
            {
                _seatRepository.Create(new Seat(id, areaId, row, number));
            }
            else
            {
                throw new Exception("Can`t add a seat");
            }
        }

        // Method that adds event area after checking data validation
        public void AddEventArea(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            foreach (var eventAreaElem in _eventAreaRepository.GetAll())
            {
                if (eventAreaElem.EventId == eventId && eventAreaElem.CoordX == coordX && coordY == eventAreaElem.CoordY)
                {
                    throw new Exception("There is already an event area with such coords");
                }
            }

            var @event = _eventRepository.FindById(eventId);
            if (@event != null && description.Length <= 200 && coordX >= 0 && coordY >= 0 && price >= 0)
            {
                _eventAreaRepository.Create(new EventArea(id, eventId, description, coordX, coordY, price));
            }
            else
            {
                throw new Exception("Can`t add an event area");
            }
        }

        // Method that adds event seat after checking data validation
        public void AddEventSeat(int id, int eventAreaId, int row, int number, int state)
        {
            foreach (var eventSeatElem in _eventSeatRepository.GetAll())
            {
                if (eventSeatElem.EventAreaId == eventAreaId && eventSeatElem.Row == row && eventSeatElem.Number == number)
                {
                    throw new Exception("There is already an event seat with such coords");
                }
            }

            var eventArea = _eventAreaRepository.FindById(eventAreaId);
            if (eventArea != null && row >= 0 && number >= 0)
            {
                _eventSeatRepository.Create(new EventSeat(id, eventAreaId, row, number, state));
            }
            else
            {
                throw new Exception("Can`t add an event seat");
            }
        }

        // Method that updates event seat after checking data validation
        public void UpdateEventSeat(int id, int eventAreaId, int row, int number, int state)
        {
            foreach (var eventSeatElem in _eventSeatRepository.GetAll())
            {
                if (eventSeatElem.EventAreaId == eventAreaId && eventSeatElem.Row == row && eventSeatElem.Number == number)
                {
                    throw new Exception("There is already an event seat with such coords");
                }
            }

            var eventArea = _eventAreaRepository.FindById(eventAreaId);
            if (eventArea != null && row >= 0 && number >= 0)
            {
                _eventSeatRepository.Update(new EventSeat(id, eventAreaId, row, number, state));
            }
            else
            {
                throw new Exception("Can`t update an event seat");
            }
        }

        // Method that updates event area after checking data validation
        public void UpdateEventArea(int id, int eventId, string description, int coordX, int coordY, decimal price)
        {
            foreach (var eventAreaElem in _eventAreaRepository.GetAll())
            {
                if (eventAreaElem.EventId == eventId && eventAreaElem.CoordX == coordX && coordY == eventAreaElem.CoordY)
                {
                    throw new Exception("There is already an event area with such coords");
                }
            }

            var @event = _eventRepository.FindById(eventId);
            if (@event != null && description.Length <= 200 && coordX >= 0 && coordY >= 0 && price >= 0)
            {
                _eventAreaRepository.Update(new EventArea(id, eventId, description, coordX, coordY, price));
            }
            else
            {
                throw new Exception("Can`t update an event area");
            }
        }

        // Method that updates event after checking data validation
        public void UpdateEvent(int id, string name, string description, int layoutId, DateTime startDate, DateTime endDate)
        {
            var layout = _layoutRepository.FindById(layoutId);
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
                    if (venueId == venue_check.Id && ((startDate >= eventElem.StartDate && startDate < eventElem.EndDate) || (endDate > eventElem.StartDate && endDate <= eventElem.EndDate) || (startDate <= eventElem.StartDate && endDate >= eventElem.EndDate)) && eventElem.Id != id)
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

                if (isSeatExists == true && name.Length <= 120 && description.Length <= 400 && startDate >= DateTime.Now && endDate >= startDate)
                {
                    _eventRepository.Update(new Event(id, name, description, layoutId, startDate, endDate));
                }
                else
                {
                    throw new Exception("Can`t update an event");
                }
            }
        }

        // Method that updates venue after checking data validation
        public void UpdateVenue(int id, string name, string description, string address, string phone)
        {
            foreach (var venueElem in _venueRepository.GetAll())
            {
                if (venueElem.Name == name)
                {
                    throw new Exception("There is already a venue with such name");
                }
            }

            if (name.Length <= 50 && description.Length <= 120 && address.Length <= 150 && phone.Length <= 15)
            {
                _venueRepository.Update(new Venue(id, name, description, address, phone));
            }
            else
            {
                throw new Exception("Can`t update a venue");
            }
        }

        // Method that updates layout after checking data validation
        public void UpdateLayout(int id, string name, int venueId, string description)
        {
            foreach (var layoutElem in _layoutRepository.GetAll())
            {
                if (layoutElem.Name == name && layoutElem.VenueId == venueId)
                {
                    throw new Exception("There is already a layout with such venue");
                }
            }

            Venue venue = _venueRepository.FindById(venueId);
            if (name.Length <= 50 && venue != null && description.Length <= 120)
            {
                _layoutRepository.Update(new Layout(id, name, venueId, description));
            }
            else
            {
                throw new Exception("Can`t update a layout");
            }
        }

        // Method that updates area after checking data validation
        public void UpdateArea(int id, int layoutId, string description, int coordX, int coordY)
        {
            foreach (var areaElem in _areaRepository.GetAll())
            {
                if (areaElem.Description == description && areaElem.LayoutId == layoutId)
                {
                    throw new Exception("There is already a area with such layout and description");
                }
            }

            var layout = _layoutRepository.FindById(layoutId);
            if (layout != null && description.Length <= 200 && coordX >= 0 && coordY >= 0)
            {
                _areaRepository.Update(new Area(id, layoutId, description, coordX, coordY));
            }
            else
            {
                throw new Exception("Can`t update an area");
            }
        }

        // Method that updates seat after checking data validation
        public void UpdateSeat(int id, int areaId, int row, int number)
        {
            foreach (var seatElem in _seatRepository.GetAll())
            {
                if (seatElem.AreaId == areaId && seatElem.Row == row && seatElem.Number == number)
                {
                    throw new Exception("There is already a seat with this coords");
                }
            }

            var area = _areaRepository.FindById(areaId);
            if (area != null && row >= 0 && number >= 0)
            {
                _seatRepository.Update(new Seat(id, areaId, row, number));
            }
            else
            {
                throw new Exception("Can`t update a seat");
            }
        }

        // Method that deletes event seat
        public void DeleteEventSeat(int id)
        {
            _eventSeatRepository.Remove(id);
        }

        // Method that deletes event area
        public void DeleteEventArea(int id)
        {
            _eventAreaRepository.Remove(id);
        }

        // Method that deletes event
        public void DeleteEvent(int id)
        {
            _eventRepository.Remove(id);
        }

        // Method that deletes venue
        public void DeleteVenue(int id)
        {
            _venueRepository.Remove(id);
        }

        // Method that deletes layout
        public void DeleteLayout(int id)
        {
            _layoutRepository.Remove(id);
        }

        // Method that deletes area
        public void DeleteArea(int id)
        {
            _areaRepository.Remove(id);
        }

        // Method that deletes seat
        public void DeleteSeat(int id)
        {
            _seatRepository.Remove(id);
        }

        // Method that reads event seat
        public EventSeat ReadEventSeat(int id)
        {
            return _eventSeatRepository.FindById(id);
        }

        // Method that reads event area
        public EventArea ReadEventArea(int id)
        {
            return _eventAreaRepository.FindById(id);
        }

        // Method that reads event
        public Event ReadEvent(int id)
        {
            return _eventRepository.FindById(id);
        }

        // Method that reads venue
        public Venue ReadVenue(int id)
        {
            return _venueRepository.FindById(id);
        }

        // Method that reads layout
        public Layout ReadLayout(int id)
        {
            return _layoutRepository.FindById(id);
        }

        // Method that reads area
        public Area ReadArea(int id)
        {
            return _areaRepository.FindById(id);
        }

        // Method that reads seat
        public Seat ReadSeat(int id)
        {
            return _seatRepository.FindById(id);
        }

        // Method that fills event repository with data from database
        public void FillEventRepository()
        {
            _eventRepository.FillRepositoryWithSqlData();
        }

        // Method that fills event area repository with data from database
        public void FillEventAreaRepository()
        {
            _eventAreaRepository.FillRepositoryWithSqlData();
        }

        // Method that fills event seat repository with data from database
        public void FillEventSeatRepository()
        {
            _eventSeatRepository.FillRepositoryWithSqlData();
        }

        // Method that fills venue repository with data from database
        public void FillVenueRepository()
        {
            _venueRepository.FillRepositoryWithSqlData();
        }

        // Method that fills layout repository with data from database
        public void FillLayoutRepository()
        {
            _layoutRepository.FillRepositoryWithSqlData();
        }

        // Method that fills area repository with data from database
        public void FillAreaRepository()
        {
            _areaRepository.FillRepositoryWithSqlData();
        }

        // Method that fills seat repository with data from database
        public void FillSeatRepository()
        {
            _seatRepository.FillRepositoryWithSqlData();
        }
    }
}
