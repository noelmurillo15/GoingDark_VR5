using UnityEngine;

namespace GoingDark.Core.Manager
{
    public class EventsManager : MonoBehaviour
    {
        public delegate void GeneralEvent();
        public event GeneralEvent MyGenEvent;


        public void CallGeneralEvent()
        {
            if(MyGenEvent != null)
            {
                MyGenEvent();
            }
        }
    }
}
