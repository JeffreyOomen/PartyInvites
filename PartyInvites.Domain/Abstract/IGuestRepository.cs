using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyInvites.Domain.Entities;

namespace PartyInvites.Domain.Abstract {
    public interface IGuestRepository {
        //Return all guests from the database
        IEnumerable<GuestResponse> GuestResponses { get; }

        //Saves guestresponse object in database
        bool AddGuest(GuestResponse guestResponse);
    }
}
