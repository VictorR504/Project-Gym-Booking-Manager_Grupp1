using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Gym_Booking_Manager
{
    internal class DB
    {
        public void ReadToList(Database data, string tableName)
        {
            var cs = "Host=localhost;Username=postgres;Password=yokohama123;Database=gbm_db";
            using var con = new NpgsqlConnection(cs);
            con.Open();
            string sql = $"SELECT * FROM {tableName}";
            using var cmd = new NpgsqlCommand(sql, con);
            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            if (tableName == "users")
            {
                while (rdr.Read())
                {
                    data.userObjects.Add(new ReservingEntity(rdr.GetInt32(0),rdr.GetString(1), rdr.GetString(5),
                        rdr.GetString(2), rdr.GetString(3), rdr.GetString(4)));
                }
            }
            else if(tableName == "spaces")
            {
                while (rdr.Read())
                {
                    if (rdr.GetInt32(1) == 0)
                        data.spaceObjects.Add(new Space(rdr.GetInt32(0),Space.Category.Hall, rdr.GetString(2)));
                    else if(rdr.GetInt32(1) == 1)
                        data.spaceObjects.Add(new Space(rdr.GetInt32(0),Space.Category.Lane, rdr.GetString(2)));
                    else if (rdr.GetInt32(1) == 2)
                        data.spaceObjects.Add(new Space(rdr.GetInt32(0),Space.Category.Studio, rdr.GetString(2)));
                    else
                        Console.WriteLine("Something went wrong!!");
                }
            }
            else if (tableName == "equipments")
            {
                while (rdr.Read())
                {
                    if (rdr.GetInt32(1) == 0)
                        data.equipmentObjects.Add(new Equipment(rdr.GetInt32(0), Equipment.Category.Small, rdr.GetString(2)));
                    else if (rdr.GetInt32(1) == 1)
                        data.equipmentObjects.Add(new Equipment(rdr.GetInt32(0), Equipment.Category.Large, rdr.GetString(2)));
                    else
                        Console.WriteLine("Something went wrong!!");
                }
            }
            else if (tableName == "Trainers")
            {
                while (rdr.Read())
                {
                    if (rdr.GetInt32(1) == 0)
                        data.trainerObjects.Add(new Trainer(rdr.GetInt32(0), Trainer.Category.Trainer, rdr.GetString(2)));
                    else if (rdr.GetInt32(1) == 1)
                        data.trainerObjects.Add(new Trainer(rdr.GetInt32(0), Trainer.Category.Consultation, rdr.GetString(2)));
                    else
                        Console.WriteLine("Something went wrong!!");
                }
            }
            else if (tableName == "activitys")
            {
                while (rdr.Read())
                {
                    ReservingEntity user = new ReservingEntity();
                    Trainer trainer = new Trainer();
                    Space space = new Space();
                    Equipment equipment = new Equipment();
                    foreach(ReservingEntity a in data.userObjects) 
                    {
                        if (a.Id == rdr.GetInt32(5))
                           user = a;
                    }
                    foreach(Space b in data.spaceObjects)
                    {
                        if (b.Id == rdr.GetInt32(6))
                            space = b;
                    }
                    foreach(Trainer c in data.trainerObjects)
                    {
                        if(c.Id == rdr.GetInt32(7))
                            trainer = c;
                    }
                    foreach(Equipment d in data.equipmentObjects)
                    {
                        if(d.Id == rdr.GetInt32(8))
                            equipment = d;
                    }
                    data.activities.Add(new Activity(rdr.GetInt32(0), rdr.GetString(1),rdr.GetInt32(2),rdr.GetDateTime(3),
                        rdr.GetDouble(4),user,space,trainer,equipment));
                }
            }

        }
        public void ReadToDb(Database data, string tableName)
        {
            var cs = "Host=localhost;Username=postgres;Password=yokohama123;Database=gbm_db";
            using var con = new NpgsqlConnection(cs);
            con.Open();
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            if(tableName == "users")
            {
                cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $@"CREATE TABLE {tableName}(user_id SERIAL PRIMARY KEY,user_name TEXT,
                                    user_phone TEXT,user_email TEXT,user_status TEXT,user_uniqueID TEXT)";
                cmd.ExecuteNonQuery();
                foreach(ReservingEntity user in data.userObjects)
                {
                    cmd.CommandText = $"INSERT INTO users(user_name, user_phone,user_email,user_status,user_uniqueID) " +
                        $"VALUES('{user.name}','{user.phone}','{user.email}','{user.status}','{user.uniqueID}')";
                    cmd.ExecuteNonQuery();
                }
            }
            else if (tableName == "spaces")
            {
                cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $@"CREATE TABLE {tableName}(space_id SERIAL PRIMARY KEY, category INT, space_name TEXT)";
                cmd.ExecuteNonQuery();
                foreach(Space space in data.spaceObjects)
                {
                    cmd.CommandText = $"INSERT INTO spaces(category,space_name) " +
                        $"VALUES({space.Id},'{space.name}')";
                    cmd.ExecuteNonQuery();
                }

            }
            else if(tableName == "equipments")
            {
                cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $@"CREATE TABLE {tableName}(equipment_id SERIAL PRIMARY KEY, category INT, equipment_name TEXT)";
                cmd.ExecuteNonQuery();
                foreach (Equipment equipment in data.equipmentObjects)
                {
                    cmd.CommandText = $"INSERT INTO equipments(category,equipment_name) " +
                        $"VALUES({equipment.Id},'{equipment.name}')";
                    cmd.ExecuteNonQuery();
                }
            }
            else if(tableName == "Trainers")
            {
                cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $@"CREATE TABLE {tableName}(trainer_id SERIAL PRIMARY KEY, category INT, trainer_name TEXT)";
                cmd.ExecuteNonQuery();
                foreach (Trainer trainer in data.trainerObjects)
                {
                    cmd.CommandText = $"INSERT INTO Trainers(category,trainer_name) " +
                        $"VALUES({trainer.Id},'{trainer.name}')";
                    cmd.ExecuteNonQuery();
                }
            }
            else if(tableName == "activitys")
            {
                cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $@"CREATE TABLE {tableName}(activity_id SERIAL PRIMARY KEY,
	                                                        activity_details TEXT,
	                                                        participant_limits INT,
	                                                        start_time DATE,
	                                                        duration_min float8,
	                                                        activity_owner INT,
	                                                        activity_space INT,
	                                                        activity_trainer INT,
	                                                        activity_equipment INT)";
                cmd.ExecuteNonQuery();
                int index = 0 ;
                foreach (Activity activity in data.activities)
                {
                    var start = activity.timeSlot.reservations[0].startTime;
                    var duration = activity.timeSlot.reservations[0].durationMinutes;
                    var owner = activity.timeSlot.reservations[0].owner.Id;
                    cmd.CommandText = $"INSERT INTO {tableName}(activity_details, participant_limits,start_time," +
                        $"duration_min, activity_owner, activity_space, activity_trainer, activity_equipment) " +
                        $"VALUES('{activity.activityDetails}',{activity.participantLimit},'{start}'," +
                        $"{duration}, {owner}, {activity.space.Id}, {activity.trainer.Id}, {activity.equipment.Id})";
                    cmd.ExecuteNonQuery();
                    index++;
                }
            }

        }
    }
}
