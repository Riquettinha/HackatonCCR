﻿using HackathonCCR.EDM.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace HackathonCCR.EDM.Context
{
    public interface IHackathonCCRContext : IDisposable
    {
        Database Database { get; }
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbContextConfiguration Configuration { get; }
        DbSet<User> User { get; set; }
        int SaveChanges();
    }
}