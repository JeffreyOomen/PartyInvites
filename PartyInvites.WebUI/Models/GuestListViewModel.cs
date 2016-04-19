using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PartyInvites.Domain.Entities;

namespace PartyInvites.WebUI.Models {
    public class GuestListViewModel {
        public IEnumerable<GuestResponse> GuestResponses { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public string CurrentCategory { get; set; }
    }
}