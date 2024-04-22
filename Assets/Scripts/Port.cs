using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    public enum PortType { In, Out };

    public PortType portType;
    //public Port connectedPort;
    public Connection Connection { get; set; }
    public int anomalyValue = 0;

}
