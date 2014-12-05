using Home.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Home.Handlers
{
    public class HomeHandler : ContentHandler
    {
        public HomeHandler(IRepository<HomePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}