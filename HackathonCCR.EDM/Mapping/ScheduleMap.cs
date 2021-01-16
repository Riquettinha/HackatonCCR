﻿using System.Data.Entity.ModelConfiguration;

namespace HackathonCCR.EDM.Models.Mapping
{
    public class ScheduleMap : EntityTypeConfiguration<Schedule>
    {
        public ScheduleMap()
        {
            ToTable("Schedule");

            HasKey(t => t.ScheduleId);
            Property(t => t.ScheduleId).HasColumnName("ScheduleId");
            Property(t => t.MentorId).HasColumnName("MentorId").IsRequired();
            Property(t => t.MentoredId).HasColumnName("MentoredId");
            Property(t => t.ScheduleAt).HasColumnName("Time").IsRequired();
            Property(t => t.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(t => t.AppointmentId).HasColumnName("AppointmentId");
            Property(t => t.Status).HasColumnName("Status").IsRequired();

            HasRequired(t => t.Mentor)
                            .WithMany(t => t.Mentorings)
                            .HasForeignKey(d => d.MentorId)
                            .WillCascadeOnDelete(false);

            HasRequired(t => t.Category)
                            .WithMany(t => t.Schedules)
                            .HasForeignKey(d => d.CategoryId)
                            .WillCascadeOnDelete(false);

            HasOptional(t => t.Mentored)
                            .WithMany(t => t.Schedules)
                            .HasForeignKey(d => d.MentoredId)
                            .WillCascadeOnDelete(false);
        }
    }
}
