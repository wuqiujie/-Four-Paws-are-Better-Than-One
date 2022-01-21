using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Classification { Human, Animal }

[Serializable]
public class Subject
{
    public string subjectName;
    public string subjectTypeStr;
    public string subjectPictureFilePath;

    public Classification subjectClassification {
        get 
        {
            return (Classification)Enum.Parse(typeof(Classification), subjectTypeStr);
        }
    }

    public Sprite subjectPicture {
        get
        {
            return Resources.Load<Sprite>(subjectPictureFilePath);
        }
    }

    // Can also contain any other fields necessary for art assets
}

[Serializable]
public class Subjects
{
    public Subject[] animals;
    public Subject[] humans;
}
