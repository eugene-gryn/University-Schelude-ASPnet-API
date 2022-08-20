using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using DAL.Entities;

namespace LibraryTesting.DataGenerator;

public class ScheduleRandomGenerator {
    private readonly Random _random = new();


    public List<Couple> Couples { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public List<HomeworkTask> Homework { get; set; } = new();
    public List<Subject> Subjects { get; set; } = new();
    public List<User> Users { get; set; } = new();


    public void MakeDataSet(int count) {
        Clear();

        // Generate users
        Users.AddRange(GenEmptyUsers(count));

        // Generate owners
        var exc = Users.Where((u, i) => i == 1 || RBool());

        foreach (var user in exc) {
            // Group generation
            var group = GenEmptyGroups(1, user).First();
            user.UsersRoles.Add(group.UsersRoles.First());
            Groups.Add(group);

            var users = Users.Where(userS => RBool() && userS.Id != user.Id).ToList();
            var moderators = Users.Except(users).Where(usr => usr.Id != user.Id).ToList();
            users.ForEach(usr => {
                UserRole roleUser = new() {
                    Group = group,
                    GroupId = group.Id,
                    User = usr,
                    UserId = usr.Id,
                    IsModerator = false,
                    IsOwner = false
                };
                group.UsersRoles.Add(roleUser);
                usr.UsersRoles.Add(roleUser);
            });
            moderators.ForEach(usr => {
                UserRole roleUser = new() {
                    Group = group,
                    GroupId = group.Id,
                    User = usr,
                    UserId = usr.Id,
                    IsModerator = true,
                    IsOwner = false
                };
                group.UsersRoles.Add(roleUser);
                usr.UsersRoles.Add(roleUser);
            });


            //-->Subject
            var subjectCount = _random.Next(7, 14);

            for (var i = 0; i < subjectCount; i++) {
                var subj = SubjectGenerate(Subjects.Count + 1, group);
                Subjects.Add(subj);
                group.Subjects.Add(subj);
            }

            //-->Couples

            var coupleCount = _random.Next(20, 40);

            for (var i = 0; i < coupleCount; i++) {
                var couple = CoupleGenerate(Couples.Count + 1,
                    group.Subjects.ToList()[_random.Next(1, group.Subjects.Count - 1)], group);
                group.Couples.Add(couple);
                Couples.Add(couple);
            }
        }

        Users.ForEach(usr => {
            usr.UsersRoles.Select(role => role.Group).ToList().ForEach(group => {
                group.Subjects.ToList().ForEach(subj => {
                    var task = HomeworkGenerate(Homework.Count + 1, subj, usr);
                    usr.Homework.Add(task);
                    Homework.Add(task);
                });
            });
        });
    }


    private User GenEmptyUser(int index = 1) {
        return UserGenerate(Users.Count + index, new List<UserRole>(), new List<HomeworkTask>());
    }

    public List<User> GenEmptyUsers(int count) {
        var users = new List<User>(count);

        for (var i = 0; i < count; i++) users.Add(GenEmptyUser(i + 1));

        return users;
    }

    public List<Group> GenEmptyGroups(int count = 1, User creator = null) {
        var groups = new List<Group>(count);

        for (var i = 0; i < count; i++) {
            var group = GroupGenerate(Groups.Count + i + 1,
                new List<Subject>(), new EditableList<UserRole>(), new List<Couple>());
            groups.Add(group);
            if (creator != null)
                groups.LastOrDefault()?.UsersRoles.Add(new UserRole {
                    User = creator,
                    UserId = creator.Id,
                    Group = group,
                    GroupId = group.Id,
                    IsModerator = false,
                    IsOwner = true
                });
        }


        return groups;
    }


    public List<Couple> GenEmptyCouple(int count = 1, Subject subject = null, Group group = null) {
        var couples = new List<Couple>(count);

        for (var i = 0; i < count; i++)
            couples.Add(CoupleGenerate(Couples.Count + 1, subject, group));

        return couples;
    }

    public List<Subject> GenEmptySubject(int count = 1, Group group = null) {
        var subjects = new List<Subject>(count);

        for (var i = 0; i < count; i++)
            subjects.Add(SubjectGenerate(Subjects.Count + 1, group));

        return subjects;
    }

    public List<HomeworkTask> GenEmptyHomework(int count = 1, Subject subject = null, User user = null) {
        var homeworkTasks = new List<HomeworkTask>(count);

        for (var i = 0; i < count; i++)
            homeworkTasks.Add(HomeworkGenerate(Homework.Count + 1, subject, user));

        return homeworkTasks;
    }

    public void Clear() {
        Couples.Clear();
        Groups.Clear();
        Homework.Clear();
        Subjects.Clear();
        Users.Clear();
    }


    #region DataGenerators

    private string RString(int length = 10) {
        const string chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    private int RNum() {
        return _random.Next(int.MaxValue);
    }

    private bool RBool() {
        return _random.Next(2) == 1;
    }

    private byte[] RByteArr() {
        var result = new byte[20];
        _random.NextBytes(result);
        return result;
    }

    #endregion


    #region EntityGenerators

    /*User*/
    public User UserGenerate(int id, List<UserRole> groups, List<HomeworkTask> home) {
        var user = new User {
            Id = id,
            Login = RString(),
            Name = RString(),
            IsAdmin = RBool(),
            ImageLocation = RString(),
            Password = RByteArr(),
            Salt = RByteArr(),
            Settings = SettingsGenerate(),
            UsersRoles = groups,
            Homework = home
        };


        return user;
    }

    /*Setting*/
    public Settings SettingsGenerate() {
        var settings = new Settings {
            NotifyBeforeCouple = RBool(),
            NotifyAboutCouple = RBool(),
            NotifyAboutHomework = RBool(),
            NotifyAboutDeadlineHomework = RBool(),
            NotifyAboutLoseDeadlineHomework = RBool()
        };

        return settings;
    }

    /*Group*/
    public Group GroupGenerate(int id, List<Subject> subj, List<UserRole> users,
        List<Couple> couples) {
        var group = new Group {
            Id = id,
            Name = RString(),
            PrivateType = RBool(),
            Subjects = subj,
            UsersRoles = users,
            Couples = couples
        };
        return group;
    }

    /*Coupe*/
    public Couple CoupleGenerate(int id, Subject subject, Group group) {
        var couple = new Couple {
            Id = id,
            Begin = DateTime.Now,
            End = DateTime.Now.AddHours(2),
            Subject = subject,
            SubjectId = subject?.Id ?? 0,
            Group = group,
            GroupId = group?.Id ?? 0
        };
        return couple;
    }

    /*HomeworkTask*/
    public HomeworkTask HomeworkGenerate(int id, Subject subject, User user) {
        var homework = new HomeworkTask {
            Id = id,
            Description = RString(),
            Deadline = DateTime.Now.AddDays(_random.Next(10, 100)),
            Priority = (byte)_random.Next(10),
            Subject = subject,
            SubjectId = subject.Id,
            User = user,
            UserId = user.Id
        };
        return homework;
    }

    /*Subject*/
    public Subject SubjectGenerate(int id, Group group) {
        var subject = new Subject {
            Id = id,
            Name = RString(),
            IsPractice = RBool(),
            Url = RString(),
            Location = RString(),
            Teacher = RString(),
            OwnerGroup = group,
            GroupId = group.Id
        };
        return subject;
    }

    #endregion
}