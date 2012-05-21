﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchedulerClient
{
    class Task
    {
        public string Title
        { get; set; }
        public string Notes
        { get; set; }
        public string StartTimeDate
        { get; set; }
        public string EndTimeDate
        { get; set; }
        public string Place
        { get; set; }
        public int Index
        { get; set; }
        public float Top
        { get; set; }
    }
}
