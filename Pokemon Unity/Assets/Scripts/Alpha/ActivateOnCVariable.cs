//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class ActivateOnCVariable : MonoBehaviour
{
    public string cVariable;

    public enum Check
    {
        Equal,
        LessThan,
        GreaterThan
    }

    public bool not = false;
    public Check check;
    public float cNumber;

    public GameObject target;

    void Awake()
    {
        if (target == null)
        {
            target = this.transform.GetChild(0).gameObject;
        }
    }

    void Start()
    {
        //CheckActivation();
    }


    //private void CheckActivation()
    //{
    //    bool checkResult = false;
    //    if (check == Check.Equal)
    //    {
    //        if (SaveDataOld.currentSave.getCVariable(cVariable) == cNumber)
    //        {
    //            checkResult = true;
    //        }
    //    }
    //    else if (check == Check.GreaterThan)
    //    {
    //        if (SaveDataOld.currentSave.getCVariable(cVariable) > cNumber)
    //        {
    //            checkResult = true;
    //        }
    //    }
    //    else if (check == Check.LessThan)
    //    {
    //        if (SaveDataOld.currentSave.getCVariable(cVariable) < cNumber)
    //        {
    //            checkResult = true;
    //        }
    //    }
	//
    //    if (not)
    //    {
    //        //invert bool
    //        checkResult = (checkResult) ? false : true;
    //    }
    //    target.SetActive(checkResult);
    //}
}