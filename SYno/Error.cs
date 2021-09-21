﻿using System.Collections.Generic;

namespace SynologyClient
{
    public class Error
    {
        public int code { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public List<Error> errors { get; set; }
    }
}