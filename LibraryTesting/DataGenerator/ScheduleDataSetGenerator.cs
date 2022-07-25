using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;

namespace LibraryTesting.DataGenerator;

public class ScheduleDataSetGenerator
{
    private readonly Random _random = new();


    public List<Couple> Couples { get; } = new();
    public List<Group> Groups { get; } = new();
    public List<HomeworkTask> Homework { get; } = new();
    public List<Subject> Subjects { get; } = new();
    public List<User> Users { get; } = new();

    public List<User> RGenerate(int count)
    {
        Couples.Clear();
        Groups.Clear();
        Homework.Clear();
        Subjects.Clear();
        Users.Clear();

        for (var i = 0; i < count; i++) Users.Add(UserGenerate(Users.Count, new List<Group>(), new List<HomeworkTask>()));

        // Generate owners
        foreach (var user in Users.Where(user => RBool()))
        {
            //--->Group

            // Group generation
            var group = GroupGenerate(Groups.Count, user, new List<Subject>(), new List<User>(), new List<User>(),
                new List<Couple>());

            // Fill Moderators
            var moderators = Users.Where(moder => RBool() && user.Id != moder.Id).ToList();
            foreach (var moderator in moderators) moderator.Groups.Add(group);
            group.Moderators.AddRange(moderators);

            // Fill Users
            var dUsers = group.Moderators.Except(Users).ToList();
            foreach (var dUser in dUsers) dUser.Groups.Add(group);
            group.Users.AddRange(dUsers);


            user.Groups.Add(group);

            Groups.Add(group);

            //-->Subject
            var subjectCount = _random.Next(7, 14);

            for (var i = 0; i < subjectCount; i++)
            {
                var subj = SubjectGenerate(Subjects.Count, group);
                group.Subjects.Add(subj);
                Subjects.Add(subj);
            }

            //-->Couples

            var coupleCount = _random.Next(20, 40);

            for (var i = 0; i < coupleCount; i++)
            {
                var couple = CoupleGenerate(Couples.Count, group.Subjects[_random.Next(0, group.Subjects.Count - 1)]);
                group.Couples.Add(couple);
                Couples.Add(couple);
            }

            //--->HomeworkTask

            var uSubj = new List<Subject>();
            user.Groups.ForEach(groupU => uSubj.AddRange(groupU.Subjects));

            var homeworkCount = _random.Next(0, count * Users.Count + 1);
            for (var i = 0; i < homeworkCount; i++)
            {
                var homeW = HomeworkGenerate(Homework.Count, uSubj[_random.Next(0, uSubj.Count - 1)]);
                user.Homeworks.Add(homeW);
                Homework.Add(homeW);
            }
        }

        return Users;
    }

    public void Clear()
    {
        Couples.Clear();
        Groups.Clear();
        Homework.Clear();
        Subjects.Clear();
        Users.Clear();
    }

    #region DataGenerators

    private string RString(int length = 10)
    {
        const string chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    private int RNum()
    {
        return _random.Next(int.MaxValue);
    }

    private bool RBool()
    {
        return _random.Next(2) == 1;
    }

    private byte[] RByteArr()
    {
        var result = new byte[20];
        _random.NextBytes(result);
        return result;
    }

    #endregion

    #region EntityGenerators

    /*User*/
    private User UserGenerate(int id, List<Group> groups, List<HomeworkTask> home)
    {
        var user = new User
        {
            Id = id,
            Login = RString(),
            Name = RString(),
            IsAdmin = RBool(),
            ImageLocation = RString(),
            Password = RByteArr(),
            Salt = RByteArr(),
            Settings = RSettings(),
            Groups = groups,
            Homeworks = home
        };


        return user;
    }

    /*Setting*/
    private Settings RSettings()
    {
        var settings = new Settings
        {
            NotifyBeforeCouple = RBool(),
            NotifyAboutCouple = RBool(),
            NotifyAboutHomework = RBool(),
            NotifyAboutDeadlineHomework = RBool(),
            NotifyAboutLoseDeadlineHomework = RBool()
        };

        return settings;
    }

    /*Group*/
    private Group GroupGenerate(int id, User creator, List<Subject> subj, List<User> users, List<User> moderator,
        List<Couple> couples)
    {
        var group = new Group
        {
            Id = id,
            Name = RString(),
            PrivateType = RBool(),
            Creator = creator,
            Subjects = subj,
            Users = users,
            Couples = couples,
            Moderators = moderator
        };
        return group;
    }

    /*Coupe*/
    private Couple CoupleGenerate(int id, Subject subject)
    {
        var couple = new Couple
        {
            Id = id,
            Begin = DateTime.Now,
            End = DateTime.Now.AddHours(1),
            Subject = subject
        };
        return couple;
    }

    /*HomeworkTask*/
    private HomeworkTask HomeworkGenerate(int id, Subject subject)
    {
        var homework = new HomeworkTask
        {
            Id = id,
            Description = RString(),
            Deadline = TimeSpan.FromHours(_random.Next(10, 100)),
            Priority = (byte) _random.Next(10),
            Subject = subject
        };
        return homework;
    }

    /*Subject*/
    private Subject SubjectGenerate(int id, Group group)
    {
        var subject = new Subject
        {
            Id = id,
            Name = RString(),
            IsPractice = RBool(),
            OwnerGroup = group,
            Url = RString(),
            Location = RString(),
            Teacher = RString()
        };
        return subject;
    }

    #endregion
}