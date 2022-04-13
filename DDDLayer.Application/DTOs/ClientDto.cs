﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDLayer.Application.DTOs
{
    public class ClientDto
    {
        public string Id { get; set; }
        public string ClientSecret { get; set; }
        public List<String> Audiences { get; set; }
    }
}
