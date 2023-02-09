﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#if DEBUG
[assembly: InternalsVisibleTo("Tests")]
#endif
namespace Gym_Booking_Manager
{
    //
    internal class GroupSchedule
    {
        public List<GroupActivity> activities = new List<GroupActivity>();
        public void ViewSchedule(ReservingEntity user)
        {
            //Todo

            foreach(GroupActivity activity in activities)
            {
                if (activity.participants.Contains(user))
                {
                    Console.WriteLine(activity);    
                }
            }
        }
        public void AddActivity(ReservingEntity user, string activityDetails)
        {
            //Todo
        }
        public void UpdateActivity(ReservingEntity user, string activityDetails, string activityID)
        {
            //Todo
        }
        public void RemoveActivity(ReservingEntity user, string activityID)
        {
            //Todo
            foreach (GroupActivity activity in activities)
            {
                if (user.status == "Member")
                {
                    //Do something
                    if(activity.activityID == activityID)
                    {
                        activity.participants.Remove(user);
                        Console.WriteLine($"The User {user.name} was removed from activity {activity.activityID}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"The User was not found in the schedule.");
                    }
                }
                else if (user.status == "Staff")
                {
                    if (activity.activityID == activityID)
                    {
                        activities.Remove(activity);
                        Console.WriteLine($"The activity with ID {activityID} has been removed from the schedule.");
                        // Here we need a method so that the activity gets removed from the Database!!
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"The activity with ID {activityID} was not found in the schedule.");
                    }
                }
            }
        }
        public override string ToString()
        {
            return $"{activities}";
        }

    }
}

