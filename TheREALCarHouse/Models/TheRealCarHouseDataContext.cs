﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CarHouseThree.Models
{
    public class TheRealCarHouseDataContext : DbContext

    {
        //Connection string properties can be defined within base()
        //If left empty, a database will be automatically created
        //see Web.config for an example connection string varsitydatacontext
        public TheRealCarHouseDataContext() : base("name=TheRealCarHouseDataContext")
        {

        }

        //Instruction to set the models as tables in our database.
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Post> Posts { get; set; }


        //To Run the database, use code-first migrations
        //https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx

        //Tools > NuGet Package Manager > Package Manager Console
        //enable-migrations (only once)
        //add-migration {migration name}
        //update-database

        //To View the Database Changes sequentially, go to Project/Migrations folder

        //To View Database itself, go to View > SQL Server Object Explorer
        // (localdb)\MSSQLLocalDB
        // Right Click {ProjectName}.Models.DataContext > Tables
        // Can Right Click Tables to View Data
        // Can Right Click Database to Query

        // You will have to add in some example data to the database locally

    }
}