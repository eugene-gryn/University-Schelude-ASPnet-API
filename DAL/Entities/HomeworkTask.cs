﻿using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class HomeworkTask
{
    [Key] public int Id { get; set; }

    [MaxLength(1000)] public string? Description { get; set; }

    [Required, DataType(DataType.DateTime)] public TimeSpan Deadline { get; set; }

    [Required] public byte Priority { get; set; } = 5;

    [Required] public Subject Subject { get; set; } = new();
}