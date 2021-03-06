﻿using System;

namespace HackathonCCR.MVC.Models.Schedule
{
    public class UserSchedule
    {
        public Guid ScheduleId { get; set; }
        public string PartnerPicture { get; set; }
        public string PartnerName { get; set; }
        public string PartnerFirstName { get; set; }
        public string Course { get; set; }
        public string CourseColor { get; set; }
        public string ScheduleTime { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleDateMonth { get; set; }
        public string ScheduleDateDay { get; set; }
    }
}
