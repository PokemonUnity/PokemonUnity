using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialog {
    public DialogTriggerBehaviour DialogTriggers { get; }
    public DialogSeries DialogSeries { get; set; }
}
