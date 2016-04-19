using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyInvites.Domain.Abstract;
using PartyInvites.Domain.Entities;

namespace PartyInvites.Domain.Concrete {
    public class EFGuestRepository : IGuestRepository {
        private EFDbContext context = new EFDbContext();

        //Get all guests from the database
        public IEnumerable<GuestResponse> GuestResponses {
            get { return context.GuestResponses; }
        }

        //Add a new guest to the database if not already exists
        public bool AddGuest(GuestResponse guestResponse) {
            bool registrationState = true;
            GuestResponse foundGuest = context.GuestResponses.SingleOrDefault(g => g.Email == guestResponse.Email); //Look if guest already exists

            if (foundGuest != null) { //Already registered
                if (!foundGuest.WillAttend.Equals(guestResponse.WillAttend)) {
                    //If WillAttend has changed
                    foundGuest.WillAttend = !foundGuest.WillAttend;
                } else {
                    registrationState = false;
                }
                foundGuest.ImageData = guestResponse.ImageData;
                foundGuest.ImageMimeType = guestResponse.ImageMimeType;
            } else {
                context.GuestResponses.Add(guestResponse); //Add new guest
            }

            context.SaveChanges();
            return registrationState;
        }

    }
}
