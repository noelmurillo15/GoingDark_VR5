using UnityEngine;

public class PlayerStats : MonoBehaviour {
    //**    Attach to Player    **//
    //  Movement
    public float moveSpeed;
    public float maxSpeed;
    public float rotateSpeed;
    public float acceleration;
    //  Weapons
    public int numMissiles;
    //  Credits ( Money )
    public int numCredits;

    // Use this for initialization
    void Start () {
        moveSpeed = 0f;
        maxSpeed = 50f;
        numMissiles = 10;
        rotateSpeed = 20f;
        acceleration = 2.0f;
        numCredits = PlayerPrefs.GetInt("Credits", 100);
        //pgmanager.GetComponent<PersistentGameManager>().SendMessage("GetPlayerCredits", numCredits);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Accessors
    public int GetNumMissiles()
    {
        return numMissiles;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public float GetRotateSpeed()
    {
        return rotateSpeed;
    }
    #endregion

    #region Modifiers
    public void DecreaseMissileCount()
    {
        numMissiles--;
    }

    public void IncreaseSpeed(float percentage)
    {
        if (moveSpeed < (maxSpeed * percentage))
            moveSpeed += Time.deltaTime * acceleration;
        else if (moveSpeed > (maxSpeed * percentage) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (moveSpeed > 0.0f)
            moveSpeed -= Time.deltaTime * acceleration * 2.0f;
        else
            moveSpeed = 0.0f;
    }
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
    }

    public void Hit()
    {
        Debug.Log("Player Has been Hit");
    }

    public void Kill()
    {
        Debug.Log("Destroyed Player Ship");
    }
    #endregion
}
