﻿using System;

namespace Dto
{
    public class OrderReadDto
    {
        public int ID { get; set; }
        public string Details { get; set; }
        public DateTimeOffset AddOrderDate { get; set; }
        public DateTimeOffset? DeliveryOrderDate { get; set; }
        public double? StartLongitude { get; set; }
        public double? StartLatitude { get; set; }
        public double EndLongitude { get; set; }
        public double EndLatitude { get; set; }
        public string ReceiptImageUrl { get; set; }
        public UserReadDto User { get; set; }

    }
}