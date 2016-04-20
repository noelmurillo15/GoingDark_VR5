using Leap;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeapData : MonoBehaviour {
    //**    Attach Script to LeapController   **//

    public bool LeapConnected;
    private bool bothHandsOpen;
    private Controller controller;

    private int prevFrameNum = 0;
    private int numHands, prevNumHands;
    private int numFingersL, numFingersR;
    public int numFingersLHeld, numFingersRHeld;
    private int prevNumFingersL, prevNumFingersR;
    private int prevFingersHeldL, prevFingersHeldR;

    private float gripStrL, gripStrR;
    private float prevGripStrL, prevGripStrR;

    private Vector3 palmPosL;
    private Vector3 palmPosR;
    private Vector3 palmNormsL;
    private Vector3 palmNormsR;
    private Vector3 palmVelocityL;
    public Vector3 palmVelocityR;
    private Vector3 prevPalmPosL;
    private Vector3 prevPalmPosR;
    private Vector3 prevPalmNormL;
    private Vector3 prevPalmNormR;
    private Vector3 prevPalmVelocityL;
    private Vector3 prevPalmVelocityR;

    private List<Hand> handList;
    private List<Hand> prevHandList;
    private List<Finger> fingerListL, fingerListR;
    private List<Finger> prevFingerListL, prevFingerListR;    


    void Awake() {        
        numHands = 0;
        numFingersL = 0;
        numFingersR = 0;
        numFingersLHeld = 0;
        numFingersRHeld = 0;
        palmPosL = Vector3.zero;
        palmPosR = Vector3.zero;
        palmNormsL = Vector3.zero;
        palmNormsR = Vector3.zero;
        palmVelocityL = Vector3.zero;
        palmVelocityR = Vector3.zero;

        prevNumHands = 0;
        prevNumFingersL = 0;
        prevNumFingersR = 0;
        prevFingersHeldL = 0;
        prevFingersHeldR = 0;
        prevPalmPosL = Vector3.zero;
        prevPalmPosR = Vector3.zero;
        prevPalmNormL = Vector3.zero;
        prevPalmNormR = Vector3.zero;
        prevPalmVelocityL = Vector3.zero;
        prevPalmVelocityR = Vector3.zero;

    }

    // Use this for initialization
    void Start() {
        LeapConnected = false;        
        controller = new Controller();
        while (!controller.IsServiceConnected)
            Debug.Log("Leap is connecting...");

        Debug.Log("Leap connected");
    }

    // Update is called once per frame
    void Update() {
        LeapConnected = controller.IsConnected;
        if (LeapConnected) {
            Frame frame = controller.Frame(); //  1- 60 access prev frames
            Frame prevFrame = null;
            if (prevFrameNum != 0)
                controller.Frame(prevFrameNum);

            //  Current frame
            handList = frame.Hands;
            numHands = handList.Count;
            if (numHands == 2)
            {
                if (handList[0].IsLeft)
                    handList.Reverse();

                #region Palm Data
                gripStrL = handList[0].GrabStrength;
                gripStrR = handList[1].GrabStrength;

                palmNormsL.x = handList[0].PalmNormal.x;
                palmNormsL.y = handList[0].PalmNormal.y;
                palmNormsL.z = handList[0].PalmNormal.z;

                palmNormsR.x = handList[1].PalmNormal.x;
                palmNormsR.y = handList[1].PalmNormal.y;
                palmNormsR.z = handList[1].PalmNormal.z;

                palmPosL.x = handList[0].PalmPosition.x;
                palmPosL.y = handList[0].PalmPosition.y;
                palmPosL.z = handList[0].PalmPosition.z;

                palmPosR.x = handList[1].PalmPosition.x;
                palmPosR.y = handList[1].PalmPosition.y;
                palmPosR.z = handList[1].PalmPosition.z;

                palmVelocityL.x = handList[0].PalmVelocity.x;
                palmVelocityL.y = handList[0].PalmVelocity.y;
                palmVelocityL.z = handList[0].PalmVelocity.z;

                palmVelocityR.x = handList[1].PalmVelocity.x;
                palmVelocityR.y = handList[1].PalmVelocity.y;
                palmVelocityR.z = handList[1].PalmVelocity.z;
                #endregion

                #region Finger Data
                fingerListL = handList[0].Fingers;
                fingerListR = handList[1].Fingers;

                numFingersL = fingerListL.Count;
                numFingersR = fingerListR.Count;

                numFingersLHeld = 0;
                numFingersRHeld = 0;

                for (int i = 0; i < fingerListL.Count; i++)
                    if (fingerListL[i].IsExtended)
                        numFingersLHeld++;

                for (int i = 0; i < fingerListR.Count; i++)
                    if (fingerListR[i].IsExtended)
                        numFingersRHeld++;
                #endregion

            }
            else if (numHands == 1)
            {
                if (handList[0].IsLeft)
                {
                    numFingersR = 0;
                    numFingersRHeld = 0;

                    #region Palm Data
                    palmPosL.x = handList[0].PalmPosition.x;
                    palmPosL.y = handList[0].PalmPosition.y;
                    palmPosL.z = handList[0].PalmPosition.z;
                    palmNormsL.x = handList[0].PalmNormal.x;
                    palmNormsL.y = handList[0].PalmNormal.y;
                    palmNormsL.z = handList[0].PalmNormal.z;
                    palmVelocityL.x = handList[0].PalmVelocity.x;
                    palmVelocityL.y = handList[0].PalmVelocity.y;
                    palmVelocityL.z = handList[0].PalmVelocity.z;
                    #endregion

                    #region Finger Data
                    fingerListL = handList[0].Fingers;
                    numFingersL = fingerListL.Count;
                    numFingersLHeld = 0;
                    for (int i = 0; i < fingerListL.Count; i++)
                        if (fingerListL[i].IsExtended)
                            numFingersLHeld++;
                    #endregion
                }
                else
                {
                    numFingersL = 0;
                    numFingersLHeld = 0;

                    #region Palm Data
                    palmPosR.x = handList[0].PalmPosition.x;
                    palmPosR.y = handList[0].PalmPosition.y;
                    palmPosR.z = handList[0].PalmPosition.z;
                    palmNormsR.x = handList[0].PalmNormal.x;
                    palmNormsR.y = handList[0].PalmNormal.y;
                    palmNormsR.z = handList[0].PalmNormal.z;
                    palmVelocityR.x = handList[0].PalmVelocity.x;
                    palmVelocityR.y = handList[0].PalmVelocity.y;
                    palmVelocityR.z = handList[0].PalmVelocity.z;
                    #endregion

                    #region Finger Data
                    fingerListR = handList[0].Fingers;
                    numFingersR = fingerListR.Count;
                    numFingersRHeld = 0;
                    for (int i = 0; i < fingerListR.Count; i++)
                        if (fingerListR[i].IsExtended)
                            numFingersRHeld++;
                    #endregion
                }
            }
            else
            {
                numHands = 0;
                numFingersL = 0;
                numFingersR = 0;
                numFingersLHeld = 0;
                numFingersRHeld = 0;
                palmPosL = Vector3.zero;
                palmPosR = Vector3.zero;
                palmNormsL = Vector3.zero;
                palmNormsR = Vector3.zero;
                palmVelocityL = Vector3.zero;
                palmVelocityR = Vector3.zero;
            }


            //  Previous frame
            if (prevFrame != null)
            {
                prevHandList = prevFrame.Hands;
                prevNumHands = prevHandList.Count;

                if (prevNumHands == 2)
                {
                    if (prevHandList[0].IsLeft)
                        prevHandList.Reverse();

                    #region Prev Palm Data
                    prevGripStrL = prevHandList[0].GrabStrength;
                    prevGripStrR = prevHandList[1].GrabStrength;
                    prevPalmNormL.x = prevHandList[0].PalmNormal.x;
                    prevPalmNormL.y = prevHandList[0].PalmNormal.y;
                    prevPalmNormL.z = prevHandList[0].PalmNormal.z;
                    prevPalmNormR.x = prevHandList[1].PalmNormal.x;
                    prevPalmNormR.y = prevHandList[1].PalmNormal.y;
                    prevPalmNormR.z = prevHandList[1].PalmNormal.z;
                    prevPalmPosL.x = prevHandList[0].PalmPosition.x;
                    prevPalmPosL.y = prevHandList[0].PalmPosition.y;
                    prevPalmPosL.z = prevHandList[0].PalmPosition.z;
                    prevPalmPosR.x = prevHandList[1].PalmPosition.x;
                    prevPalmPosR.y = prevHandList[1].PalmPosition.y;
                    prevPalmPosR.z = prevHandList[1].PalmPosition.z;
                    prevPalmVelocityL.x = prevHandList[0].PalmVelocity.x;
                    prevPalmVelocityL.y = prevHandList[0].PalmVelocity.y;
                    prevPalmVelocityL.z = prevHandList[0].PalmVelocity.z;
                    prevPalmVelocityR.x = prevHandList[1].PalmVelocity.x;
                    prevPalmVelocityR.y = prevHandList[1].PalmVelocity.y;
                    prevPalmVelocityR.z = prevHandList[1].PalmVelocity.z;
                    #endregion

                    #region Finger Data
                    prevFingerListL = prevHandList[0].Fingers;
                    prevFingerListR = prevHandList[1].Fingers;

                    prevNumFingersL = prevFingerListL.Count;
                    prevNumFingersR = prevFingerListR.Count;

                    prevFingersHeldL = 0;
                    prevFingersHeldR = 0;

                    for (int i = 0; i < prevFingerListL.Count; i++)
                        if (prevFingerListL[i].IsExtended)
                            prevFingersHeldL++;

                    for (int i = 0; i < prevFingerListR.Count; i++)
                        if (prevFingerListR[i].IsExtended)
                            prevFingersHeldR++;
                    #endregion

                }
                else if (prevNumHands == 1)
                {
                    prevNumFingersR = 0;
                    prevFingersHeldR = 0;

                    #region Palm Data
                    prevPalmPosL.x = prevHandList[0].PalmPosition.x;
                    prevPalmPosL.y = prevHandList[0].PalmPosition.y;
                    prevPalmPosL.z = prevHandList[0].PalmPosition.z;
                    prevPalmNormL.x = prevHandList[0].PalmNormal.x;
                    prevPalmNormL.y = prevHandList[0].PalmNormal.y;
                    prevPalmNormL.z = prevHandList[0].PalmNormal.z;
                    prevPalmVelocityL.x = prevHandList[0].PalmVelocity.x;
                    prevPalmVelocityL.y = prevHandList[0].PalmVelocity.y;
                    prevPalmVelocityL.z = prevHandList[0].PalmVelocity.z;
                    #endregion

                    #region Finger Data
                    prevFingerListL = prevHandList[0].Fingers;
                    prevNumFingersL = prevFingerListL.Count;
                    prevFingersHeldL = 0;
                    for (int i = 0; i < prevFingerListL.Count; i++)
                        if (prevFingerListL[i].IsExtended)
                            prevFingersHeldL++;
                    #endregion
                }
                else
                {
                    prevNumHands = 0;
                    prevNumFingersL = 0;
                    prevNumFingersR = 0;
                    prevFingersHeldL = 0;
                    prevFingersHeldR = 0;
                    prevPalmPosL = Vector3.zero;
                    prevPalmPosR = Vector3.zero;
                    prevPalmNormL = Vector3.zero;
                    prevPalmNormR = Vector3.zero;
                    prevPalmVelocityL = Vector3.zero;
                    prevPalmVelocityR = Vector3.zero;
                }
            }
        }
    }

    #region Private Funcs
    public bool GetBothHandsOpen() {
        if(GetNumLFingersHeld() == 5 && GetNumRFingersHeld() == 5)
            return true;

        return false;
    }

    public bool GetManualDriveSign() {
        if (numHands != 2)
            return false;

        bool indexL = false;
        bool indexR = false;
        if (GetNumLFingersHeld() == 1 && GetNumRFingersHeld() == 1) {
            for (int x = 0; x < numFingersL; x++) {
                if (fingerListL[x].Type == Finger.FingerType.TYPE_INDEX && fingerListL[x].IsExtended)
                    indexL = true;

                if (fingerListR[x].Type == Finger.FingerType.TYPE_INDEX && fingerListR[x].IsExtended)
                    indexR = true;
            }
        }

        if(GetLPalmNormals().y > 0.8f && GetRPalmNormals().y > 0.8f) 
            if (indexL && indexR)
                return true;

        return false;
    }

    public bool GetAutoPilotSign() {
        if (numHands != 2)
            return false;

        bool indexL = false;
        bool indexR = false;
        if (GetNumLFingersHeld() < 2 && GetNumRFingersHeld() < 2) {
            for (int x = 0; x < numFingersL; x++) {
                if (fingerListL[x].Type == Finger.FingerType.TYPE_INDEX && fingerListL[x].IsExtended)
                    indexL = true;

                if (fingerListR[x].Type == Finger.FingerType.TYPE_INDEX && fingerListR[x].IsExtended)
                    indexR = true;
            }
        }

        if (GetLPalmNormals().y < -0.5f && GetRPalmNormals().y < -0.5f)
            if (indexL && indexR)
                return true;

        return false;
    }

    public bool GetSettingsSign()
    {
        return true;
    }


    public bool GetForearmCollision()
    {
        if (GetNumRFingersHeld() == 1)
        {
            for (int x = 0; x < numFingersR; x++)
            {
                if (fingerListR[x].Type == Finger.FingerType.TYPE_INDEX && fingerListR[x].IsExtended)
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    #region Accessors
    public bool GetIsLeapConnected() {
        return LeapConnected;
    }


    public int GetNumHands() {
        return numHands;
    }
    public int GetNumLFingersHeld() {
        return numFingersLHeld;
    }
    public int GetNumRFingersHeld() {
        return numFingersRHeld;
    }
    public int GetPrevNumHands() {
        return prevNumHands;
    }
    public int GetPrevNumLFingersHeld() {
        return prevFingersHeldL;
    }
    public int GetPrevNumRFingersHeld() {
        return prevFingersHeldR;
    }



    public float GetLGripStrength() {
        return gripStrL;
    }
    public float GetRGripStrength() {
        return gripStrR;
    }
    public float GetPrevLGripStrength() {
        return prevGripStrL;
    }
    public float GetPrevRGripStrength() {
        return prevGripStrR;
    }



    public Vector3 GetLPalmPosition() {
        return palmPosL;
    }
    public Vector3 GetRPalmPosition() {
        return palmPosR;
    }
    public Vector3 GetLPalmNormals() {
        return palmNormsL;
    }
    public Vector3 GetRPalmNormals() {
        return palmNormsR;
    }
    public Vector3 GetLPalmVelocity() {
        return palmVelocityL;
    }
    public Vector3 GetRPalmVelocity() {
        return palmVelocityR;
    }
    public Vector3 GetPrevLPalmPosition() {
        return prevPalmPosL;
    }
    public Vector3 GetPrevRPalmPosition() {
        return prevPalmPosR;
    }
    public Vector3 GetPrevLPalmNormals() {
        return prevPalmNormL;
    }
    public Vector3 GetPrevRPalmNormals() {
        return prevPalmNormR;
    }
    public Vector3 GetPrevLPalmVelocity() {
        return prevPalmVelocityL;
    }
    public Vector3 GetPrevRPalmVelocity() {
        return prevPalmVelocityR;
    }
    #endregion
}