﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Sender.Model
{
    public class CoffeeMachineData
    {
        public string City { get; set; }
        public string SerialNumber { get; set; }
        public string SensorType { get; set; }
        public int SensorValue { get; set; }
        public DateTime RecordingTime { get; set; }
    }
}
