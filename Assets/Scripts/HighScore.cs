using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//added namespaces
using System;
using System.Linq;
using System.Text;

public class HighScore
{
    //must be created this way so the variable can be accessed properly
    public int ID { get; set; }
    public int Score { get; set; }

    public string Name { get; set; }

    public HighScore(int id, int score, string name)
    {
        this.Score = score;
        this.ID = id;
        this.Name = name;
    }

}